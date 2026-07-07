using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms.Integration;
using System.Windows.Threading;
using AutoSaveAddin.Model;
using Aveva.Core.Database;
using Aveva.Core.Utilities.CommandLine;

namespace AutoSaveAddin
{
    public static class AutoSaveServer
    {
        private static readonly object SyncRoot = new object();
        private static Dispatcher _mainThreadDispatcher;

        static AutoSaveServer()
        {
            _mainThreadDispatcher = Dispatcher.CurrentDispatcher;
        }

        private static Settings _settings;
        private static CancellationTokenSource _cancellation;

        private const string AutoSaveMessage = "\u0420\u0430\u0431\u043e\u0442\u0430 \u0441\u043e\u0445\u0440\u0430\u043d\u0435\u043d\u0430.";
        private const string AutoSaveErrorMessage = "\u0410\u0432\u0442\u043e\u0441\u043e\u0445\u0440\u0430\u043d\u0435\u043d\u0438\u0435 \u043d\u0435 \u0443\u0434\u0430\u043b\u043e\u0441\u044c.";
        private const string AutoSaveCancelMessage = "\u0410\u0432\u0442\u043e\u0441\u043e\u0445\u0440\u0430\u043d\u0435\u043d\u0438\u0435 \u043e\u0442\u043c\u0435\u043d\u0435\u043d\u043e.";
        private const string AutoSaveServerStart = "AutoSave server started.";
        private const string AutoSaveServerStop = "AutoSave server stopped.";

        private static Task ActiveTask;

        public static void Start()
        {
            lock (SyncRoot)
            {
                if (ActiveTask != null && !ActiveTask.IsCompleted)
                    return;

                _settings = SettingsStorage.Load();

                _cancellation = new CancellationTokenSource();
                Trace.WriteLine(AutoSaveServerStart);
                ActiveTask = Task.Factory.StartNew(
                    () => Run(_cancellation.Token),
                    _cancellation.Token,
                    TaskCreationOptions.LongRunning,
                    TaskScheduler.Default);
            }
        }

        private static void Run(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    if (token.WaitHandle.WaitOne((int)_settings.Delay.TotalMilliseconds))
                        break;

                    if (!_settings.Enabled)
                        continue;

                    bool saving = true;
                    if (_settings.IsNeedRequest)
                    {
                        bool? result = _mainThreadDispatcher.Invoke(new Func<bool?>(ShowSaveConfirmation));
                        saving = result == true;

                        if (!saving)
                            Trace.WriteLine(AutoSaveCancelMessage);
                    }

                    if (saving)
                        _mainThreadDispatcher.Invoke(new Action(SaveWork));
                }
                catch (Exception ex)
                {
                    string message = AutoSaveErrorMessage + " " + ex.Message;
                    PrintUserMessage(message);
                }
            }
        }

        private static void SaveWork()
        {
            try
            {
                if (Aveva.Core.Database.MDB.CurrentMDB.SaveWork("Autosave"))
                {
                    PrintUserMessage(CreateAutoSaveCompletedMessage());

                    if (_settings.UnclaimAfterSave)
                    {
                        PrintUserMessage("AutoSave: running UNCLAIM ALL");
                        UnclaimAll();
                    }
                }
                else
                {
                    PrintUserMessage(AutoSaveErrorMessage);
                }
            }
            catch (Exception ex)
            {
                string message = AutoSaveErrorMessage + " " + ex.Message;
                PrintUserMessage(message);
            }
        }

        private static string CreateAutoSaveCompletedMessage()
        {
            return "\u0412\u044b\u043f\u043e\u043b\u043d\u0435\u043d\u043e \u0430\u0432\u0442\u043e\u0441\u043e\u0445\u0440\u0430\u043d\u0435\u043d\u0438\u0435 " + DateTime.Now.ToString("HH:mm:ss");
        }

        private static bool? ShowSaveConfirmation()
        {
            MessageBoxViewModel vm = new MessageBoxViewModel();
            vm.Time = (int)_settings.CloseDelay.TotalSeconds;

            MessageBoxT mb = new MessageBoxT
            {
                DataContext = vm
            };

            ElementHost.EnableModelessKeyboardInterop(mb);
            return mb.ShowDialog();
        }

        public static void PrintInfo(string text)
        {
            PrintUserMessage(text);
        }

        private static void UnclaimAll()
        {
            try
            {
                MDB.CurrentMDB.ReleaseAll();
                PrintUserMessage("AutoSave: ReleaseAll completed");
            }
            catch (Exception ex)
            {
                string message = "AutoSave: ReleaseAll failed: " + ex.Message;
                PrintUserMessage(message);
            }
        }

        private static void PrintUserMessage(string text)
        {
            Console.WriteLine(text);
            Trace.WriteLine(text);
            PdmsPrint(text);
        }

        private static void PdmsPrint(string text)
        {
            try
            {
                Command.CreateCommand(string.Format("$p {0}", text)).RunInPdms();
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
        }

        public static void Stop()
        {
            lock (SyncRoot)
            {
                if (_cancellation != null)
                {
                    _cancellation.Cancel();
                    _cancellation.Dispose();
                    _cancellation = null;
                }

                ActiveTask = null;
                Trace.WriteLine(AutoSaveServerStop);
            }
        }

        public static void Restart()
        {
            Stop();
            Start();
        }
    }
}

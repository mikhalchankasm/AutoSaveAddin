using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AutoIt;
using AutoSaveAddin.Model;
using Aveva.Core.Utilities.CommandLine;
using Newtonsoft.Json;
using NLog;

namespace AutoSaveAddin
{
    public static class AutoSaveServer
    {
        private static ILogger Logger;

        static AutoSaveServer()
        {
            Logger = LogManager.GetCurrentClassLogger();
        }
        
        private static bool _running;

        private static Settings _settings;

        private static int _interval = (int)(0.3 * 60 * 1000);
        private static int _time = 5 * 1000;
        private const string Title = "Подтверждение";
        private const string Text = "Сохранить работу. Вы уверены?";
        private const string AutoSaveMessage = "Работа сохранена.";
        private const string AutoSaveErrorMessage = "Автосохранение не удалось.";
        private const string AutoSaveCancelMessage = "Автосохранение отменено.";
        private const string AutoSaveServerStart = "Запуск сервера автосохранений.";
        private const string AutoSaveServerStop = "Остановка сервера автосохранений.";

        private static Task ActiveTask;

        public static void Start()
        {
            if (ActiveTask is null)
            {
                if (File.Exists(Environment.SettingPath))
                {
                    _settings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(Environment.SettingPath));
                }
                else
                {
                    _settings = Settings.GetDefault();
                    File.WriteAllText(Environment.SettingPath, JsonConvert.SerializeObject(_settings));
                }

                Logger.Debug(AutoSaveServerStart);
                ActiveTask = Task.Factory.StartNew(Run);
            }
        }

        private static void Run()
        {
            _running = true;
            while (_running)
            {
                Thread.Sleep((int)_settings.Delay.TotalMilliseconds);

                bool saving = true;
                Task<DialogResult> task = Task.Run(() =>
                {
                    return MessageBox.Show(Text, Title, MessageBoxButtons.YesNo);
                });

                Thread.Sleep((int)_settings.CloseDelay.TotalMilliseconds);
                if (!task.IsCompleted)
                {
                    IntPtr handle = AutoItX.WinGetHandle("Подтверждение");
                    AutoItX.WinActivate(handle);
                    AutoItX.Send("{Enter}");
                }

                DialogResult dialogResult = task.Result;
                switch (dialogResult)
                {
                    case DialogResult.Yes:
                        break;
                    default:
                        saving = false;
                        PdmsPrint(AutoSaveCancelMessage);
                        Logger.Info(AutoSaveCancelMessage);
                        break;
                }

                if (saving)
                {
                    if (Aveva.Core.Database.MDB.CurrentMDB.SaveWork("Autosave"))
                    {
                        PdmsPrint(AutoSaveMessage);
                        Logger.Info(AutoSaveMessage);
                    }
                    else
                    {
                        PdmsPrint(AutoSaveErrorMessage);
                        Logger.Info(AutoSaveErrorMessage);
                    }
                }
            }
        }

        private static void PdmsPrint(string text)
        {
            Command.CreateCommand($"q '{text}'").RunInPdms();
        }

        public static void Stop()
        {
            _running = false;
            ActiveTask = null;
            Logger.Debug(AutoSaveServerStop);
        }

        public static void Restart()
        {
            Stop();
            Start();
        }
    }
}
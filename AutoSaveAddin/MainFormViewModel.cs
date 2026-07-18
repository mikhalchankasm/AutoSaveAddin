using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using AutoSaveAddin.Lib;
using AutoSaveAddin.Localization;
using AutoSaveAddin.Model;

namespace AutoSaveAddin
{
    public class MainFormViewModel : NotifyBase
    {
        private Settings _settings;
        private Settings _lastSavedSettings;
        private string _statusText;

        public Settings Settings
        {
            get { return _settings; }
            set { SetProperty(ref _settings, value); }
        }

        public string StatusText
        {
            get { return _statusText; }
            set { SetProperty(ref _statusText, value); }
        }

        public MainFormViewModel()
        {
            Settings = SettingsStorage.Load();
            _lastSavedSettings = CloneSettings(Settings);
            StatusText = string.Empty;
        }

        private ICommand _saveCommand;
        private ICommand _increaseDelayCommand;
        private ICommand _decreaseDelayCommand;
        private ICommand _increaseCloseDelayCommand;
        private ICommand _decreaseCloseDelayCommand;
        private ICommand _openSettingsFileCommand;
        private ICommand _normalizeSettingsFileCommand;

        public ICommand SaveCommand
        {
            get
            {
                return _saveCommand ?? (_saveCommand = new RelayCommand(OnSaveCommandExecuted));
            }
        }

        public ICommand IncreaseDelayCommand
        {
            get
            {
                return _increaseDelayCommand ?? (_increaseDelayCommand = new RelayCommand(delegate { ChangeDelay(1); }));
            }
        }

        public ICommand DecreaseDelayCommand
        {
            get
            {
                return _decreaseDelayCommand ?? (_decreaseDelayCommand = new RelayCommand(delegate { ChangeDelay(-1); }));
            }
        }

        public ICommand IncreaseCloseDelayCommand
        {
            get
            {
                return _increaseCloseDelayCommand ?? (_increaseCloseDelayCommand = new RelayCommand(delegate { ChangeCloseDelay(5); }));
            }
        }

        public ICommand DecreaseCloseDelayCommand
        {
            get
            {
                return _decreaseCloseDelayCommand ?? (_decreaseCloseDelayCommand = new RelayCommand(delegate { ChangeCloseDelay(-5); }));
            }
        }

        public ICommand OpenSettingsFileCommand
        {
            get
            {
                return _openSettingsFileCommand ?? (_openSettingsFileCommand = new RelayCommand(OnOpenSettingsFileCommandExecuted));
            }
        }

        public ICommand NormalizeSettingsFileCommand
        {
            get
            {
                return _normalizeSettingsFileCommand ?? (_normalizeSettingsFileCommand = new RelayCommand(OnNormalizeSettingsFileCommandExecuted));
            }
        }

        private void OnSaveCommandExecuted(object p)
        {
            Settings oldSettings = CloneSettings(_lastSavedSettings);
            SettingsStorage.Save(Settings);
            _lastSavedSettings = CloneSettings(Settings);

            string message = string.Format(UiText.SettingsSavedMessageFormat, DescribeSettings(oldSettings), DescribeSettings(Settings));

            StatusText = UiText.SettingsSavedStatus;

            AutoSaveServer.Restart();
            Dispatcher.CurrentDispatcher.BeginInvoke(
                new Action(delegate { ShowSavedMessage(message); }),
                DispatcherPriority.ApplicationIdle);
        }

        private static void ShowSavedMessage(string message)
        {
            Window owner = AddinUiContext.MainForm;
            if (owner != null)
            {
                MessageBox.Show(
                    owner,
                    message,
                    "AutoSaveAddin",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show(
                    message,
                    "AutoSaveAddin",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }

        private void OnOpenSettingsFileCommandExecuted(object p)
        {
            try
            {
                Process.Start(SettingsStorage.SettingsPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    UiText.OpenSettingsErrorPrefix + "\r\n" + ex.Message,
                    "AutoSaveAddin",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void OnNormalizeSettingsFileCommandExecuted(object p)
        {
            try
            {
                SettingsStorage.NormalizeFile();
                StatusText = "JSON OK";
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    UiText.WriteJsonErrorPrefix + "\r\n" + ex.Message,
                    "AutoSaveAddin",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void ChangeDelay(int deltaMinutes)
        {
            int minutes = Math.Max(1, (int)Math.Round(Settings.Delay.TotalMinutes) + deltaMinutes);
            Settings.Delay = TimeSpan.FromMinutes(minutes);
        }

        private void ChangeCloseDelay(int deltaSeconds)
        {
            int seconds = Math.Max(1, (int)Math.Round(Settings.CloseDelay.TotalSeconds) + deltaSeconds);
            Settings.CloseDelay = TimeSpan.FromSeconds(seconds);
        }

        private static Settings CloneSettings(Settings settings)
        {
            return new Settings
            {
                Enabled = settings.Enabled,
                IsNeedRequest = settings.IsNeedRequest,
                RequireConfirmationToSave = settings.RequireConfirmationToSave,
                UnclaimAfterSave = settings.UnclaimAfterSave,
                Delay = settings.Delay,
                CloseDelay = settings.CloseDelay
            };
        }

        private static string DescribeSettings(Settings settings)
        {
            return string.Format(
                UiText.SettingsDescriptionFormat,
                settings.Enabled,
                settings.IsNeedRequest,
                settings.RequireConfirmationToSave,
                settings.UnclaimAfterSave,
                (int)Math.Round(settings.Delay.TotalMinutes),
                (int)Math.Round(settings.CloseDelay.TotalSeconds));
        }
    }
}

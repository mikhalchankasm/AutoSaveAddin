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
        private Settings _Settings;
        private Settings _lastSavedSettings;
        private string _StatusText;

        public Settings Settings
        {
            get { return _Settings; }
            set { SetProperty(ref _Settings, value); }
        }

        public string StatusText
        {
            get { return _StatusText; }
            set { SetProperty(ref _StatusText, value); }
        }

        public MainFormViewModel()
        {
            Settings = SettingsStorage.Load();
            _lastSavedSettings = CloneSettings(Settings);
            StatusText = string.Empty;
        }

        private ICommand _SaveCommand;
        private ICommand _IncreaseDelayCommand;
        private ICommand _DecreaseDelayCommand;
        private ICommand _IncreaseCloseDelayCommand;
        private ICommand _DecreaseCloseDelayCommand;
        private ICommand _OpenSettingsFileCommand;
        private ICommand _NormalizeSettingsFileCommand;

        public ICommand SaveCommand
        {
            get
            {
                return _SaveCommand ?? (_SaveCommand = new RelayCommand(OnSaveCommandExecuted));
            }
        }

        public ICommand IncreaseDelayCommand
        {
            get
            {
                return _IncreaseDelayCommand ?? (_IncreaseDelayCommand = new RelayCommand(delegate { ChangeDelay(1); }));
            }
        }

        public ICommand DecreaseDelayCommand
        {
            get
            {
                return _DecreaseDelayCommand ?? (_DecreaseDelayCommand = new RelayCommand(delegate { ChangeDelay(-1); }));
            }
        }

        public ICommand IncreaseCloseDelayCommand
        {
            get
            {
                return _IncreaseCloseDelayCommand ?? (_IncreaseCloseDelayCommand = new RelayCommand(delegate { ChangeCloseDelay(5); }));
            }
        }

        public ICommand DecreaseCloseDelayCommand
        {
            get
            {
                return _DecreaseCloseDelayCommand ?? (_DecreaseCloseDelayCommand = new RelayCommand(delegate { ChangeCloseDelay(-5); }));
            }
        }

        public ICommand OpenSettingsFileCommand
        {
            get
            {
                return _OpenSettingsFileCommand ?? (_OpenSettingsFileCommand = new RelayCommand(OnOpenSettingsFileCommandExecuted));
            }
        }

        public ICommand NormalizeSettingsFileCommand
        {
            get
            {
                return _NormalizeSettingsFileCommand ?? (_NormalizeSettingsFileCommand = new RelayCommand(OnNormalizeSettingsFileCommandExecuted));
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
            Window owner = Environment.MainForm;
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

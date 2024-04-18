using System.IO;
using System.Windows.Input;
using AutoSaveAddin.Lib;
using AutoSaveAddin.Model;
using Newtonsoft.Json;

namespace AutoSaveAddin
{
    public class MainFormViewModel : NotifyBase
    {
        #region Settings : Settings - Настройки

        private Settings _Settings;

        /// <summary>Настройки</summary>
        public Settings Settings
        {
            get => _Settings;
            set => SetProperty(ref _Settings, value);
        }

        #endregion

        public MainFormViewModel()
        {
            if (File.Exists(Environment.SettingPath))
            {
                Settings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(Environment.SettingPath));
            }
            else
            {
                string directory = Path.GetDirectoryName(Environment.SettingPath);
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                Settings = Settings.GetDefault();
                File.WriteAllText(Environment.SettingPath, JsonConvert.SerializeObject(Settings));
            }
        }

        #region SaveCommand : ICommand - Сохранение

        private ICommand _SaveCommand;

        /// <summary>Сохранение</summary>
        public ICommand SaveCommand =>
            _SaveCommand ?? (_SaveCommand = new RelayCommand(OnSaveCommandExecuted, CanSaveCommandExecute));

        private void OnSaveCommandExecuted(object p)
        {
            string directory = Path.GetDirectoryName(Environment.SettingPath);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            File.WriteAllText(Environment.SettingPath, JsonConvert.SerializeObject(Settings));
            
            AutoSaveServer.Restart();
        }

        private bool CanSaveCommandExecute(object p)
        {
            return true;
        }

        #endregion
    }
}
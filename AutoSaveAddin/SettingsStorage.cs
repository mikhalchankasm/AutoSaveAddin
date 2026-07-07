using System.IO;
using System.Reflection;
using AutoSaveAddin.Model;
using Newtonsoft.Json;

namespace AutoSaveAddin
{
    public static class SettingsStorage
    {
        public static string SettingsPath
        {
            get
            {
                return Path.Combine(
                    System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData),
                    "AutoSaveAddin",
                    "settings.json");
            }
        }

        private static string LegacySettingsPath
        {
            get
            {
                return Path.Combine(
                    Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                    "Resources",
                    "AutoSaveAddin",
                    "settings.json");
            }
        }

        public static Settings Load()
        {
            EnsureSettingsFile();

            try
            {
                Settings settings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(SettingsPath));
                if (settings != null)
                {
                    Save(settings);
                    return settings;
                }
            }
            catch
            {
            }

            Settings defaultSettings = Settings.GetDefault();
            Save(defaultSettings);
            return defaultSettings;
        }

        public static void Save(Settings settings)
        {
            string directory = Path.GetDirectoryName(SettingsPath);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            File.WriteAllText(SettingsPath, JsonConvert.SerializeObject(settings, Formatting.Indented));
        }

        public static void NormalizeFile()
        {
            Settings settings = Load();
            Save(settings);
        }

        private static void EnsureSettingsFile()
        {
            string directory = Path.GetDirectoryName(SettingsPath);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            if (File.Exists(SettingsPath))
                return;

            if (File.Exists(LegacySettingsPath))
            {
                File.Copy(LegacySettingsPath, SettingsPath, false);
                return;
            }

            Save(Settings.GetDefault());
        }
    }
}

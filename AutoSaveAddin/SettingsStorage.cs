using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using AutoSaveAddin.Model;

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
                Settings settings = Deserialize(File.ReadAllText(SettingsPath));
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

            File.WriteAllText(SettingsPath, Serialize(settings), Encoding.UTF8);
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

        private static Settings Deserialize(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return null;

            SettingsDto dto;
            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(SettingsDto));
                dto = serializer.ReadObject(stream) as SettingsDto;
            }

            if (dto == null)
                return null;

            Settings defaults = Settings.GetDefault();
            return new Settings
            {
                Enabled = dto.Enabled.HasValue ? dto.Enabled.Value : defaults.Enabled,
                IsNeedRequest = dto.IsNeedRequest.HasValue ? dto.IsNeedRequest.Value : defaults.IsNeedRequest,
                RequireConfirmationToSave = dto.RequireConfirmationToSave.HasValue ? dto.RequireConfirmationToSave.Value : defaults.RequireConfirmationToSave,
                UnclaimAfterSave = dto.UnclaimAfterSave.HasValue ? dto.UnclaimAfterSave.Value : defaults.UnclaimAfterSave,
                Delay = ParseTimeSpan(dto.Delay, defaults.Delay),
                CloseDelay = ParseTimeSpan(dto.CloseDelay, defaults.CloseDelay)
            };
        }

        private static string Serialize(Settings settings)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("{");
            builder.AppendLine("  \"Enabled\": " + ToJson(settings.Enabled) + ",");
            builder.AppendLine("  \"IsNeedRequest\": " + ToJson(settings.IsNeedRequest) + ",");
            builder.AppendLine("  \"RequireConfirmationToSave\": " + ToJson(settings.RequireConfirmationToSave) + ",");
            builder.AppendLine("  \"UnclaimAfterSave\": " + ToJson(settings.UnclaimAfterSave) + ",");
            builder.AppendLine("  \"Delay\": \"" + settings.Delay.ToString() + "\",");
            builder.AppendLine("  \"CloseDelay\": \"" + settings.CloseDelay.ToString() + "\"");
            builder.AppendLine("}");
            return builder.ToString();
        }

        private static TimeSpan ParseTimeSpan(string value, TimeSpan defaultValue)
        {
            TimeSpan result;
            return TimeSpan.TryParse(value, out result) ? result : defaultValue;
        }

        private static string ToJson(bool value)
        {
            return value ? "true" : "false";
        }

        [DataContract]
        private class SettingsDto
        {
            [DataMember]
            public bool? Enabled { get; set; }

            [DataMember]
            public bool? IsNeedRequest { get; set; }

            [DataMember]
            public bool? RequireConfirmationToSave { get; set; }

            [DataMember]
            public bool? UnclaimAfterSave { get; set; }

            [DataMember]
            public string Delay { get; set; }

            [DataMember]
            public string CloseDelay { get; set; }
        }
    }
}

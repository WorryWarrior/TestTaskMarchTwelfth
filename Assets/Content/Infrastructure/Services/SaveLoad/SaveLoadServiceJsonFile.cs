using System.IO;
using System.Threading.Tasks;
using Content.Data;
using Content.Infrastructure.Services.Logging;
using Content.Infrastructure.Services.PersistentData;
using Newtonsoft.Json;
using static Newtonsoft.Json.JsonConvert;

namespace Content.Infrastructure.Services.SaveLoad
{
    public class SaveLoadServiceJsonFile : ISaveLoadService
    {
        private const string SettingsSaveFileName = "Settings.json";
        private const string ProgressSaveFileName = "Progress.json";

        private readonly IPersistentDataService _persistentDataService;
        private readonly ILoggingService _loggingService;

        public SaveLoadServiceJsonFile(
            IPersistentDataService persistentDataService,
            ILoggingService loggingService)
        {
            _persistentDataService = persistentDataService;
            _loggingService = loggingService;
        }

        public void SaveSettings()
        {
            string settingsJson = SerializeObject(_persistentDataService.Settings, Formatting.Indented);

            using StreamWriter sw = File.CreateText(GetSettingsSaveFilePath());
            sw.WriteLine(settingsJson);
        }

        public Task<SettingsData> LoadSettings()
        {
            if (!File.Exists(GetSettingsSaveFilePath()))
                return Task.FromResult((SettingsData)null);

            SettingsData settingsData = DeserializeObject<SettingsData>(File.ReadAllText(GetSettingsSaveFilePath()));

            _loggingService.LogMessage($"Loaded settings from file at {GetSettingsSaveFilePath()}", this);

            return Task.FromResult(settingsData);
        }

        public void SaveProgress()
        {
            string progressJson = SerializeObject(_persistentDataService.Progress, Formatting.Indented);

            using StreamWriter sw = File.CreateText(GetProgressSaveFilePath());
            sw.WriteLine(progressJson);
        }

        public Task<ProgressData> LoadProgress()
        {
            if (!File.Exists(GetProgressSaveFilePath()))
                return Task.FromResult((ProgressData)null);

            ProgressData progressData = DeserializeObject<ProgressData>(File.ReadAllText(GetProgressSaveFilePath()));

            _loggingService.LogMessage($"Loaded progress from file at {GetProgressSaveFilePath()}", this);

            return Task.FromResult(progressData);
        }

        private string GetSettingsSaveFilePath() =>
            $"{UnityEngine.Application.persistentDataPath}{Path.DirectorySeparatorChar}{SettingsSaveFileName}";
        private string GetProgressSaveFilePath() =>
            $"{UnityEngine.Application.persistentDataPath}{Path.DirectorySeparatorChar}{ProgressSaveFileName}";
    }
}
using System.Threading.Tasks;
using Content.Data;
using Content.Infrastructure.Services.PersistentData;
using UnityEngine;
using static Newtonsoft.Json.JsonConvert;

namespace Content.Infrastructure.Services.SaveLoad
{
    public class SaveLoadServicePlayerPrefs : ISaveLoadService
    {
        private const string SettingsKey = "Settings";
        private const string ProgressKey = "Progress";

        private readonly IPersistentDataService _persistentDataService;

        public SaveLoadServicePlayerPrefs(
            IPersistentDataService persistentDataService)
        {
            _persistentDataService = persistentDataService;
        }

        public void SaveSettings()
        {
            string settingsJson = SerializeObject(_persistentDataService.Settings);
            PlayerPrefs.SetString(SettingsKey, settingsJson);
        }

        public Task<SettingsData> LoadSettings()
        {
            SettingsData settings = DeserializeObject<SettingsData>(PlayerPrefs.GetString(SettingsKey));
            return Task.FromResult(settings);
        }

        public void SaveProgress()
        {
            string progressJson = SerializeObject(_persistentDataService.Progress);
            PlayerPrefs.SetString(ProgressKey, progressJson);
        }

        public Task<ProgressData> LoadProgress()
        {
            ProgressData progress = DeserializeObject<ProgressData>(PlayerPrefs.GetString(ProgressKey));
            return Task.FromResult(progress);
        }
    }
}
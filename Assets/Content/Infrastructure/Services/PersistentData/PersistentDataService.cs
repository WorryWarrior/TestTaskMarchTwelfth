using Content.Data;

namespace Content.Infrastructure.Services.PersistentData
{
    public class PersistentDataService : IPersistentDataService
    {
        public SettingsData Settings { get; set; }
        public ProgressData Progress { get; set; }
        public GameplayData Gameplay { get; set; }
    }
}
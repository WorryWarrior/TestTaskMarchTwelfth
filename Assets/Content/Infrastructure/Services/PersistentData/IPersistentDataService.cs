using Content.Data;

namespace Content.Infrastructure.Services.PersistentData
{
    public interface IPersistentDataService
    {
        SettingsData Settings { get; set; }
        ProgressData Progress { get; set; }
        GameplayData Gameplay { get; set; }
    }
}
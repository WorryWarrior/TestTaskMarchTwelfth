using System.Threading.Tasks;
using Content.Data;

namespace Content.Infrastructure.Services.SaveLoad
{
    public interface ISaveLoadService
    {
        void SaveSettings();
        Task<SettingsData> LoadSettings();

        void SaveProgress();
        Task<ProgressData> LoadProgress();
    }
}
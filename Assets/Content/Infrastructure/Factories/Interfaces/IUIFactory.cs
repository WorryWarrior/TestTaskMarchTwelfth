using System.Threading.Tasks;
using Content.UI;

namespace Content.Infrastructure.Factories.Interfaces
{
    public interface IUIFactory
    {
        GameplayHUDController GameplayHUD { get; }

        Task WarmUp();
        void CleanUp();
        Task CreateUIRoot();
        Task<MainMenuHUDController> CreateMainMenuHUD();
        Task<GameplayHUDController> CreateGameplayHUD();
        Task<ProgressWindowEntryController> CreateProgressWindowEntry(ProgressWindowController hud);
    }
}
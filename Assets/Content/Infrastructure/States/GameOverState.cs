using Content.Data;
using Content.Infrastructure.Factories.Interfaces;
using Content.Infrastructure.Services.PersistentData;
using Content.Infrastructure.Services.SaveLoad;
using Content.Infrastructure.States.Interfaces;

namespace Content.Infrastructure.States
{
    public class GameOverState : IPayloadedState<GameOverStateData>
    {
        private IUIFactory _uiFactory;
        private readonly IPersistentDataService _persistentDataService;
        private readonly ISaveLoadService _saveLoadService;

        public GameOverState(
            IUIFactory uiFactory,
            IPersistentDataService persistentDataService,
            ISaveLoadService saveLoadService)
        {
            _uiFactory = uiFactory;
            _persistentDataService = persistentDataService;
            _saveLoadService = saveLoadService;
        }

        public void Enter(GameOverStateData payload)
        {
            _persistentDataService.Progress.GameSessions.Add(new ProgressEntryData
            {
                PlayerName = _persistentDataService.Settings.PlayerName,
                SessionScore = payload.GameSessionScore
            });
            _saveLoadService.SaveProgress();

            _uiFactory.GameplayHUD.ShowGameOverScreen();
        }

        public void Exit()
        {
            Contexts.sharedInstance.game.DestroyAllEntities();
        }
    }
}
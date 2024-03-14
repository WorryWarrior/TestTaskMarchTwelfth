using System.Collections;
using Content.Infrastructure.Services.PersistentData;
using Content.Infrastructure.States;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Content.UI
{
    public class GameplayHUDController : MonoBehaviour
    {
        [SerializeField] private Button menuButton = null;
        [SerializeField] private GameOverWindowController gameOverWindowController = null;
        [SerializeField] private LeaderboardWindowController leaderboardWindowController = null;

        private GameStateMachine _gameStateMachine;
        private IPersistentDataService _persistentDataService;

        [Inject]
        private void Construct(
            GameStateMachine gameStateMachine,
            IPersistentDataService persistentDataService
            )
        {
            _gameStateMachine = gameStateMachine;
            _persistentDataService = persistentDataService;
        }

        public void Initialize()
        {
            gameOverWindowController.Initialize(_gameStateMachine);
            leaderboardWindowController.Initialize(_persistentDataService);

            menuButton.onClick.AddListener(() => _gameStateMachine.Enter<LoadMetaState>());
        }

        public void ShowGameOverScreen()
        {
            menuButton.gameObject.SetActive(false);
            leaderboardWindowController.gameObject.SetActive(false);

            gameOverWindowController.Show(.5f);
        }
    }
}
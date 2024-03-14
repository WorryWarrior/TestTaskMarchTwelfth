using System.Threading.Tasks;
using Content.Infrastructure.Services.Logging;
using Content.Infrastructure.Services.PersistentData;
using Content.Infrastructure.Services.SaveLoad;
using Content.Infrastructure.States;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Content.UI
{
    public class MainMenuHUDController : MonoBehaviour
    {
        [SerializeField] private SettingsWindowController settingsWindowController = null;
        [SerializeField] private ProgressWindowController progressWindowController = null;
        [SerializeField] private Button startButton = null;
        [SerializeField] private Button settingsButton = null;
        [SerializeField] private Button progressButton = null;

        private GameStateMachine _gameStateMachine;
        private ISaveLoadService _saveLoadService;
        private IPersistentDataService _persistentDataService;
        private ILoggingService _loggingService;

        [Inject]
        private void Construct(
            GameStateMachine gameStateMachine,
            ISaveLoadService saveLoadService,
            IPersistentDataService persistentDataService,
            ILoggingService loggingService
            )
        {
            _gameStateMachine = gameStateMachine;
            _saveLoadService = saveLoadService;
            _persistentDataService = persistentDataService;
            _loggingService = loggingService;
        }

        public async Task Initialize()
        {
            await SetupProgressWindow();
            SetupButtons();
            SetupSettingsWindow();
        }

        private void SetupButtons()
        {
            startButton.onClick.AddListener(() =>
            {
                _gameStateMachine.Enter<LoadLevelState>();
            });

            settingsButton.onClick.AddListener(() =>
            {
                settingsWindowController.Show(_persistentDataService.Settings);
            });

            progressButton.onClick.AddListener(() =>
            {
                progressWindowController.Show();
            });
        }

        private void SetupSettingsWindow()
        {
            settingsWindowController.Initialize();
            settingsWindowController.Hide();
            settingsWindowController.OnSettingsStateChanged += _saveLoadService.SaveSettings;
        }

        private async Task SetupProgressWindow()
        {
            await progressWindowController.Initialize();
            progressWindowController.Hide();
        }
    }
}
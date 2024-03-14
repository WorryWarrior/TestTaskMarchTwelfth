using System.Threading.Tasks;
using Content.Audio;
using Content.Gameplay;
using Content.Infrastructure.AssetManagement;
using Content.Infrastructure.Factories.Interfaces;
using Content.Infrastructure.SceneManagement;
using Content.Infrastructure.Services.Gameplay;
using Content.Infrastructure.Services.Logging;
using Content.Infrastructure.States.Interfaces;
using Content.UI;
using UnityEngine;

namespace Content.Infrastructure.States
{
    public class LoadLevelState : IState
    {
        private const string LevelSoundFileId = "AUD_Core";

        private readonly GameStateMachine _stateMachine;
        private readonly IUIFactory _uiFactory;
        private readonly IAudioFactory _audioFactory;
        private readonly IGameplayFactory _gameplayFactory;
        private readonly ISceneLoader _sceneLoader;
        private readonly IAssetProvider _assetProvider;
        private readonly IGameplayService _gameplayService;
        private readonly ILoggingService _loggingService;

        public LoadLevelState(
            GameStateMachine gameStateMachine,
            IUIFactory uiFactory,
            IAudioFactory audioFactory,
            IGameplayFactory gameplayFactory,
            ISceneLoader sceneLoader,
            IAssetProvider assetProvider,
            IGameplayService gameplayService,
            ILoggingService loggingService)
        {
            _stateMachine = gameStateMachine;
            _uiFactory = uiFactory;
            _audioFactory = audioFactory;
            _gameplayFactory = gameplayFactory;
            _sceneLoader = sceneLoader;
            _loggingService = loggingService;
            _gameplayService = gameplayService;
            _assetProvider = assetProvider;
        }

        public async void Enter()
        {
            await _gameplayFactory.WarmUp();
            await _sceneLoader.LoadScene(SceneName.Core, OnSceneLoaded);
        }

        public void Exit()
        {
            _gameplayFactory.CleanUp();
            _audioFactory.CleanUp();
        }

        private async void OnSceneLoaded(SceneName sceneName)
        {
            await InitUI();
            await InitGameWorld();

            _stateMachine.Enter<GameLoopState>();
        }

        private async Task InitUI()
        {
            await _uiFactory.CreateUIRoot();
            GameplayHUDController hudController = await _uiFactory.CreateGameplayHUD();
            hudController.Initialize();
        }

        private async Task InitGameWorld()
        {
            await PrepareGameplayService();
            await ConstructLevelAudioSource();
            await ConstructGameplaySimulation();
        }

        private async Task PrepareGameplayService()
        {
            _gameplayService.Initialize();
            await _gameplayService.CreateFoodPool();
            await _gameplayService.CreateEnemyPool();
        }

        private async Task ConstructLevelAudioSource()
        {
            AudioPlayerController audioSource = await _audioFactory.CreateAudioPlayer();
            audioSource.Initialize();
            AudioClip audioClip = await _assetProvider.Load<AudioClip>(LevelSoundFileId);
            audioSource.SetClipAndPlay(audioClip);
        }

        private async Task ConstructGameplaySimulation()
        {
            GameplaySimulationController controller = await _gameplayFactory.CreateGameplaySimulationController();
            controller.InitializeGameplay();
        }
    }
}
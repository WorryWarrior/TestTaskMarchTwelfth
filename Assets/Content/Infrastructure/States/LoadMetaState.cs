using System.Threading.Tasks;
using Content.Audio;
using Content.Infrastructure.AssetManagement;
using Content.Infrastructure.Factories.Interfaces;
using Content.Infrastructure.SceneManagement;
using Content.Infrastructure.States.Interfaces;
using Content.UI;
using UnityEngine;

namespace Content.Infrastructure.States
{
    public class LoadMetaState : IState
    {
        private const string MainMenuSoundFileId = "AUD_MainMenu";

        private readonly IUIFactory _uiFactory;
        private readonly IAudioFactory _audioFactory;
        private readonly ISceneLoader _sceneLoader;
        private readonly IAssetProvider _assetProvider;

        public LoadMetaState(
            IUIFactory uiFactory,
            IAudioFactory audioFactory,
            ISceneLoader sceneLoader,
            IAssetProvider assetProvider)
        {
            _uiFactory = uiFactory;
            _audioFactory = audioFactory;
            _sceneLoader = sceneLoader;
            _assetProvider = assetProvider;
        }

        public async void Enter()
        {
            await _assetProvider.Load<AudioClip>(MainMenuSoundFileId);
            await _uiFactory.WarmUp();
            await _audioFactory.WarmUp();

            await _sceneLoader.LoadScene(SceneName.Menu, OnMetaSceneLoaded);
        }

        public void Exit()
        {
            _uiFactory.CleanUp();
        }

        private async void OnMetaSceneLoaded(SceneName sceneName)
        {
            CleanupEntities();
            await ConstructUIRoot();
            await ConstructMainMenuHUD();
            await ConstructMainMenuAudioSource();
        }

        private void CleanupEntities() => Contexts.sharedInstance.game.DestroyAllEntities();

        private async Task ConstructUIRoot() => await _uiFactory.CreateUIRoot();

        private async Task ConstructMainMenuHUD()
        {
            MainMenuHUDController hudController = await _uiFactory.CreateMainMenuHUD();
            await hudController.Initialize();
        }

        private async Task ConstructMainMenuAudioSource()
        {
            AudioPlayerController audioSource = await _audioFactory.CreateAudioPlayer();
            audioSource.Initialize();
            AudioClip audioClip = await _assetProvider.Load<AudioClip>(MainMenuSoundFileId);
            audioSource.SetClipAndPlay(audioClip);
        }
    }
}
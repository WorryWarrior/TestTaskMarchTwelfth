using System.Threading.Tasks;
using Content.Audio;
using Content.Infrastructure.AssetManagement;
using Content.Infrastructure.Factories.Interfaces;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Content.Infrastructure.Factories
{
    public class AudioFactory : IAudioFactory
    {
        private const string AudioPlayerPrefabId = "PFB_AudioPlayer";

        private readonly IObjectResolver _container;
        private readonly IAssetProvider _assetProvider;

        public AudioFactory(
            LifetimeScope lifetimeScope,
            IAssetProvider assetProvider)
        {
            _container = lifetimeScope.Container;
            _assetProvider = assetProvider;
        }

        public async Task WarmUp()
        {
            await _assetProvider.Load<GameObject>(AudioPlayerPrefabId);
        }

        public void CleanUp()
        {
            _assetProvider.Release(AudioPlayerPrefabId);
        }

        public async Task<AudioPlayerController> CreateAudioPlayer()
        {
            GameObject prefab = await _assetProvider.Load<GameObject>(AudioPlayerPrefabId);
            AudioPlayerController audioPlayer = Object.Instantiate(prefab).GetComponent<AudioPlayerController>();

            _container.Inject(audioPlayer);
            return audioPlayer;
        }
    }
}
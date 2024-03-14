using System;
using System.Collections.Generic;
using Content.Infrastructure.Services.PersistentData;
using UniRx;
using UnityEngine;
using VContainer;

namespace Content.Audio
{
    public class AudioPlayerController : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource = null;

        private IPersistentDataService _persistentDataService;

        private readonly List<IDisposable> _subscriptions = new();

        [Inject]
        private void Construct(
            IPersistentDataService persistentDataService
        )
        {
            _persistentDataService = persistentDataService;
        }

        public void Initialize()
        {
            audioSource.mute = _persistentDataService.Settings.IsMuted.Value;
            audioSource.volume = _persistentDataService.Settings.MasterVolume.Value;

            IDisposable muteSubscription =
                _persistentDataService.Settings.IsMuted.Subscribe(value => audioSource.mute = value);
            IDisposable volumeSubscription =
                _persistentDataService.Settings.MasterVolume.Subscribe(value => audioSource.volume = value);

            _subscriptions.Add(muteSubscription);
            _subscriptions.Add(volumeSubscription);
        }

        public void SetClipAndPlay(AudioClip audioClip)
        {
            audioSource.clip = audioClip;
            audioSource.Play();
        }

        private void OnDestroy()
        {
            for (int i = 0; i < _subscriptions.Count; i++)
            {
                _subscriptions[i].Dispose();
            }
        }
    }
}
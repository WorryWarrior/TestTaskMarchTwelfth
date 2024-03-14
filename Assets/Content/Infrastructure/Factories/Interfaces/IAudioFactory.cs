using System.Threading.Tasks;
using Content.Audio;
using UnityEngine;

namespace Content.Infrastructure.Factories.Interfaces
{
    public interface IAudioFactory
    {
        Task WarmUp();
        void CleanUp();
        Task<AudioPlayerController> CreateAudioPlayer();
    }
}
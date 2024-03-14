using System;
using Content.Data.Enums;
using UniRx;

namespace Content.Data
{
    [Serializable]
    public class SettingsData
    {
        public ReactiveProperty<bool> IsMuted { get; set; }
        public ReactiveProperty<float> MasterVolume { get; set; }
        public PlayerColour PlayerColour { get; set; }
        public string PlayerName { get; set; }
    }
}
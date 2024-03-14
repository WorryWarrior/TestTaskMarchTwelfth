using System;
using Content.Data;
using Content.Data.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Content.UI
{
    public class SettingsWindowController : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup = null;
        [SerializeField] private Button closeButton = null;

        [SerializeField] private Toggle muteToggle = null;
        [SerializeField] private Slider volumeSlider = null;
        [SerializeField] private Toggle[] playerColourToggles = null;
        [SerializeField] private TMP_InputField playerNameInput = null;

        public event Action OnSettingsStateChanged;

        private SettingsData _settings;

        public void Initialize()
        {
            closeButton.onClick.AddListener(Hide);

            muteToggle.onValueChanged.AddListener(value =>
            {
                _settings.IsMuted.Value = value;
                OnSettingsStateChanged?.Invoke();
            });
            volumeSlider.onValueChanged.AddListener(value =>
            {
                _settings.MasterVolume.Value = value;
                OnSettingsStateChanged?.Invoke();
            });
            for (int i = 0; i < playerColourToggles.Length; i++)
            {
                int toggleIndex = i;
                playerColourToggles[i].onValueChanged.AddListener(value =>
                {
                    if (value)
                    {
                        _settings.PlayerColour = (PlayerColour)toggleIndex;
                        OnSettingsStateChanged?.Invoke();
                    }
                });
            }
            playerNameInput.onValueChanged.AddListener(value =>
            {
                _settings.PlayerName = value;
                OnSettingsStateChanged?.Invoke();
            });
        }

        public void Show(SettingsData settings)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;

            _settings = settings;

            muteToggle.isOn = _settings.IsMuted.Value;
            volumeSlider.value = _settings.MasterVolume.Value;

            int playerColourIndex = (int)_settings.PlayerColour;
            for (int i = 0; i < playerColourToggles.Length; i++)
            {
                playerColourToggles[i].isOn = i == playerColourIndex;
            }

            playerNameInput.text = _settings.PlayerName;
        }

        public void Hide()
        {
            canvasGroup.alpha = 0f;
            canvasGroup.blocksRaycasts = false;
        }
    }
}
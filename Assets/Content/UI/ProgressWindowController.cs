using System.Collections.Generic;
using System.Threading.Tasks;
using Content.Data;
using Content.Infrastructure.Factories.Interfaces;
using Content.Infrastructure.Services.PersistentData;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Content.UI
{
    public class ProgressWindowController : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup = null;
        [SerializeField] private Button closeButton = null;

        public Transform progressWindowEntryContainer;

        private IUIFactory _uiFactory;
        private IPersistentDataService _persistentDataService;

        [Inject]
        private void Construct(
            IUIFactory uiFactory,
            IPersistentDataService persistentDataService
            )
        {
            _uiFactory = uiFactory;
            _persistentDataService = persistentDataService;
        }

        public async Task Initialize()
        {
            await SetupProgressEntries();
            closeButton.onClick.AddListener(Hide);
        }

        public void Show()
        {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
        }

        public void Hide()
        {
            canvasGroup.alpha = 0f;
            canvasGroup.blocksRaycasts = false;
        }

        private async Task SetupProgressEntries()
        {
            List<ProgressEntryData> gameSessions = _persistentDataService.Progress.GameSessions;
            gameSessions.Sort((a, b) => b.SessionScore.CompareTo(a.SessionScore));
            foreach (ProgressEntryData gameSession in gameSessions)
            {
                ProgressWindowEntryController progressEntry = await _uiFactory.CreateProgressWindowEntry(this);
                progressEntry.SetData(gameSession.PlayerName, gameSession.SessionScore);
            }
        }
    }
}
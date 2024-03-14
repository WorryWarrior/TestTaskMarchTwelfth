using System;
using System.Collections.Generic;
using Content.Data;
using Content.Infrastructure.Services.PersistentData;
using UnityEngine;
using UniRx;

namespace Content.UI
{
    public class LeaderboardWindowController : MonoBehaviour
    {
        [SerializeField] private Transform entryContainer = null;
        [SerializeField] private GameObject entryPrefab = null;

        private IPersistentDataService _persistentDataService;

        private readonly List<LeaderboardWindowEntryController> _entries = new();
        private readonly List<IDisposable> _subscriptions = new();

        public void Initialize(IPersistentDataService persistentDataService)
        {
            _persistentDataService = persistentDataService;

            SetupEntries();
            IDisposable leaderboardSubscription = _persistentDataService.Gameplay.LeaderboardData.Subscribe(UpdateEntries);
            _subscriptions.Add(leaderboardSubscription);
        }

        private void SetupEntries()
        {
            for (int i = 0; i < _persistentDataService.Gameplay.LeaderboardSize; i++)
            {
                LeaderboardWindowEntryController entry = Instantiate(entryPrefab, entryContainer)
                    .GetComponent<LeaderboardWindowEntryController>();

                _entries.Add(entry);
            }
        }

        private void UpdateEntries(List<ProgressEntryData> data)
        {
            if (data == null)
                return;

            for (int i = 0; i < data.Count; i++)
            {
                if (i >= _entries.Count)
                    return;

                _entries[i].SetEntryData(i, data[i].PlayerName, data[i].SessionScore);
            }
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
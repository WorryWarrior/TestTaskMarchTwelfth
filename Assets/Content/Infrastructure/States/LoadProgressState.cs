using System.Collections.Generic;
using System.Threading.Tasks;
using Content.Data;
using Content.Data.Enums;
using Content.Infrastructure.Services.PersistentData;
using Content.Infrastructure.Services.SaveLoad;
using Content.Infrastructure.States.Interfaces;
using UniRx;
using UnityEngine;

namespace Content.Infrastructure.States
{
    public class LoadProgressState : IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly IPersistentDataService _persistentDataService;
        private readonly ISaveLoadService _saveLoadService;

        public LoadProgressState(
            GameStateMachine stateMachine,
            IPersistentDataService persistentDataService,
            ISaveLoadService saveLoadService)
        {
            _stateMachine = stateMachine;
            _persistentDataService = persistentDataService;
            _saveLoadService = saveLoadService;
        }

        public async void Enter()
        {
            await LoadSettingsOrCreateNew();
            await LoadProgressOrCreateNew();
            LoadGameplayDataOrCreateNew();

            _stateMachine.Enter<LoadMetaState>();
        }

        public void Exit()
        {
        }

        private async Task LoadSettingsOrCreateNew()
        {
            _persistentDataService.Settings = await _saveLoadService.LoadSettings() ?? CreateNewSettings();
        }

        private SettingsData CreateNewSettings() => new()
        {
            IsMuted = new ReactiveProperty<bool>(false),
            MasterVolume = new ReactiveProperty<float>(.3f),
            PlayerColour = PlayerColour.Red,
            PlayerName = "agar"
        };

        private async Task LoadProgressOrCreateNew()
        {
            _persistentDataService.Progress = await _saveLoadService.LoadProgress() ?? CreateNewProgress();
        }

        private ProgressData CreateNewProgress() => new()
        {
            GameSessions = new List<ProgressEntryData>()
        };

        private void LoadGameplayDataOrCreateNew()
        {
            _persistentDataService.Gameplay = CreateNewGameplayData();
        }

        private GameplayData CreateNewGameplayData() => new()
        {
            LeaderboardData = new ReactiveProperty<List<ProgressEntryData>>(),
            LeaderboardSize = 8,
            LeaderboardNames = new []{ "Bot Ivan", "Bot Petar", "Bot Dmitry", "Bot Sergey", "Bot Nick",
                "Bot Oleg", "Bot Owen", "Bot Simon", "Bot Leonid", "Bot Max"},
            GameplayAreaSize = new Vector2(40f, 30f),
            StartPlayerScale = .75f,
            StartPlayerSpeed = 5f,
            RadiusIncreaseRate = .1f,
            SpeedDecreaseRate = .025f,
            FoodScale = .45f,
            StartEnemySpeed = 2.25f,
            EnemyCount = 10,
            EnemyAwarenessRadius = 10f,
            AiUpdateFrequency = .15f,
            FoodCount = 100,
            FoodRespawnFrequency = 7.5f,
            EnemyRespawnFrequency = 10f,
        };
    }
}
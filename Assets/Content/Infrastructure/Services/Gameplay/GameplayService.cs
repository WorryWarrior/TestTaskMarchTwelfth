using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Content.Data;
using Content.Gameplay;
using Content.Infrastructure.Factories.Interfaces;
using Content.Infrastructure.Services.PersistentData;
using UnityEngine;

namespace Content.Infrastructure.Services.Gameplay
{
    public class GameplayService : IGameplayService
    {
        private readonly IGameplayFactory _gameplayFactory;
        private readonly IPersistentDataService _persistentDataService;

        private GameplayObjectPool<FoodController> _foodPool;
        private GameplayObjectPool<EnemyController> _enemyPool;

        private List<ProgressEntryData> _tempLeaderboardState = new();

        public GameplayService(
            IGameplayFactory gameplayFactory,
            IPersistentDataService persistentDataService)
        {
            _gameplayFactory = gameplayFactory;
            _persistentDataService = persistentDataService;
        }

        public void Initialize()
        {
            _foodPool = new GameplayObjectPool<FoodController>(_persistentDataService.Gameplay.FoodCount);
            _enemyPool = new GameplayObjectPool<EnemyController>(_persistentDataService.Gameplay.EnemyCount);
        }

        public FoodController GetFoodObject(out int objectPoolId)
        {
            FoodController element = _foodPool.GetObject(out int id);
            objectPoolId = id;

            return element;
        }

        public EnemyController GetEnemyObject(out int objectPoolId)
        {
            EnemyController element = _enemyPool.GetObject(out int id);
            objectPoolId = id;

            return element;
        }

        public void ReleaseFoodObject(int objectPoolId) => _foodPool.ReleaseObject(objectPoolId);
        public void ReleaseEnemyObject(int objectPoolId) => _enemyPool.ReleaseObject(objectPoolId);

        public async Task CreateFoodPool()
        {
            Transform foodPoolParent = new GameObject("Food Pool Parent").transform;

            await _foodPool.CreatePool(async () => await _gameplayFactory.CreateFood(), it =>
            {
                it.transform.localScale = Vector3.one * _persistentDataService.Gameplay.FoodScale;
                it.transform.SetParent(foodPoolParent);
                it.gameObject.SetActive(false);
            });
        }

        public async Task CreateEnemyPool()
        {
            Transform enemyPoolParent = new GameObject("Enemy Pool Parent").transform;

            await _enemyPool.CreatePool(async () => await _gameplayFactory.CreateEnemy(), it =>
            {
                it.transform.localScale = Vector3.one * _persistentDataService.Gameplay.StartPlayerScale;
                it.transform.SetParent(enemyPoolParent);
                it.gameObject.SetActive(false);
            });
        }

        public void ClearFoodPool() => _foodPool.ClearPool();
        public void ClearEnemyPool() => _enemyPool.ClearPool();

        public void ClearLeaderboardEntries() => _tempLeaderboardState.Clear();

        public void SubmitLeaderboardEntry(int id, int score)
        {
            string leaderboardEntryName = id == -1
                ? _persistentDataService.Settings.PlayerName
                : _persistentDataService.Gameplay.LeaderboardNames[id];

            _tempLeaderboardState.Add(new ProgressEntryData
            {
                PlayerName = leaderboardEntryName,
                SessionScore = score
            });
        }

        public void UpdateLeaderboardState()
        {
            _tempLeaderboardState.Sort((a, b) => b.SessionScore.CompareTo(a.SessionScore));
            _persistentDataService.Gameplay.LeaderboardData.Value =
                _tempLeaderboardState.Take(_persistentDataService.Gameplay.LeaderboardSize).ToList();
        }
    }
}
using Content.Infrastructure.Services.Gameplay;
using Content.Infrastructure.Services.PersistentData;
using Entitas;
using UnityEngine;

namespace Content.Gameplay.Entitas.Systems
{
    public class RespawnEnemiesSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities = Contexts.sharedInstance.game.GetGroup(GameMatcher.AllOf(
            GameMatcher.Enemy));

        private readonly IPersistentDataService _persistentDataService;
        private readonly IGameplayService _gameplayService;

        private float _previousRespawnTime;

        public RespawnEnemiesSystem(
            IPersistentDataService persistentDataService,
            IGameplayService gameplayService)
        {
            _persistentDataService = persistentDataService;
            _gameplayService = gameplayService;
        }

        public void Execute()
        {
            if (_previousRespawnTime + _persistentDataService.Gameplay.FoodRespawnFrequency < Time.time)
            {
                if (_entities.count != _persistentDataService.Gameplay.EnemyCount)
                {
                    for (int i = 0; i < _persistentDataService.Gameplay.EnemyCount - _entities.count; i++)
                    {
                        EnemyController enemyController = _gameplayService.GetEnemyObject(out int enemyObjectIndex);

                        if (enemyController != null)
                        {
                            enemyController.Initialize();

                            Vector2 enemyPosition = new Vector2(
                                Random.Range(-_persistentDataService.Gameplay.GameplayAreaSize.x,
                                    _persistentDataService.Gameplay.GameplayAreaSize.x),
                                Random.Range(-_persistentDataService.Gameplay.GameplayAreaSize.y,
                                    _persistentDataService.Gameplay.GameplayAreaSize.y));
                            enemyController.transform.position = enemyPosition;

                            GameEntity e = Contexts.sharedInstance.game.CreateEntity();
                            e.AddScore(0);
                            e.AddPosition(enemyPosition);
                            e.AddEnemy(enemyObjectIndex);
                            e.AddRadius(enemyController.transform.localScale.x * 0.5f);
                            e.AddAiAgent(Vector2.zero, false);
                            e.AddLinkedGO(enemyController.gameObject);
                        }
                    }
                }

                _previousRespawnTime = Time.time;
            }
        }
    }
}
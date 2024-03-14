using System.Threading.Tasks;
using Content.Infrastructure.Factories.Interfaces;
using Content.Infrastructure.Services.Gameplay;
using Content.Infrastructure.Services.Logging;
using Content.Infrastructure.Services.PersistentData;
using Entitas;
using UnityEngine;

namespace Content.Gameplay.Entitas.Systems
{
    public class InitializePopulationSystem : IInitializeSystem
    {
        private readonly IGameplayFactory _gameplayFactory;
        private readonly IGameplayService _gameplayService;
        private readonly IPersistentDataService _persistentDataService;
        private readonly ILoggingService _loggingService;

        public InitializePopulationSystem(
            IGameplayFactory gameplayFactory,
            IGameplayService gameplayService,
            IPersistentDataService persistentDataService,
            ILoggingService loggingService
        )
        {
            _gameplayFactory = gameplayFactory;
            _gameplayService = gameplayService;
            _persistentDataService = persistentDataService;
            _loggingService = loggingService;
        }

        public async void Initialize()
        {
            await CreatePlayer();
            CreateFood();
            CreateEnemies();
        }

        private async Task CreatePlayer()
        {
            GameEntity e = Contexts.sharedInstance.game.CreateEntity();
            e.isInputListener = true;
            e.isPlayer = true;
            e.AddPosition(Vector2.zero);
            e.AddScore(1);

            PlayerController playerController = await _gameplayFactory.CreatePlayer();
            playerController.Initialize();
            Camera.main!.GetComponent<CameraController>().SetFollowTarget(playerController.transform);

            e.AddRadius(playerController.transform.localScale.x * 0.5f);
            e.AddLinkedGO(playerController.gameObject);
        }

        private void CreateEnemies()
        {
            for (int i = 0; i < _persistentDataService.Gameplay.EnemyCount; i++)
            {
                GameEntity e = Contexts.sharedInstance.game.CreateEntity();
                e.AddPosition(RandomPositionOnField());
                e.AddScore(0);

                EnemyController enemyController = _gameplayService.GetEnemyObject(out int enemyObjectIndex);
                enemyController.Initialize();
                enemyController.transform.position = e.position.value;
                e.AddEnemy(enemyObjectIndex);
                e.AddRadius(enemyController.transform.localScale.x * 0.5f);
                e.AddAiAgent(Vector2.zero, false);
                e.AddLinkedGO(enemyController.gameObject);
            }
        }

        private void CreateFood()
        {
            for (int i = 0; i < _persistentDataService.Gameplay.FoodCount; i++)
            {
                GameEntity e = Contexts.sharedInstance.game.CreateEntity();
                e.AddPosition(RandomPositionOnField());

                FoodController foodController = _gameplayService.GetFoodObject(out int foodObjectIndex);
                foodController.transform.position = e.position.value;
                e.AddFood(foodObjectIndex);
                e.AddRadius(foodController.transform.localScale.x * 0.5f);
                e.AddLinkedGO(foodController.gameObject);
            }
        }

        private Vector2 RandomPositionOnField() => new(
            Random.Range(-_persistentDataService.Gameplay.GameplayAreaSize.x,
                _persistentDataService.Gameplay.GameplayAreaSize.x),
            Random.Range(-_persistentDataService.Gameplay.GameplayAreaSize.y,
                _persistentDataService.Gameplay.GameplayAreaSize.y));
    }
}
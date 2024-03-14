using Content.Infrastructure.Services.Gameplay;
using Content.Infrastructure.Services.PersistentData;
using Entitas;
using UnityEngine;

namespace Content.Gameplay.Entitas.Systems
{
    public class RespawnFoodSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities = Contexts.sharedInstance.game.GetGroup(GameMatcher.AllOf(
            GameMatcher.Food));

        private readonly IPersistentDataService _persistentDataService;
        private readonly IGameplayService _gameplayService;

        private float _previousRespawnTime;

        public RespawnFoodSystem(
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
                if (_entities.count != _persistentDataService.Gameplay.FoodCount)
                {
                    for (int i = 0; i < _persistentDataService.Gameplay.FoodCount - _entities.count; i++)
                    {
                        FoodController foodController = _gameplayService.GetFoodObject(out int foodObjectIndex);

                        if (foodController != null)
                        {
                            Vector2 foodPosition = new Vector2(
                                Random.Range(-_persistentDataService.Gameplay.GameplayAreaSize.x,
                                    _persistentDataService.Gameplay.GameplayAreaSize.x),
                                Random.Range(-_persistentDataService.Gameplay.GameplayAreaSize.y,
                                    _persistentDataService.Gameplay.GameplayAreaSize.y));
                            foodController.transform.position = foodPosition;

                            GameEntity e = Contexts.sharedInstance.game.CreateEntity();

                            e.AddPosition(foodPosition);
                            e.AddFood(foodObjectIndex);
                            e.AddRadius(foodController.transform.localScale.x * 0.5f);
                            e.AddLinkedGO(foodController.gameObject);
                        }
                    }
                }

                _previousRespawnTime = Time.time;
            }
        }
    }
}
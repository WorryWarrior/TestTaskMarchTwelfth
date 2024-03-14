using Content.Infrastructure.Services.PersistentData;
using Entitas;
using UnityEngine;

namespace Content.Gameplay.Entitas.Systems
{
    public class ProcessAiMovementSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities = Contexts.sharedInstance.game.GetGroup(GameMatcher.AllOf(
            GameMatcher.AiAgent,
            GameMatcher.Position));

        private readonly IPersistentDataService _persistentDataService;

        public ProcessAiMovementSystem(
            IPersistentDataService persistentDataService
        )
        {
            _persistentDataService = persistentDataService;
        }

        public void Execute()
        {
            foreach (GameEntity entity in _entities)
            {
                Vector2 targetPosition = entity.aiAgent.value;
                Vector2 dir = targetPosition - entity.position.value;

                float dirFactor = entity.aiAgent.invertDir ? -1f : 1f;
                float dirMagnitude = dir.magnitude;

                float speedValue = _persistentDataService.Gameplay.StartEnemySpeed;

                float adjustedSpeed = Mathf.Clamp(speedValue * dirMagnitude, .5f,
                    4f - entity.score.value * _persistentDataService.Gameplay.SpeedDecreaseRate);

                dir.Normalize();
                Vector2 newPosition = entity.position.value + dir * dirFactor * adjustedSpeed * Time.deltaTime;
                Vector2 gameplayAreaSize = _persistentDataService.Gameplay.GameplayAreaSize;

                Vector2 boundPositionValue = new Vector2(
                    Mathf.Clamp(newPosition.x, -gameplayAreaSize.x, gameplayAreaSize.x),
                    Mathf.Clamp(newPosition.y, -gameplayAreaSize.y, gameplayAreaSize.y));

                entity.position.value = boundPositionValue;
            }
        }
    }
}
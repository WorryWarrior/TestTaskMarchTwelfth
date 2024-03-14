using Content.Infrastructure.Services.Input;
using Content.Infrastructure.Services.PersistentData;
using Entitas;
using UnityEngine;

namespace Content.Gameplay.Entitas.Systems
{
    public class ProcessInputSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _inputListeners = Contexts.sharedInstance.game.GetGroup(GameMatcher.AllOf(
            GameMatcher.InputListener,
            GameMatcher.Position));

        private readonly IPersistentDataService _persistentDataService;
        private readonly IInputService _inputService;

        public ProcessInputSystem(
            IPersistentDataService persistentDataService,
            IInputService inputService
        )
        {
            _persistentDataService = persistentDataService;
            _inputService = inputService;
        }

        public void Execute()
        {
            foreach (GameEntity entity in _inputListeners)
            {
                Vector2 inputPosition = _inputService.MouseWorldPosition;
                Vector2 dir = inputPosition - entity.position.value;
                float dirMagnitude = dir.magnitude;

                float speedValue = _persistentDataService.Gameplay.StartPlayerSpeed;

                float adjustedSpeed = Mathf.Clamp(speedValue * dirMagnitude, .5f,
                    6f - entity.score.value * _persistentDataService.Gameplay.SpeedDecreaseRate);

                dir.Normalize();
                Vector2 newPosition = entity.position.value + dir * adjustedSpeed * Time.deltaTime;
                Vector2 gameplayAreaSize = _persistentDataService.Gameplay.GameplayAreaSize;

                Vector2 boundPositionValue = new Vector2(
                    Mathf.Clamp(newPosition.x, -gameplayAreaSize.x, gameplayAreaSize.x),
                    Mathf.Clamp(newPosition.y, -gameplayAreaSize.y, gameplayAreaSize.y));

                entity.position.value = boundPositionValue;
            }
        }
    }
}
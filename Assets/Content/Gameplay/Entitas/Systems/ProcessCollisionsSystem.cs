using Content.Data;
using Content.Infrastructure.Services.Gameplay;
using Content.Infrastructure.States;
using Entitas;
using UnityEngine;

namespace Content.Gameplay.Entitas.Systems
{
    public class ProcessCollisionsSystem : IExecuteSystem, ICleanupSystem
    {
        private readonly IGroup<GameEntity> _collidables = Contexts.sharedInstance.game.GetGroup(GameMatcher.AllOf(
            GameMatcher.Position,
            GameMatcher.Radius).NoneOf(GameMatcher.Food));

        private readonly IGroup<GameEntity> _surroundings = Contexts.sharedInstance.game.GetGroup(GameMatcher.AllOf(
            GameMatcher.Position,
            GameMatcher.Radius));

        private readonly GameStateMachine _gameStateMachine;
        private readonly IGameplayService _gameplayService;

        public ProcessCollisionsSystem(
            GameStateMachine gameStateMachine,
            IGameplayService gameplayService)
        {
            _gameStateMachine = gameStateMachine;
            _gameplayService = gameplayService;
        }

        public void Execute()
        {
            foreach (GameEntity entity in _collidables)
            {
                foreach (GameEntity other in _surroundings)
                {
                    if (entity != other && !other.isMarkedForDestruction)
                    {
                        float distance = Vector2.Distance(entity.position.value, other.position.value);
                        if (distance <= entity.radius.value + other.radius.value)
                        {
                            if (other.hasFood)
                            {
                                other.isMarkedForDestruction = true;
                                entity.score.value++;
                                break;
                            }

                            if (other.hasEnemy)
                            {
                                if (other.score.value < entity.score.value)
                                {
                                    other.isMarkedForDestruction = true;
                                    entity.score.value += other.score.value + 1;
                                }
                                else
                                {
                                    if (entity.isPlayer)
                                    {
                                        _gameStateMachine.Enter<GameOverState, GameOverStateData>(new GameOverStateData
                                            { GameSessionScore = entity.score.value });
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public void Cleanup()
        {
            foreach (GameEntity entity in _surroundings.GetEntities())
            {
                if (entity.isMarkedForDestruction)
                {
                    if (entity.hasFood)
                        _gameplayService.ReleaseFoodObject(entity.food.linkedObjectIndex);
                    if (entity.hasEnemy)
                        _gameplayService.ReleaseEnemyObject(entity.enemy.linkedObjectIndex);

                    entity.Destroy();
                }
            }
        }
    }
}
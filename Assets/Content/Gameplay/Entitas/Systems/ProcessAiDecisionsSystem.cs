using Content.Infrastructure.Services.PersistentData;
using Entitas;
using UnityEngine;

namespace Content.Gameplay.Entitas.Systems
{
    public class ProcessAiDecisionsSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _aiAgents = Contexts.sharedInstance.game.GetGroup(GameMatcher.AllOf(
            GameMatcher.AiAgent));

        private readonly IGroup<GameEntity> _surroundings = Contexts.sharedInstance.game.GetGroup(GameMatcher.AllOf(
            GameMatcher.Position,
            GameMatcher.Radius));

        private readonly IPersistentDataService _persistentDataService;

        private float _previousDecisionTime;

        public ProcessAiDecisionsSystem(
            IPersistentDataService persistentDataService
        )
        {
            _persistentDataService = persistentDataService;
        }

        public void Execute()
        {
            if (_previousDecisionTime + _persistentDataService.Gameplay.AiUpdateFrequency < Time.time)
            {
                foreach (GameEntity entity in _aiAgents)
                {
                    Vector2 escapeTarget = Vector2.zero;
                    Vector2 huntTarget = Vector2.zero;
                    Vector2 feedTarget = Vector2.zero;

                    foreach (GameEntity other in _surroundings)
                    {
                        if (other.hasScore && Vector2.Distance(other.position.value, entity.position.value) <
                            _persistentDataService.Gameplay.EnemyAwarenessRadius)
                        {
                            if (other.score.value > entity.score.value)
                            {
                                escapeTarget = other.position.value;
                                break;
                            }

                            if (other.score.value < entity.score.value)
                            {
                                huntTarget = other.position.value;
                                break;
                            }
                        }

                        if (other.hasFood && Vector2.Distance(other.position.value, entity.position.value) <
                            _persistentDataService.Gameplay.EnemyAwarenessRadius)
                        {
                            feedTarget = other.position.value;
                            break;
                        }
                    }

                    if (escapeTarget != Vector2.zero)
                    {
                        entity.aiAgent.value = escapeTarget;
                        entity.aiAgent.invertDir = true;
                    }
                    else if (huntTarget != Vector2.zero)
                    {
                        entity.aiAgent.value = huntTarget;
                        entity.aiAgent.invertDir = false;
                    }
                    else if (feedTarget != Vector2.zero)
                    {
                        entity.aiAgent.value = feedTarget;
                        entity.aiAgent.invertDir = false;
                    }
                    else
                    {
                        entity.aiAgent.value = entity.position.value +
                                               new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                        entity.aiAgent.invertDir = false;
                    }
                }

                _previousDecisionTime = Time.time;
            }
        }
    }
}
using System.Collections.Generic;
using Content.Infrastructure.Services.Gameplay;
using Content.Infrastructure.Services.PersistentData;
using Entitas;
using UnityEngine;

namespace Content.Gameplay.Entitas.Systems
{
    public class ProcessSizeChangeSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities = Contexts.sharedInstance.game.GetGroup(GameMatcher.AllOf(
            GameMatcher.Score,
            GameMatcher.Radius,
            GameMatcher.LinkedGO));

        private readonly IGameplayService _gameplayService;
        private readonly float _initialRadius;
        private readonly float _increaseRate;

        private CameraController _cameraController;
        private CameraController CameraController
        {
            get
            {
                if (_cameraController == null)
                    _cameraController = Camera.main!.GetComponent<CameraController>();

                return _cameraController;
            }
        }

        public ProcessSizeChangeSystem(
            IGameplayService gameplayService,
            IPersistentDataService persistentDataService)
        {
            _gameplayService = gameplayService;
            _initialRadius = persistentDataService.Gameplay.StartPlayerScale * 0.5f;
            _increaseRate = persistentDataService.Gameplay.RadiusIncreaseRate;
        }

        public void Execute()
        {
            _gameplayService.ClearLeaderboardEntries();

            foreach (GameEntity entity in _entities)
            {
                entity.radius.value = _initialRadius + entity.score.value * _increaseRate;

                if (entity.linkedGO.value)
                {
                    entity.linkedGO.value.transform.localScale = Vector3.one * entity.radius.value * 2;
                }

                if (entity.isPlayer)
                {
                    _gameplayService.SubmitLeaderboardEntry(-1, entity.score.value);
                    CameraController.SetViewportScale(entity.score.value);
                }

                if (entity.hasEnemy)
                {
                    _gameplayService.SubmitLeaderboardEntry(entity.enemy.linkedObjectIndex, entity.score.value);
                }
            }

            _gameplayService.UpdateLeaderboardState();
        }
    }
}
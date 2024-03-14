using System.Threading.Tasks;
using Content.Gameplay;
using UnityEngine;

namespace Content.Infrastructure.Factories.Interfaces
{
    public interface IGameplayFactory
    {
        GameplaySimulationController GameplaySimulationController { get; }

        Task WarmUp();
        void CleanUp();
        Task<GameplaySimulationController> CreateGameplaySimulationController();
        Task<CameraController> CreateCamera();
        Task<PlayerController> CreatePlayer();
        Task<FoodController> CreateFood();
        Task<EnemyController> CreateEnemy();
    }
}
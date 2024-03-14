using System.Threading.Tasks;
using Content.Gameplay;
using Content.Infrastructure.AssetManagement;
using Content.Infrastructure.Factories.Interfaces;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Content.Infrastructure.Factories
{
    public class GameplayFactory : IGameplayFactory
    {
        private const string GameplayControllerPrefabId = "PFB_GameplaySimulationController";
        private const string PlayerPrefabId             = "PFB_Player";
        private const string CameraPrefabId             = "PFB_Camera";
        private const string FoodPrefabId               = "PFB_Food";
        private const string EnemyPrefabId              = "PFB_Enemy";

        private readonly IObjectResolver _container;
        private readonly IAssetProvider _assetProvider;

        public GameplayFactory(
            LifetimeScope lifetimeScope,
            IAssetProvider assetProvider)
        {
            _container = lifetimeScope.Container;
            _assetProvider = assetProvider;
        }

        public GameplaySimulationController GameplaySimulationController { get; private set; }

        public async Task WarmUp()
        {
            await _assetProvider.Load<GameObject>(GameplayControllerPrefabId);
            await _assetProvider.Load<GameObject>(PlayerPrefabId);
            await _assetProvider.Load<GameObject>(CameraPrefabId);
            await _assetProvider.Load<GameObject>(FoodPrefabId);
            await _assetProvider.Load<GameObject>(EnemyPrefabId);
        }

        public void CleanUp()
        {
            _assetProvider.Release(GameplayControllerPrefabId);
            _assetProvider.Release(PlayerPrefabId);
            _assetProvider.Release(CameraPrefabId);
            _assetProvider.Release(FoodPrefabId);
            _assetProvider.Release(EnemyPrefabId);
        }

        public async Task<GameplaySimulationController> CreateGameplaySimulationController()
        {
            GameObject prefab = await _assetProvider.Load<GameObject>(GameplayControllerPrefabId);
            GameplaySimulationController controller =
                Object.Instantiate(prefab).GetComponent<GameplaySimulationController>();
            GameplaySimulationController = controller;

            _container.Inject(controller);
            return controller;
        }

        public async Task<CameraController> CreateCamera()
        {
            GameObject prefab = await _assetProvider.Load<GameObject>(CameraPrefabId);
            CameraController controller =
                Object.Instantiate(prefab).GetComponent<CameraController>();

            _container.Inject(controller);
            return controller;
        }

        public async Task<PlayerController> CreatePlayer()
        {
            GameObject prefab = await _assetProvider.Load<GameObject>(PlayerPrefabId);
            PlayerController player = Object.Instantiate(prefab).GetComponent<PlayerController>();

            _container.Inject(player);
            return player;
        }

        public async Task<FoodController> CreateFood()
        {
            GameObject prefab = await _assetProvider.Load<GameObject>(FoodPrefabId);
            FoodController food = Object.Instantiate(prefab).GetComponent<FoodController>();

            _container.Inject(food);
            return food;
        }

        public async Task<EnemyController> CreateEnemy()
        {
            GameObject prefab = await _assetProvider.Load<GameObject>(EnemyPrefabId);
            EnemyController enemy = Object.Instantiate(prefab).GetComponent<EnemyController>();

            _container.Inject(enemy);
            return enemy;
        }
    }
}
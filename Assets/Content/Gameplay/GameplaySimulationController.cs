using Content.Gameplay.Entitas.Systems;
using Content.Infrastructure.Factories;
using UnityEngine;
using VContainer;

namespace Content.Gameplay
{
    public class GameplaySimulationController : MonoBehaviour
    {
        private EntitasSystemFactory _systemFactory;
        private Feature _systems;

        private bool _isSimulating = true;

        [Inject]
        private void Construct(
            EntitasSystemFactory systemFactory)
        {
            _systemFactory = systemFactory;
        }

        private void Update()
        {
            if (!_isSimulating)
                return;

            _systems.Execute();
            _systems.Cleanup();
        }

        public void InitializeGameplay()
        {
            _systems = ConstructEntitasSystems();
            _systems.Initialize();
        }

        public void ToggleSimulation(bool value) => _isSimulating = value;

        private Feature ConstructEntitasSystems()
        {
            Feature simulation = new Feature("Gameplay Simulation");
            simulation.Add(_systemFactory.CreateSystem<InitializePopulationSystem>());
            simulation.Add(_systemFactory.CreateSystem<ProcessInputSystem>());
            simulation.Add(_systemFactory.CreateSystem<ProcessAiDecisionsSystem>());
            simulation.Add(_systemFactory.CreateSystem<ProcessAiMovementSystem>());
            simulation.Add(_systemFactory.CreateSystem<ProcessCollisionsSystem>());
            simulation.Add(_systemFactory.CreateSystem<ProcessSizeChangeSystem>());
            simulation.Add(_systemFactory.CreateSystem<RespawnFoodSystem>());
            simulation.Add(_systemFactory.CreateSystem<RespawnEnemiesSystem>());
            simulation.Add(_systemFactory.CreateSystem<UpdateLinkedGOSystem>());

            return simulation;
        }
    }
}
using Content.Infrastructure.Factories.Interfaces;
using Content.Infrastructure.States.Interfaces;

namespace Content.Infrastructure.States
{
    public class GameLoopState : IState
    {
        private readonly IGameplayFactory _gameplayFactory;

        public GameLoopState(
            IGameplayFactory gameplayFactory)
        {
            _gameplayFactory = gameplayFactory;
        }

        public void Enter()
        {
        }

        public void Exit()
        {
            _gameplayFactory.GameplaySimulationController.ToggleSimulation(false);
        }
    }
}
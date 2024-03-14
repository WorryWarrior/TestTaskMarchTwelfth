using Content.Infrastructure.States.Interfaces;
using VContainer;
using VContainer.Unity;

namespace Content.Infrastructure.Factories
{
    public class StateFactory
    {
        private readonly IObjectResolver _container;

        public StateFactory(LifetimeScope lifetimeScope) =>
            _container = lifetimeScope.Container;

        public T CreateState<T>() where T : IExitableState =>
            _container.Resolve<T>();
    }
}
using Entitas;
using VContainer;
using VContainer.Unity;

namespace Content.Infrastructure.Factories
{
    public class EntitasSystemFactory
    {
        private readonly IObjectResolver _container;

        public EntitasSystemFactory(LifetimeScope lifetimeScope) =>
            _container = lifetimeScope.Container;

        public T CreateSystem<T>() where T : ISystem =>
            _container.Resolve<T>();
    }
}
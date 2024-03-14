using Content.Gameplay.Entitas.Systems;
using Content.Infrastructure.AssetManagement;
using Content.Infrastructure.Factories;
using Content.Infrastructure.Factories.Interfaces;
using Content.Infrastructure.SceneManagement;
using Content.Infrastructure.Services.Gameplay;
using Content.Infrastructure.Services.Input;
using Content.Infrastructure.Services.Logging;
using Content.Infrastructure.Services.PersistentData;
using Content.Infrastructure.Services.SaveLoad;
using Content.Infrastructure.States;
using VContainer;
using VContainer.Unity;

namespace Content.Infrastructure.Scopes
{
    public class InfrastructureScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            ConfigureProviders(builder);
            ConfigureServices(builder);
            ConfigureFactories(builder);

            ConfigureStates(builder);
            ConfigureSystems(builder);
        }

        private void ConfigureProviders(IContainerBuilder builder)
        {
            builder.Register<AssetProvider>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<ISceneLoader, SceneLoader>(Lifetime.Singleton);
        }

        private void ConfigureServices(IContainerBuilder builder)
        {
            builder.Register<IInputService, LegacyInputService>(Lifetime.Singleton);
            builder.Register<ILoggingService, LoggingService>(Lifetime.Singleton);
            builder.Register<IPersistentDataService, PersistentDataService>(Lifetime.Singleton);
            builder.Register<ISaveLoadService, SaveLoadServiceJsonFile>(Lifetime.Singleton);
            //builder.Register<ISaveLoadService, SaveLoadServicePlayerPrefs>(Lifetime.Singleton);
            builder.Register<IGameplayService, GameplayService>(Lifetime.Singleton);
        }

        private void ConfigureFactories(IContainerBuilder builder)
        {
            builder.Register<StateFactory>(Lifetime.Singleton);
            builder.Register<EntitasSystemFactory>(Lifetime.Singleton);
            builder.Register<IUIFactory, UIFactory>(Lifetime.Singleton);
            builder.Register<IAudioFactory, AudioFactory>(Lifetime.Singleton);
            builder.Register<IGameplayFactory, GameplayFactory>(Lifetime.Singleton);
        }

        private void ConfigureStates(IContainerBuilder builder)
        {
            builder.Register<BootstrapState>(Lifetime.Singleton);
            builder.Register<LoadProgressState>(Lifetime.Singleton);
            builder.Register<LoadMetaState>(Lifetime.Singleton);
            builder.Register<LoadLevelState>(Lifetime.Singleton);
            builder.Register<GameLoopState>(Lifetime.Singleton);
            builder.Register<GameOverState>(Lifetime.Singleton);

            builder.Register<GameStateMachine>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
        }

        private void ConfigureSystems(IContainerBuilder builder)
        {
            builder.Register<InitializePopulationSystem>(Lifetime.Singleton);
            builder.Register<ProcessInputSystem>(Lifetime.Singleton);
            builder.Register<ProcessAiDecisionsSystem>(Lifetime.Singleton);
            builder.Register<ProcessAiMovementSystem>(Lifetime.Singleton);
            builder.Register<ProcessCollisionsSystem>(Lifetime.Singleton);
            builder.Register<ProcessSizeChangeSystem>(Lifetime.Singleton);
            builder.Register<RespawnFoodSystem>(Lifetime.Singleton);
            builder.Register<RespawnEnemiesSystem>(Lifetime.Singleton);
            builder.Register<UpdateLinkedGOSystem>(Lifetime.Singleton);
        }
    }
}
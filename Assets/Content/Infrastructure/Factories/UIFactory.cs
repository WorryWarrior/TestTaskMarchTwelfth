using Content.Infrastructure.AssetManagement;
using Content.Infrastructure.Factories.Interfaces;
using System.Threading.Tasks;
using Content.UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Content.Infrastructure.Factories
{
    public class UIFactory : IUIFactory
    {
        private const string UIRootPrefabId         = "PFB_UIRoot";
        private const string MainMenuHudId          = "PFB_MainMenuHUD";
        private const string GameplayHudId          = "PFB_GameplayHUD";
        private const string ProgressWindowEntryId  = "PFB_ProgressWindowEntry";

        private readonly IObjectResolver _container;
        private readonly IAssetProvider _assetProvider;

        private Canvas _uiRoot;
        public GameplayHUDController GameplayHUD { get; private set; }

        public UIFactory(
            LifetimeScope lifetimeScope,
            IAssetProvider assetProvider)
        {
            _container = lifetimeScope.Container;
            _assetProvider = assetProvider;
        }

        public async Task WarmUp()
        {
            await _assetProvider.Load<GameObject>(UIRootPrefabId);
            await _assetProvider.Load<GameObject>(MainMenuHudId);
            await _assetProvider.Load<GameObject>(GameplayHudId);
            await _assetProvider.Load<GameObject>(ProgressWindowEntryId);
        }

        public void CleanUp()
        {
            _assetProvider.Release(MainMenuHudId);
            _assetProvider.Release(GameplayHudId);
            _assetProvider.Release(ProgressWindowEntryId);
        }

        public async Task CreateUIRoot()
        {
            GameObject prefab = await _assetProvider.Load<GameObject>(UIRootPrefabId);
            _uiRoot = Object.Instantiate(prefab).GetComponent<Canvas>();
        }

        public async Task<MainMenuHUDController> CreateMainMenuHUD()
        {
            GameObject prefab = await _assetProvider.Load<GameObject>(MainMenuHudId);
            MainMenuHUDController hud = Object.Instantiate(prefab, _uiRoot.transform).GetComponent<MainMenuHUDController>();

            _container.InjectGameObject(hud.gameObject);
            return hud;
        }

        public async Task<GameplayHUDController> CreateGameplayHUD()
        {
            GameObject prefab = await _assetProvider.Load<GameObject>(GameplayHudId);
            GameplayHUDController hud = Object.Instantiate(prefab, _uiRoot.transform).GetComponent<GameplayHUDController>();
            GameplayHUD = hud;

            _container.InjectGameObject(hud.gameObject);
            return hud;
        }

        public async Task<ProgressWindowEntryController> CreateProgressWindowEntry(ProgressWindowController hud)
        {
            GameObject prefab = await _assetProvider.Load<GameObject>(ProgressWindowEntryId);
            ProgressWindowEntryController entry = Object.Instantiate(prefab, hud.progressWindowEntryContainer).GetComponent<ProgressWindowEntryController>();

            //_container.Inject(entry);
            return entry;
        }
    }
}
using Content.Infrastructure.Services.PersistentData;
using UnityEngine;
using VContainer;

namespace Content.Gameplay
{
    public abstract class PlayableEntityController : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer = null;

        protected IPersistentDataService _persistentDataService;

        [Inject]
        private void Construct(
            IPersistentDataService persistentDataService
        )
        {
            _persistentDataService = persistentDataService;
        }

        public void Initialize()
        {
            spriteRenderer.color = GenerateEntityColour();
            transform.localScale = Vector3.one * _persistentDataService.Gameplay.StartPlayerScale;
        }

        protected abstract Color GenerateEntityColour();
    }
}
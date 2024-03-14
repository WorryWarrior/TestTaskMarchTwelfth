using UnityEngine;

namespace Content.Infrastructure.Services.Input
{
    public class LegacyInputService : IInputService
    {
        public Vector2 MousePosition => UnityEngine.Input.mousePosition;

        public Vector2 MouseWorldPosition =>
            UnityEngine.Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
    }
}
using UnityEngine;

namespace Content.Infrastructure.Services.Input
{
    public interface IInputService
    {
        Vector2 MousePosition { get; }

        Vector2 MouseWorldPosition { get; }
    }
}
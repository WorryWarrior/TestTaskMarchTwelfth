using Entitas;
using UnityEngine;

namespace Content.Gameplay.Entitas.Components
{
    // dotnet Jenny/Jenny.Generator.Cli.dll gen
    [Game] public class InputListenerComponent : IComponent { }

    [Game] public class PositionComponent : IComponent { public Vector2 value; }
    [Game] public class RadiusComponent : IComponent { public float value; }
    [Game] public class ScoreComponent : IComponent { public int value; }
    [Game] public class LinkedGOComponent : IComponent { public GameObject value; }

    [Game] public class FoodComponent : IComponent { public int linkedObjectIndex; }
    [Game] public class EnemyComponent : IComponent { public int linkedObjectIndex; }
    [Game] public class AiAgentComponent : IComponent { public Vector2 value; public bool invertDir; }
    [Game] public class PlayerComponent : IComponent {  }
    [Game] public class MarkedForDestructionComponent : IComponent {  }
}
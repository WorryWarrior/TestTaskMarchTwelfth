using UnityEngine;

namespace Content.Gameplay
{
    public class EnemyController : PlayableEntityController
    {
        protected override Color GenerateEntityColour() =>
            new(Random.Range(.25f, 1f), Random.Range(.25f, 1f), Random.Range(.25f, 1f));
    }
}
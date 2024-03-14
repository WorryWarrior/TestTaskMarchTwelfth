using Entitas;

namespace Content.Gameplay.Entitas.Systems
{
    public class UpdateLinkedGOSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities = Contexts.sharedInstance.game.GetGroup(GameMatcher.AllOf(
            GameMatcher.Position,
            GameMatcher.LinkedGO));

        public void Execute()
        {
            foreach (GameEntity entity in _entities)
            {
                if (entity.linkedGO.value)
                {
                    entity.linkedGO.value.transform.position = entity.position.value;
                }
            }
        }
    }
}
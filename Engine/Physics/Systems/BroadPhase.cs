using Engine.Physics.Components;
using Engine.Physics.Components.RigidBody;
using Leopotam.Ecs;

namespace Engine.Physics.Systems
{
    public class BroadPhase : IEcsRunSystem
    {
        private EcsFilter<RigidBody, BoundingBox> colliders = null;
        private PhysicsSystemState state = null;

        public void Run()
        {
            var pairs = state.CollisionPairs;

            for (var i = 0; i < colliders.GetEntitiesCount(); i++) {
                for (var k = i + 1; k < colliders.GetEntitiesCount(); k++)
                {
                    if (BoundingBox.Intersects(ref colliders.Get2(i), ref colliders.Get2(k)))
                    {
                        pairs.Add(new CollisionPair {BodyA = colliders.GetEntity(i), BodyB = colliders.GetEntity(k)});
                    }
                }
            }
        }

    }
}

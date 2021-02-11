using Engine.Physics.Components;
using Leopotam.Ecs;

namespace Engine.Physics.Systems
{
    public class BroadPhase : IEcsRunSystem
    {
        private EcsFilter<RigidBody, BoundingBox> bodies = null;

        private PhysicsSystemState state = null;

        public void Run()
        {
            var pairs = state.CollisionPairs;
            var bodiesCount = bodies.GetEntitiesCount();

            for (var i = 0; i < bodiesCount; ++i) {
                for (var k = i + 1; k < bodiesCount; ++k)
                {
                    if (BoundingBox.Intersects(ref bodies.Get2(i), ref bodies.Get2(k)))
                    {
                        pairs.Add(new CollisionPair {BodyA = bodies.GetEntity(i), BodyB = bodies.GetEntity(k)});
                    }
                }
            }
        }

    }
}

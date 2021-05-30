using Engine.Physics.Components;
using Leopotam.EcsLite;

namespace Engine.Physics.Systems
{
    public class BroadPhase : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter bodies;

        public void Init(EcsSystems systems)
        {
            var world = systems.GetWorld ();

            bodies = world.Filter<RigidBody>().Inc<BoundingBox>().End();
        }

        public void Run(EcsSystems systems)
        {
            var sharedData = systems.GetShared<SharedData>();
            var pairs = sharedData.PhysicsSystemData.CollisionPairs;
            var bboxes = sharedData.bboxes;
            var bodiesCount = bodies.GetEntitiesCount();

            // TODO don't do that, use foreach or something smart
            for (var i = 0; i < bodiesCount; ++i) {
                for (var k = i + 1; k < bodiesCount; ++k)
                {
                    if (BoundingBox.Intersects(in bboxes.Get(i), in bboxes.Get(k)))
                    {
                        pairs.Add(new CollisionPair {BodyA = i, BodyB = k});
                    }
                }
            }
        }

    }
}

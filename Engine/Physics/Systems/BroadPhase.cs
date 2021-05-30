using Leopotam.EcsLite;
using Engine.Physics.Components;

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

            // don't do that, use foreach or something smarter
            var entities = bodies.GetRawEntities();
            var entitiesCount = bodies.GetEntitiesCount();
            for (var idx = 0; idx < entitiesCount; ++idx) {
                for (var idy = idx + 1; idy < entitiesCount; ++idy)
                {
                    var A = entities[idx];
                    var B = entities[idy];

                    if (BoundingBox.Intersects(in bboxes.Get(A), in bboxes.Get(B)))
                    {
                        pairs.Add(new CollisionPair {BodyA = A, BodyB = B});
                    }
                }
            }
        }

    }
}

using Leopotam.EcsLite;
using Engine.Physics.Components;

namespace Engine.Physics.Systems
{
    public class RemoveLeakedObjects : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter bodies;

        public void Init(EcsSystems systems)
        {
            var world = systems.GetWorld();

            bodies = world.Filter<RigidBody>().Inc<BoundingBox>().End();
        }

        public void Run(EcsSystems systems)
        {
            var world = systems.GetWorld();
            var sharedData = systems.GetShared<SharedData>();
            var boundaries = sharedData.PhysicsSystemData.Boundaries;

            var bboxes = sharedData.bboxes;

            foreach (var idx in bodies) {
                if (!BoundingBox.Intersects(in bboxes.Get(idx), in boundaries))
                {
                    // world is not perfect...
                    world.DelEntity(idx);
                }
            }
        }

    }
}

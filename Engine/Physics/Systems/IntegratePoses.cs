using Engine.Physics.Components;
using Leopotam.EcsLite;

namespace Engine.Physics.Systems
{
    public class IntegratePoses : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter bodies;

        public void Init(EcsSystems systems)
        {
            var world = systems.GetWorld();

            bodies = world.Filter<RigidBody>().Inc<Pose>().Inc<Velocity>().End();
        }

        public void Run(EcsSystems systems)
        {
            var sharedData = systems.GetShared<SharedData>();
            var dt = sharedData.PhysicsSystemData.dt;

            var poses = sharedData.poses;
            var velocities = sharedData.velocities;

            foreach (var idx in bodies)
            {
                ref var pose = ref poses.Get(idx);
                ref var velocity = ref velocities.Get(idx).Linear;
                pose.Position += velocity * dt;
            }
        }

    }
}

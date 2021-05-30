using Engine.Game.Components;
using Leopotam.EcsLite;
using Engine.Physics.Components;

namespace Engine.Game.Systems
{
    public class ApplyGravity : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter bodies;

        public void Init(EcsSystems systems)
        {
            var world = systems.GetWorld ();

            bodies = world.Filter<RigidBody>().Inc<Velocity>().Exc<Attractor>().End();
        }

        public void Run(EcsSystems systems)
        {
            var sharedData = systems.GetShared<SharedData>();
            var physicsData = sharedData.PhysicsSystemData;
            var gravityDt = physicsData.Gravity * physicsData.dt;
            var rigidBodies = sharedData.rigidBodies;
            var velocities = sharedData.velocities;

            foreach (var entity in bodies)
            {
                if (rigidBodies.Get(entity).Locked) continue;
                ref var velocity = ref velocities.Get(entity).Linear;
                velocity += gravityDt;
            }
        }

    }
}

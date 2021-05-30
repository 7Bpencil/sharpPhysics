using Engine.Example.Components;
using Engine.Physics;
using Engine.Physics.Components;
using Leopotam.EcsLite;

namespace Engine.Example.Systems
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

            ApplyForce(gravityDt, sharedData.rigidBodies, sharedData.velocities);
        }

        private void ApplyForce(Vector2 velocityDelta, EcsPool<RigidBody> rigidBodies, EcsPool<Velocity> velocities)
        {
            foreach (var idx in bodies)
            {
                ref var body = ref rigidBodies.Get(idx);
                if (body.Locked) continue;

                ref var velocity = ref velocities.Get(idx).Linear;
                velocity += velocityDelta;
            }
        }

    }
}

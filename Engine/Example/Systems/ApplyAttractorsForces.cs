using Engine.Example.Components;
using Engine.Physics;
using Engine.Physics.Components;
using Leopotam.EcsLite;

namespace Engine.Example.Systems
{
    public class ApplyAttractorsForces : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter attractors;
        private EcsFilter bodies;

        public void Init(EcsSystems systems)
        {
            var world = systems.GetWorld();

            attractors = world.Filter<Attractor>().Inc<RigidBody>().Inc<Pose>().End();
            bodies = world.Filter<RigidBody>().Inc<Pose>().Inc<Velocity>().Exc<Attractor>().End();
        }

        public void Run(EcsSystems systems)
        {
            var sharedData = systems.GetShared<SharedData>();
            ApplyForces(sharedData.PhysicsSystemData.dt, sharedData.rigidBodies, sharedData.poses, sharedData.velocities);
        }

        private void ApplyForces(float dt, EcsPool<RigidBody> rigidBodies, EcsPool<Pose> poses, EcsPool<Velocity> velocities)
        {
            foreach (var idx in bodies)
            {
                ref var body = ref rigidBodies.Get(idx);
                if (body.Locked) continue;

                ref var velocity = ref velocities.Get(idx).Linear;
                var bodyCenter = poses.Get(idx).Position;

                velocity += CalculateAttractorsInfluence(bodyCenter) * dt;
            }


            Vector2 CalculateAttractorsInfluence(Vector2 bodyCenter)
            {
                var forces = Vector2.Zero;
                foreach (var idx in attractors)
                {
                    var diff = poses.Get(idx).Position - bodyCenter;
                    var falloffMultiplier = rigidBodies.Get(idx).Mass / diff.LengthSquared;
                    forces += diff * falloffMultiplier;
                }

                return forces;
            }
        }

    }
}

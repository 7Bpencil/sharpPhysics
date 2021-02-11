using Engine.Example.Components;
using Engine.Physics;
using Engine.Physics.Components;
using Leopotam.Ecs;

namespace Engine.Example.Systems
{
    public class ApplyAttractorsForces : IEcsRunSystem
    {
        private EcsFilter<Attractor, RigidBody, Pose> attractors = null;
        private EcsFilter<RigidBody, Pose, Velocity>.Exclude<Attractor> bodies = null;

        private PhysicsSettings settings = null;

        public void Run()
        {
            var dt = settings.dt;

            foreach (var idx in bodies)
            {
                ref var body = ref bodies.Get1(idx);
                if (body.Locked) continue;

                ref var velocity = ref bodies.Get3(idx).Linear;
                var bodyCenter = bodies.Get2(idx).Position;

                velocity += CalculateAttractorsInfluence(bodyCenter) * dt;
            }
        }

        private Vector2 CalculateAttractorsInfluence(Vector2 bodyCenter)
        {
            var forces = Vector2.Zero;
            foreach (var idx in attractors)
            {
                var diff = attractors.Get3(idx).Position - bodyCenter;
                var falloffMultiplier = attractors.Get2(idx).Mass / diff.LengthSquared;
                forces += diff * falloffMultiplier;
            }

            return forces;
        }
    }
}

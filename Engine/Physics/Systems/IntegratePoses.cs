using Engine.Physics.Components;
using Leopotam.Ecs;

namespace Engine.Physics.Systems
{
    public class IntegratePoses : IEcsRunSystem
    {
        private EcsFilter<RigidBody, Pose, Velocity> bodies = null;

        private PhysicsSettings settings = null;

        public void Run()
        {
            var dt = settings.dt;

            foreach (var idx in bodies)
            {
                ref var pose = ref bodies.Get2(idx);
                ref var velocity = ref bodies.Get3(idx).Linear;
                pose.Position += velocity * dt;
            }
        }

    }
}

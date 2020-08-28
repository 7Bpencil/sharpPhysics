using Engine.Physics.Components;
using Engine.Physics.Components.RigidBody;
using Leopotam.Ecs;

namespace Engine.Physics.Systems
{
    public class ApplyGlobalGravity : IEcsRunSystem
    {
        private EcsFilter<RigidBody, Transform, Velocity>.Exclude<Attractor> rigidBodies = null;
        private PhysicsSettings settings = null;

        public void Run()
        {
            var gravity = settings.Gravity * settings.VelocityCoefficient;

            foreach (var idx in rigidBodies)
            {
                ref var body = ref rigidBodies.Get1(idx);
                if (body.Locked) continue;
                
                ref var velocity = ref rigidBodies.Get3(idx);
                velocity.Value += gravity;
            }
        }
    }
}
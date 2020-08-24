using Engine.Physics.Components;
using Engine.Physics.Components.RigidBody;
using Engine.Physics.Helpers;
using Leopotam.Ecs;

namespace Engine.Physics.Systems
{
    public class ApplyVelocity : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter<RigidBody, Transform, Velocity> rigidBodies = null;
        private PhysicsSettings settings = null;
        
        public void Init() { }

        public void Run()
        {
            var dt = settings.dt;
            var tolerance = settings.AccuracyTolerance;
            foreach (var idx in rigidBodies)
            {
                ref var transform = ref rigidBodies.Get2(idx);
                ref var velocity = ref rigidBodies.Get3(idx).Value;
                
                PhysMath.RoundToZero(ref velocity, tolerance);
                transform.Position += velocity * dt;
            }
        }
    }
}
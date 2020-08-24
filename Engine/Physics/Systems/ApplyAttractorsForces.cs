using Engine.Physics.Components;
using Engine.Physics.Components.RigidBody;
using Leopotam.Ecs;

namespace Engine.Physics.Systems
{
    public class ApplyAttractorsForces : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter<Attractor, RigidBody, Transform> gravityBodies = null;
        private EcsFilter<RigidBody, Transform, Velocity>.Exclude<Attractor> rigidBodies = null;
        private PhysicsSettings settings = null;
            
        public void Init() { }

        public void Run()
        {
            foreach (var idx in rigidBodies)
            {
                ref var body = ref rigidBodies.Get1(idx);
                if (body.Locked) continue;
                
                ref var velocity = ref rigidBodies.Get3(idx);
                var bodyCenter = rigidBodies.Get2(idx).Position;

                velocity.Value += CalculateBodiesInfluence(bodyCenter);
            }
        }

        private Vector2 CalculateBodiesInfluence(Vector2 bodyCenter)
        {
            var forces = Vector2.Zero;
            foreach (var idy in gravityBodies)
            {
                //apply inverse square law
                var diff = gravityBodies.Get3(idy).Position - bodyCenter;
                var falloffMultiplier = gravityBodies.Get2(idy).Mass / diff.LengthSquared;
                forces += diff * falloffMultiplier;
            }

            return forces;
        }
    }
}
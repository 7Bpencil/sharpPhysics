using System;
using Engine.Physics.Components;
using Leopotam.Ecs;

namespace Engine.Physics.Systems
{
    public class SolveCollisions : IEcsRunSystem
    {
        private PhysicsSystemState state = null;

        public void Run()
        {
            foreach (var manifold in state.Manifolds)
            {
                ResolveCollision(manifold);
            }
        }

        private static void ResolveCollision(Manifold m)
        {
            ref var velocityA = ref m.BodyA.Get<Velocity>().Linear;
            ref var velocityB = ref m.BodyB.Get<Velocity>().Linear;

            var rigidBodyA = m.BodyA.Get<RigidBody>();
            var rigidBodyB = m.BodyB.Get<RigidBody>();


            if (float.IsNaN(m.Normal.X + m.Normal.Y))
            {
                return;
            }

            var velAlongNormal = Vector2.Dot(velocityB - velocityA, m.Normal);

            if (velAlongNormal > 0)
            {
                return;
            }

            var e = Math.Min(rigidBodyA.Restitution, rigidBodyB.Restitution);
            var j = -(1 + e) * velAlongNormal / (rigidBodyA.IMass + rigidBodyB.IMass);

            var impulse = m.Normal * j;

            if (!rigidBodyA.Locked) velocityA -= impulse * rigidBodyA.IMass;
            if (!rigidBodyB.Locked) velocityB += impulse * rigidBodyB.IMass;
        }
    }
}

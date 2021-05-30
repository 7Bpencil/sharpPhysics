using System;
using Engine.Physics.Components;
using Leopotam.EcsLite;

namespace Engine.Physics.Systems
{
    public class SolveCollisions : IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            var sharedData = systems.GetShared<SharedData>();
            var rigidBodies = sharedData.rigidBodies;
            var velocities = sharedData.velocities;
            var physicsData = sharedData.PhysicsSystemData;

            foreach (var manifold in physicsData.Manifolds)
            {
                ref var velocityA = ref velocities.Get(manifold.BodyA).Linear;
                ref var velocityB = ref velocities.Get(manifold.BodyB).Linear;

                var rigidBodyA = rigidBodies.Get(manifold.BodyA);
                var rigidBodyB = rigidBodies.Get(manifold.BodyB);

                ResolveCollision(manifold, ref velocityA, ref velocityB, rigidBodyA, rigidBodyB);
            }
        }

        private static void ResolveCollision(
            Manifold m,
            ref Vector2 velocityA, ref Vector2 velocityB,
            RigidBody rigidBodyA, RigidBody rigidBodyB)
        {
            if (float.IsNaN(m.Normal.X + m.Normal.Y)) return;

            var velAlongNormal = Vector2.Dot(velocityB - velocityA, m.Normal);

            if (velAlongNormal > 0) return;

            var e = Math.Min(rigidBodyA.Restitution, rigidBodyB.Restitution);
            var j = -(1 + e) * velAlongNormal / (rigidBodyA.IMass + rigidBodyB.IMass);

            var impulse = m.Normal * j;

            if (!rigidBodyA.Locked) velocityA -= impulse * rigidBodyA.IMass;
            if (!rigidBodyB.Locked) velocityB += impulse * rigidBodyB.IMass;
        }
    }
}

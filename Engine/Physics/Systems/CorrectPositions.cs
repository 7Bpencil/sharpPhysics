using Leopotam.EcsLite;
using Engine.Physics.Components;

namespace Engine.Physics.Systems
{
    public class CorrectPositions : IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            var sharedData = systems.GetShared<SharedData>();
            var physicsData = sharedData.PhysicsSystemData;
            var rigidBodies = sharedData.rigidBodies;
            var poses = sharedData.poses;

            foreach (var manifold in physicsData.Manifolds)
            {
                var rigidBodyA = rigidBodies.Get(manifold.BodyA);
                var rigidBodyB = rigidBodies.Get(manifold.BodyB);

                ref var positionA = ref poses.Get(manifold.BodyA).Position;
                ref var positionB = ref poses.Get(manifold.BodyB).Position;

                Correct(manifold, rigidBodyA, rigidBodyB, ref positionA, ref positionB);
            }
        }

        private static void Correct(
            Manifold m,
            RigidBody rigidBodyA, RigidBody rigidBodyB,
            ref Vector2 positionA, ref Vector2 positionB)
        {
            const float percent = 0.6F; // usually 20% to 80%
            var correction = m.Normal * (percent * (m.Penetration / (rigidBodyA.IMass + rigidBodyB.IMass)));

            if (!rigidBodyA.Locked) positionA += -correction * rigidBodyA.IMass;
            if (!rigidBodyB.Locked) positionB += correction * rigidBodyB.IMass;
        }
    }
}

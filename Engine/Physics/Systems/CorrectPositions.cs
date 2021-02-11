using Engine.Physics.Components;
using Leopotam.Ecs;

namespace Engine.Physics.Systems
{
    public class CorrectPositions : IEcsRunSystem
    {
        private PhysicsSystemState state = null;

        public void Run()
        {
            foreach (var manifold in state.Manifolds)
            {
                Correct(manifold);
            }
        }

        private static void Correct(Manifold m)
        {
            var rigidBodyA = m.BodyA.Get<RigidBody>();
            var rigidBodyB = m.BodyB.Get<RigidBody>();

            ref var positionA = ref m.BodyA.Get<Pose>().Position;
            ref var positionB = ref m.BodyB.Get<Pose>().Position;

            const float percent = 0.6F; // usually 20% to 80%
            var correction = m.Normal * (percent * (m.Penetration / (rigidBodyA.IMass + rigidBodyB.IMass)));

            if (!rigidBodyA.Locked) positionA += -correction * rigidBodyA.IMass;
            if (!rigidBodyB.Locked) positionB += correction * rigidBodyB.IMass;
        }
    }
}

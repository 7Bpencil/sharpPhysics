using System.Runtime.CompilerServices;
using Engine.Physics.Components;
using Engine.Physics.Components.Shapes;
using Engine.Physics.Helpers;
using Leopotam.Ecs;

namespace Engine.Physics.Systems
{
    public class NarrowPhase : IEcsRunSystem
    {
        private PhysicsSystemState state = null;

        public void Run()
        {
            foreach (var pair in state.CollisionPairs)
            {
                if (CalculateManifold(pair.BodyA, pair.BodyB, out var manifold))
                {
                    state.Manifolds.Add(manifold);
                }
            }
        }

        private static bool CalculateManifold(EcsEntity A, EcsEntity B, out Manifold manifold)
        {
            manifold = new Manifold {BodyA = A, BodyB = B};

            var typeA = A.Get<RigidBody>().Type;
            var typeB = B.Get<RigidBody>().Type;

            var positionA = A.Get<Pose>().Position;
            var positionB = B.Get<Pose>().Position;

            switch (GetCollisionType(typeA, typeB))
            {
                case CollisionType.BoxBox:
                {
                    return CollisionDetection.BoxBox(A.Get<Box>(), positionA, B.Get<Box>(), positionB, ref manifold);
                }
                case CollisionType.BoxCircle:
                {
                    return CollisionDetection.BoxCircle(A.Get<Box>(), positionA, B.Get<Circle>(), positionB, ref manifold);
                }
                case CollisionType.CircleBox:
                {
                    return CollisionDetection.BoxCircle(B.Get<Box>(), positionB, A.Get<Circle>(), positionA, ref manifold);
                }
                case CollisionType.CircleCircle:
                {
                    return CollisionDetection.CircleCircle(A.Get<Circle>(), positionA, B.Get<Circle>(), positionB, ref manifold);
                }
                default: return false;
            }
        }


        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        private static CollisionType GetCollisionType(ColliderType a, ColliderType b)
        {
            return (CollisionType) ((int) a * 10 + (int) b);
        }
    }
}

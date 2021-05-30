using System.Runtime.CompilerServices;
using Engine.Physics.Components;
using Engine.Physics.Components.Shapes;
using Engine.Physics.Helpers;
using Leopotam.EcsLite;

namespace Engine.Physics.Systems
{
    public class NarrowPhase : IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            var sharedData = systems.GetShared<SharedData>();
            var rigidBodies = sharedData.rigidBodies;
            var poses = sharedData.poses;
            var boxes = sharedData.boxShapes;
            var circles = sharedData.circleShapes;
            var physicsData = sharedData.PhysicsSystemData;

            foreach (var pair in physicsData.CollisionPairs) {
                if (CalculateManifold(pair.BodyA, pair.BodyB, rigidBodies, poses, boxes, circles, out var manifold))
                {
                    physicsData.Manifolds.Add(manifold);
                }
            }
        }

        private static bool CalculateManifold(
            int A, int B,
            EcsPool<RigidBody> rigidBodies, EcsPool<Pose> poses, EcsPool<Box> boxes, EcsPool<Circle> circles,
            out Manifold manifold)
        {
            manifold = new Manifold {BodyA = A, BodyB = B};

            var typeA = rigidBodies.Get(A).Type;
            var typeB = rigidBodies.Get(B).Type;

            var positionA = poses.Get(A).Position;
            var positionB = poses.Get(B).Position;

            switch (GetCollisionType(typeA, typeB))
            {
                case CollisionType.BoxBox:
                {
                    return CollisionDetection.BoxBox(boxes.Get(A), positionA, boxes.Get(B), positionB, ref manifold);
                }
                case CollisionType.BoxCircle:
                {
                    return CollisionDetection.BoxCircle(boxes.Get(A), positionA, circles.Get(B), positionB, ref manifold);
                }
                case CollisionType.CircleBox:
                {
                    return CollisionDetection.BoxCircle(boxes.Get(B), positionB, circles.Get(A), positionA, ref manifold);
                }
                case CollisionType.CircleCircle:
                {
                    return CollisionDetection.CircleCircle(circles.Get(A), positionA, circles.Get(B), positionB, ref manifold);
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

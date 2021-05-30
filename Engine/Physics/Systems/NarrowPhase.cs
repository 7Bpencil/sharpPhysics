using System.Collections.Generic;
using Leopotam.EcsLite;
using Engine.Physics.Components;
using Engine.Physics.Components.Shapes;
using CD = Engine.Physics.Helpers.CollisionDetection;

namespace Engine.Physics.Systems
{
    public class NarrowPhase : IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            var sharedData = systems.GetShared<SharedData>();
            var poses = sharedData.poses;
            var boxes = sharedData.boxShapes;
            var circles = sharedData.circleShapes;
            var physicsData = sharedData.PhysicsSystemData;

            foreach (var pair in physicsData.CollisionPairs)
            {
                var A = pair.BodyA;
                var B = pair.BodyB;
                var positionA = poses.Get(A).Position;
                var positionB = poses.Get(B).Position;

                CalculateManifolds(A, B, positionA, positionB, boxes, circles, physicsData.Manifolds);
            }
        }

        private static void CalculateManifolds(
            int A, int B, Vector2 positionA, Vector2 positionB, EcsPool<Box> boxes, EcsPool<Circle> circles, List<Manifold> manifolds)
        {
            var isBoxA = boxes.Has(A);
            var isBoxB = boxes.Has(B);
            var isCircleA = circles.Has(A);
            var isCircleB = circles.Has(B);

            // yeah, entity can have a circle and box components at the same time
            var manifold = new Manifold {BodyA = A, BodyB = B};
            if (isBoxA && isBoxB && CD.BoxBox(boxes.Get(A), positionA, boxes.Get(B), positionB, ref manifold))
            {
                manifolds.Add(manifold);
            }
            if (isBoxA && isCircleB && CD.BoxCircle(boxes.Get(A), positionA, circles.Get(B), positionB, ref manifold))
            {
                manifolds.Add(manifold);
            }
            if (isBoxB && isCircleA && CD.BoxCircle(boxes.Get(B), positionB, circles.Get(A), positionA, ref manifold))
            {
                manifolds.Add(manifold);
            }
            if (isCircleB && isCircleA && CD.CircleCircle(circles.Get(A), positionA, circles.Get(B), positionB, ref manifold))
            {
                manifolds.Add(manifold);
            }
        }

    }
}

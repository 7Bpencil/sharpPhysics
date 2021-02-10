using Engine.Physics.Components;
using Engine.Physics.Components.Colliders;
using Engine.Physics.Components.RigidBody;
using Leopotam.Ecs;

namespace Engine.Physics.Systems
{
    public class CalculateBoundingBoxes : IEcsRunSystem
    {
        private EcsFilter<RigidBody, Transform, Circle, BoundingBox> circles = null;
        private EcsFilter<RigidBody, Transform, Box, BoundingBox> boxes = null;

        public void Run()
        {
            ComputeForCircles();
            ComputeForBoxes();
        }

        private void ComputeForCircles()
        {
            foreach (var idx in circles)
            {
                ref var bbox = ref circles.Get4(idx);
                var circleRadius = circles.Get3(idx).Radius;
                var center = circles.Get2(idx).Position;

                var radiusVector = new Vector2(circleRadius);
                bbox.Min = center - radiusVector;
                bbox.Max = center + radiusVector;
            }
        }

        private void ComputeForBoxes()
        {
            foreach (var idx in boxes)
            {
                ref var bbox = ref boxes.Get4(idx);
                var halfSize = boxes.Get3(idx).HalfSize;
                var center = boxes.Get2(idx).Position;

                bbox.Min = center - halfSize;
                bbox.Max = center + halfSize;
            }
        }
    }
}

using Engine.Physics.Components;
using Engine.Physics.Components.Shapes;
using Leopotam.Ecs;

namespace Engine.Physics.Systems
{
    public class UpdateBoundingBoxes : IEcsRunSystem
    {
        private EcsFilter<RigidBody, Pose, Circle, BoundingBox> circles = null;
        private EcsFilter<RigidBody, Pose, Box, BoundingBox> boxes = null;

        public void Run()
        {
            UpdateCircles();
            UpdateBoxes();
        }

        private void UpdateCircles()
        {
            foreach (var idx in circles)
            {
                var center = circles.Get2(idx).Position;
                var circleRadius = circles.Get3(idx).Radius;
                ref var bbox = ref circles.Get4(idx);

                var radiusVector = new Vector2(circleRadius);
                bbox.Min = center - radiusVector;
                bbox.Max = center + radiusVector;
            }
        }

        private void UpdateBoxes()
        {
            foreach (var idx in boxes)
            {
                var center = boxes.Get2(idx).Position;
                var halfSize = boxes.Get3(idx).HalfSize;
                ref var bbox = ref boxes.Get4(idx);

                bbox.Min = center - halfSize;
                bbox.Max = center + halfSize;
            }
        }

    }
}

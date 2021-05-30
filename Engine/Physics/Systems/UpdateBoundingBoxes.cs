using Engine.Physics.Components;
using Engine.Physics.Components.Shapes;
using Leopotam.EcsLite;

namespace Engine.Physics.Systems
{
    public class UpdateBoundingBoxes : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter circles;
        private EcsFilter boxes;

        public void Init(EcsSystems systems)
        {
            var world = systems.GetWorld();

            circles = world.Filter<RigidBody>().Inc<Pose>().Inc<Circle>().Inc<BoundingBox>().End();
            boxes = world.Filter<RigidBody>().Inc<Pose>().Inc<Box>().Inc<BoundingBox>().End();
        }

        public void Run(EcsSystems systems)
        {
            var sharedData = systems.GetShared<SharedData>();
            UpdateCircles(sharedData.poses, sharedData.circleShapes, sharedData.bboxes);
            UpdateBoxes(sharedData.poses, sharedData.boxShapes, sharedData.bboxes);
        }

        private void UpdateCircles(EcsPool<Pose> poses, EcsPool<Circle> circleShapes, EcsPool<BoundingBox> bboxes)
        {
            foreach (var idx in circles)
            {
                var center = poses.Get(idx).Position;
                var radius = circleShapes.Get(idx).Radius;
                ref var bbox = ref bboxes.Get(idx);

                var radiusVector = new Vector2(radius);
                bbox.Min = center - radiusVector;
                bbox.Max = center + radiusVector;
            }
        }

        private void UpdateBoxes(EcsPool<Pose> poses, EcsPool<Box> boxShapes, EcsPool<BoundingBox> bboxes)
        {
            foreach (var idx in boxes)
            {
                var center = poses.Get(idx).Position;
                var halfSize = boxShapes.Get(idx).HalfSize;
                ref var bbox = ref bboxes.Get(idx);

                bbox.Min = center - halfSize;
                bbox.Max = center + halfSize;
            }
        }

    }
}

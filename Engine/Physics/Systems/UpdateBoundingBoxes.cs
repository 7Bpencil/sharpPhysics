using Leopotam.EcsLite;
using Engine.Physics.Components;
using Engine.Physics.Components.Shapes;

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
            foreach (var entity in circles)
            {
                var center = poses.Get(entity).Position;
                var radius = circleShapes.Get(entity).Radius;
                ref var bbox = ref bboxes.Get(entity);

                var radiusVector = new Vector2(radius);
                bbox.Min = center - radiusVector;
                bbox.Max = center + radiusVector;
            }
        }

        private void UpdateBoxes(EcsPool<Pose> poses, EcsPool<Box> boxShapes, EcsPool<BoundingBox> bboxes)
        {
            foreach (var entity in boxes)
            {
                var center = poses.Get(entity).Position;
                var halfSize = boxShapes.Get(entity).HalfSize;
                ref var bbox = ref bboxes.Get(entity);

                bbox.Min = center - halfSize;
                bbox.Max = center + halfSize;
            }
        }

    }
}

using Leopotam.EcsLite;
using Engine.Physics.Components;
using Engine.Physics.Components.Shapes;

namespace Engine.Render.Systems
{
    public class DrawColliders : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter circles;
        private EcsFilter boxes;

        public void Init(EcsSystems systems)
        {
            var world = systems.GetWorld();

            circles = world.Filter<RigidBody>().Inc<Pose>().Inc<Circle>().End();
            boxes = world.Filter<RigidBody>().Inc<Pose>().Inc<Box>().End();
        }

        public void Run(EcsSystems systems)
        {
            var sharedData = systems.GetShared<SharedData>();
            var drawingData = sharedData.DrawingSystemsData;

            var mToP = drawingData.MetersToPixels;
            var canvasHeight = drawingData.CanvasHeight;
            var gfxBuffer = drawingData.gfxBuffer;
            var colliderBrush = drawingData.ColliderBrush;

            DrawCircles(sharedData.poses, sharedData.circleShapes);
            DrawBoxes(sharedData.poses, sharedData.boxShapes);

            void DrawCircles(EcsPool<Pose> poses, EcsPool<Circle> circleShapes)
            {
                foreach (var idx in circles)
                    Renderer.DrawCircle(
                        poses.Get(idx).Position * mToP,
                        circleShapes.Get(idx).Radius * mToP,
                        colliderBrush, gfxBuffer, canvasHeight);
            }

            void DrawBoxes(EcsPool<Pose> poses, EcsPool<Box> boxShapes)
            {
                foreach (var idx in boxes)
                    Renderer.DrawBox(
                        poses.Get(idx).Position * mToP,
                        boxShapes.Get(idx).HalfSize * mToP,
                        colliderBrush, gfxBuffer, canvasHeight);
            }
        }

    }
}

using Engine.Physics.Components;
using Engine.Physics.Components.Shapes;
using Leopotam.Ecs;

namespace Engine.Render.Systems
{
    public class DrawColliders : IEcsRunSystem
    {
        private EcsFilter<RigidBody, Pose, Circle> circles = null;
        private EcsFilter<RigidBody, Pose, Box> boxes = null;

        private DrawingState drawingState = null;

        public void Run()
        {
            var mToP = drawingState.MetersToPixels;
            var canvasHeight = drawingState.CanvasHeight;
            var gfxBuffer = drawingState.gfxBuffer;
            var colliderBrush = drawingState.ColliderBrush;

            foreach (var idx in circles)
                Renderer.DrawCircle(
                    circles.Get2(idx).Position * mToP,
                    circles.Get3(idx).Radius * mToP,
                    colliderBrush, gfxBuffer, canvasHeight);

            foreach (var idx in boxes)
                Renderer.DrawBox(
                    boxes.Get2(idx).Position * mToP,
                    boxes.Get3(idx).HalfSize * mToP,
                    colliderBrush, gfxBuffer, canvasHeight);
        }

    }
}

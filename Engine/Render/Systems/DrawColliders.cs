using System.Drawing;
using Engine.Physics;
using Engine.Physics.Components;
using Engine.Physics.Components.Shapes;
using Engine.Physics.Components.RigidBody;
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

            foreach (var idx in circles)
                DrawCircle(
                    circles.Get2(idx).Position * mToP,
                    circles.Get3(idx).Radius * mToP,
                    drawingState.CircleBrush,
                    drawingState.gfxBuffer);

            foreach (var idx in boxes)
                DrawBox(
                    boxes.Get2(idx).Position * mToP,
                    boxes.Get3(idx).HalfSize * mToP,
                    drawingState.RectangleBrush,
                    drawingState.gfxBuffer);
        }

        private static void DrawCircle(Vector2 center, float radius, Brush brush, Graphics gfxBuffer)
        {
            gfxBuffer.FillEllipse(brush, center.X - radius, center.Y - radius, radius * 2, radius * 2);
        }

        private static void DrawBox(Vector2 center, Vector2 halfSize, Brush brush, Graphics gfxBuffer)
        {
            var topLeft = center - halfSize;
            gfxBuffer.FillRectangle(brush, topLeft.X, topLeft.Y, 2 * halfSize.X, 2 * halfSize.Y);
        }
    }
}

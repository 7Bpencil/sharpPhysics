using System.Drawing;
using Engine.Physics;
using Engine.Physics.Components;
using Engine.Physics.Components.Colliders;
using Engine.Physics.Components.RigidBody;
using Leopotam.Ecs;

namespace Engine.Render.Systems
{
    public class DrawColliders : IEcsRunSystem
    {
        private EcsFilter<RigidBody, Transform, Circle> circles = null;
        private EcsFilter<RigidBody, Transform, Box> boxes = null;
        private PhysicsSettings settings = null;
        private DrawingState drawingState = null;

        public void Run()
        {
            var mToP = settings.MetersToPixels;
            
            foreach (var idx in circles)
                DrawCircle(circles.Get2(idx).Position * mToP, circles.Get3(idx).Radius * mToP, drawingState.CircleBrush, drawingState.gfxBuffer);
            foreach (var idx in boxes)
                DrawBox(boxes.Get2(idx).Position * mToP, boxes.Get3(idx).HalfSize * mToP, drawingState.RectangleBrush, drawingState.gfxBuffer);
        }
        
        private void DrawCircle(Vector2 center, float radius, Brush brush, Graphics gfxBuffer)
        {
            gfxBuffer.FillEllipse(
                brush,
                center.X - radius, center.Y - radius,
                radius * 2, radius * 2);
        }
        
        private void DrawBox(Vector2 center, Vector2 halfSize, Brush brush, Graphics gfxBuffer)
        {
            var topLeft = center - halfSize;
            gfxBuffer.FillRectangle(
                brush, 
                topLeft.X, topLeft.Y,
                2 * halfSize.X, 2 * halfSize.Y);
        }
    }
}
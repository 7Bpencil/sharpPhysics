using System.Drawing;
using Engine.Physics;
using Engine.Physics.Components;
using Leopotam.Ecs;

namespace Engine.Render.Systems
{
    public class DrawBoundingBoxes : IEcsRunSystem
    {
        private EcsFilter<BoundingBox> boundingBoxes = null;
        private PhysicsSettings settings = null;
        private DrawingState drawingState = null;

        public void Run()
        {
            var mToP = settings.MetersToPixels;

            foreach (var idx in boundingBoxes)
            {
                var bbox = boundingBoxes.Get1(idx);
                DrawBoundingBox(bbox.Min * mToP, bbox.Max * mToP, drawingState.BBoxPen, drawingState.gfxBuffer);
            }
        }

        private static void DrawBoundingBox(Vector2 min, Vector2 max, Pen pen, Graphics gfxBuffer)
        {
            var size = max - min;
            gfxBuffer.DrawRectangle(pen, min.X, min.Y, size.X, size.Y);
        }
    }
}

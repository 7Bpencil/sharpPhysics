using System.Drawing;
using Engine.Physics;
using Engine.Physics.Components;
using Leopotam.Ecs;

namespace Engine.Render.Systems
{
    public class DrawBoundingBoxes : IEcsRunSystem
    {
        private EcsFilter<BoundingBox> boundingBoxes = null;

        private DrawingState drawingState = null;

        public void Run()
        {
            var mToP = drawingState.MetersToPixels;
            var canvasHeight = drawingState.CanvasHeight;

            foreach (var idx in boundingBoxes)
            {
                var bbox = boundingBoxes.Get1(idx);
                DrawBoundingBox(
                    bbox.Min * mToP,
                    bbox.Max * mToP,
                    drawingState.BBoxPen, drawingState.gfxBuffer, canvasHeight);
            }
        }

        private static void DrawBoundingBox(Vector2 min, Vector2 max, Pen pen, Graphics gfxBuffer, float canvasHeight)
        {
            var (sizeX, sizeY) = max - min;
            gfxBuffer.DrawRectangle(pen, min.X, canvasHeight - min.Y - sizeY, sizeX, sizeY);
        }
    }
}

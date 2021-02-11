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
            var gfxBuffer = drawingState.gfxBuffer;
            var bboxPen = drawingState.BBoxPen;

            foreach (var idx in boundingBoxes)
            {
                var bbox = boundingBoxes.Get1(idx);
                Renderer.DrawBoundingBox(
                    bbox.Min * mToP,
                    bbox.Max * mToP,
                    bboxPen, gfxBuffer, canvasHeight);
            }
        }

    }
}

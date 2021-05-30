using Engine.Physics.Components;
using Leopotam.EcsLite;

namespace Engine.Render.Systems
{
    public class DrawBoundingBoxes : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter boundingBoxes;

        public void Init(EcsSystems systems)
        {
            var world = systems.GetWorld();

            boundingBoxes = world.Filter<BoundingBox>().End();
        }

        public void Run(EcsSystems systems)
        {
            var sharedData = systems.GetShared<SharedData>();
            var drawingData = sharedData.DrawingSystemsData;

            var mToP = drawingData.MetersToPixels;
            var canvasHeight = drawingData.CanvasHeight;
            var gfxBuffer = drawingData.gfxBuffer;
            var bboxPen = drawingData.BBoxPen;

            var bboxes = sharedData.bboxes;

            foreach (var idx in boundingBoxes)
            {
                ref var bbox = ref bboxes.Get(idx);
                Renderer.DrawBoundingBox(
                    bbox.Min * mToP,
                    bbox.Max * mToP,
                    bboxPen, gfxBuffer, canvasHeight);
            }
        }

    }
}

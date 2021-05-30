using System.Drawing;

namespace Engine.Render
{
    public class DrawingSystemsData
    {
        public Graphics gfxBuffer;

        public SolidBrush ColliderBrush;
        public SolidBrush VelocityBrush;
        public Pen BBoxPen;
        public float CanvasWidth;
        public float CanvasHeight;
        public float MetersToPixels;
        public float PixelsToMeters;

        public DrawingSystemsData(int width, int height)
        {
            CanvasWidth = width;
            CanvasHeight = height;
            BBoxPen = new Pen(Color.White);
            ColliderBrush = new SolidBrush(Color.Silver);
            VelocityBrush = new SolidBrush(Color.Black);
            MetersToPixels = 60f;  // 1m = 60px
            PixelsToMeters = 1 / MetersToPixels;
        }

    }
}

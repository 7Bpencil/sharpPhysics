using System.Drawing;

namespace Engine.Render
{
    public class DrawingState
    {
        public Bitmap bmpBuffer;
        public Graphics gfxBuffer;
        public SolidBrush ColliderBrush;
        public SolidBrush VelocityBrush;
        public Pen BBoxPen;
        public float CanvasHeight;
        public float MetersToPixels;
        public float PixelsToMeters;

        public DrawingState(int width, int height)
        {
            bmpBuffer = new Bitmap(width, height);
            gfxBuffer = Graphics.FromImage(bmpBuffer);
            BBoxPen = new Pen(Color.White);
            ColliderBrush = new SolidBrush(Color.Silver);
            VelocityBrush = new SolidBrush(Color.Black);
            CanvasHeight = height;
            MetersToPixels = 60f;  // 1m = 60px
            PixelsToMeters = 1 / MetersToPixels;
        }
    }
}

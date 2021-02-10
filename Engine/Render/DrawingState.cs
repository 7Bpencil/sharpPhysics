using System.Drawing;

namespace Engine.Render
{
    public class DrawingState
    {
        public Bitmap bmpBuffer;
        public Graphics gfxBuffer;
        public Brush RectangleBrush;
        public Brush CircleBrush;
        public Pen BBoxPen;

        public float MetersToPixels;
        public float PixelsToMeters;

        public DrawingState(int width, int height)
        {
            bmpBuffer = new Bitmap(width, height);
            gfxBuffer = Graphics.FromImage(bmpBuffer);
            RectangleBrush = new SolidBrush(Color.Coral);
            BBoxPen = new Pen(Color.White);
            CircleBrush = new SolidBrush(Color.Silver);

            MetersToPixels = 60f;  // 1m = 60px
            PixelsToMeters = 1 / MetersToPixels;
        }
    }
}

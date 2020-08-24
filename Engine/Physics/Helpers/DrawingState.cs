﻿using System.Drawing;

namespace Engine.Physics.Helpers
{
    public class DrawingState
    {
        public Bitmap bmpBuffer;
        public Graphics gfxBuffer;
        public Brush RectangleBrush;
        public Brush CircleBrush;
        public Pen AABBpen;

        public DrawingState(int width, int height)
        {
            bmpBuffer = new Bitmap(width, height);
            gfxBuffer = Graphics.FromImage(bmpBuffer);
            RectangleBrush = new SolidBrush(Color.Coral);
            AABBpen = new Pen(Color.White);
            CircleBrush = new SolidBrush(Color.Silver);
        }
    }
}
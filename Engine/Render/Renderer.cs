using System.Drawing;
using System.Runtime.CompilerServices;
using Engine.Physics;

namespace Engine.Render
{
    public static class Renderer
    {
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static void DrawCircle(Vector2 center, float radius, Brush brush, Graphics gfxBuffer, float canvasHeight)
        {
            gfxBuffer.FillEllipse(brush, center.X - radius, canvasHeight - center.Y - radius, radius * 2, radius * 2);
        }

        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static void DrawBox(Vector2 center, Vector2 halfSize, Brush brush, Graphics gfxBuffer, float canvasHeight)
        {
            gfxBuffer.FillRectangle(brush, center.X - halfSize.X, canvasHeight - center.Y - halfSize.Y, halfSize.X * 2, halfSize.Y * 2);
        }

        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static void DrawBoundingBox(Vector2 min, Vector2 max, Pen pen, Graphics gfxBuffer, float canvasHeight)
        {
            var (sizeX, sizeY) = max - min;
            gfxBuffer.DrawRectangle(pen, min.X, canvasHeight - min.Y - sizeY, sizeX, sizeY);
        }

    }
}

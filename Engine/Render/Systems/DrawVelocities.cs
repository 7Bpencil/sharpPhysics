using System;
using System.Drawing;
using Engine.Physics;
using Engine.Physics.Components;
using Engine.Physics.Components.Colliders;
using Engine.Physics.Components.RigidBody;
using Leopotam.Ecs;

namespace Engine.Render.Systems
{
    public class DrawVelocities : IEcsRunSystem
    {
        private EcsFilter<RigidBody, Transform, Circle, Velocity> circles = null;
        private EcsFilter<RigidBody, Transform, Box> boxes = null;
        private PhysicsSettings settings = null;
        private DrawingState drawingState = null;

        public void Run()
        {
            var mToP = settings.MetersToPixels;
            
            foreach (var idx in circles)
                DrawCircle(
                    circles.Get2(idx).Position * mToP, 
                    circles.Get3(idx).Radius * mToP, 
                    circles.Get4(idx).Value * mToP / 4, 
                    drawingState.gfxBuffer);
            
            foreach (var idx in boxes)
                DrawBox(
                    boxes.Get2(idx).Position * mToP, 
                    boxes.Get3(idx).HalfSize * mToP, 
                    drawingState.RectangleBrush, drawingState.gfxBuffer);
        }
        
        private void DrawCircle(Vector2 center, float radius, Vector2 velocity, Graphics gfxBuffer)
        {
            int r, g, b;
            var particleSpeed = 220 - Math.Min((int) velocity.Length, 220);
            HsvToRgb(particleSpeed, 1, 1, out r, out g, out b);

            gfxBuffer.FillEllipse(
                new SolidBrush(Color.FromArgb(255, r, g, b)),
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
        
        public static void HsvToRgb(double H, double S, double V, out int r, out int g, out int b)
        {
            double R, G, B;
            
            while (H < 0) H += 360;
            while (H >= 360) H -= 360;

            if (V <= 0) R = G = B = 0; 
            else if (S <= 0) R = G = B = V;
            else
            {
                var hf = H / 60.0;
                var i = (int)Math.Floor(hf);
                var f = hf - i;
                var pv = V * (1 - S);
                var qv = V * (1 - S * f);
                var tv = V * (1 - S * (1 - f));
                switch (i)
                {
                    case 0: R = V; G = tv; B = pv; break;
                    case 1: R = qv; G = V; B = pv; break;
                    case 2: R = pv; G = V; B = tv; break;
                    case 3: R = pv; G = qv; B = V; break;
                    case 4: R = tv; G = pv; B = V; break;
                    case 5: R = V; G = pv; B = qv; break;

                    // Just in case we overshoot on math

                    case 6: R = V; G = tv; B = pv; break;
                    case -1: R = V; G = pv; B = qv; break;
                    default: R = G = B = V; break;
                }
            }
            
            r = Clamp((int)(R * 255.0));
            g = Clamp((int)(G * 255.0));
            b = Clamp((int)(B * 255.0));
        }

        /// <summary>
        /// Clamp a value to 0-255
        /// </summary>
        private static int Clamp(int i)
        {
            if (i < 0) return 0;
            if (i > 255) return 255;
            return i;
        }
    }
}
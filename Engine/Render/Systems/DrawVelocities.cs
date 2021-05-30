using System;
using System.Drawing;
using Engine.Physics.Components;
using Engine.Physics.Components.Shapes;
using Engine.Physics.Helpers;
using Leopotam.EcsLite;

namespace Engine.Render.Systems
{
    public class DrawVelocities : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter circles;
        private EcsFilter boxes;

        public void Init(EcsSystems systems)
        {
            var world = systems.GetWorld();

            circles = world.Filter<RigidBody>().Inc<Pose>().Inc<Circle>().Inc<Velocity>().End();
            boxes = world.Filter<RigidBody>().Inc<Pose>().Inc<Box>().End();
        }

        public void Run(EcsSystems systems)
        {
            var sharedData = systems.GetShared<SharedData>();
            var drawingData = sharedData.DrawingSystemsData;

            var mToP = drawingData.MetersToPixels;
            var canvasHeight = drawingData.CanvasHeight;
            var gfxBuffer = drawingData.gfxBuffer;
            var colliderBrush = drawingData.ColliderBrush;
            var velocityBrush = drawingData.VelocityBrush;

            DrawCircles(sharedData.poses, sharedData.circleShapes, sharedData.velocities);
            DrawBoxes(sharedData.poses, sharedData.boxShapes);

            void DrawCircles(EcsPool<Pose> poses, EcsPool<Circle> circleShapes, EcsPool<Velocity> velocities)
            {
                foreach (var idx in circles)
                {
                    var velocity = velocities.Get(idx).Linear * mToP / 4;
                    var particleSpeed = 220 - Math.Min((int) velocity.Length, 220);
                    HsvToRgb(particleSpeed, 1, 1, out var r, out var g, out var b);
                    velocityBrush.Color = Color.FromArgb(255, r, g, b);

                    Renderer.DrawCircle(
                        poses.Get(idx).Position * mToP,
                        circleShapes.Get(idx).Radius * mToP,
                        velocityBrush, gfxBuffer, canvasHeight);
                }
            }

            void DrawBoxes(EcsPool<Pose> poses, EcsPool<Box> boxShapes)
            {
                foreach (var idx in boxes)
                    Renderer.DrawBox(
                        poses.Get(idx).Position * mToP,
                        boxShapes.Get(idx).HalfSize * mToP,
                        colliderBrush, gfxBuffer, canvasHeight);
            }
        }

        public static void HsvToRgb(float H, float S, float V, out int r, out int g, out int b)
        {
            float R, G, B;

            while (H < 0) H += 360;
            while (H >= 360) H -= 360;

            if (V <= 0) R = G = B = 0;
            else if (S <= 0) R = G = B = V;
            else
            {
                var hf = H / 60f;
                var i = (int) Math.Floor(hf);
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

            r = (int) (MathHelper.Clamp(R, 0f, 1f) * 255);
            g = (int) (MathHelper.Clamp(G, 0f, 1f) * 255);
            b = (int) (MathHelper.Clamp(B, 0f, 1f) * 255);
        }

    }
}

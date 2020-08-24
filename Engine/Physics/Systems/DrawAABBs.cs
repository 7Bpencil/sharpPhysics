﻿using System.Drawing;
using Engine.Physics.Components;
using Engine.Physics.Helpers;
using Leopotam.Ecs;

namespace Engine.Physics.Systems
{
    public class DrawAABBs : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter<AABB> boundingBoxes = null;
        private PhysicsSettings settings = null;
        private DrawingState drawingState = null;
        
        public void Init()
        {
            //throw new System.NotImplementedException();
        }

        public void Run()
        {
            var mToP = settings.MetersToPixels;
            
            foreach (var idx in boundingBoxes)
            {
                var aabb = boundingBoxes.Get1(idx);
                DrawAABB(aabb.Min * mToP, aabb.Max * mToP, drawingState.AABBpen, drawingState.gfxBuffer);
            }
        }
        
        private void DrawAABB(Vector2 min, Vector2 max, Pen pen, Graphics gfxBuffer)
        {
            var size = max - min;
            gfxBuffer.DrawRectangle(
                pen, 
                min.X, min.Y,
                size.X, size.Y);
        }
    }
}
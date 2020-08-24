using System;
using System.Runtime.CompilerServices;
using Engine.Physics.Components;

namespace Engine.Physics.Helpers
{
    public static class PhysMath
    {
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static void CorrectBoundingBox(ref AABB aabb)
        {
            aabb.Min = new Vector2(Math.Min(aabb.Min.X, aabb.Max.X), Math.Min(aabb.Min.Y, aabb.Max.Y));
            aabb.Max = new Vector2(Math.Max(aabb.Min.X, aabb.Max.X), Math.Max(aabb.Min.Y, aabb.Max.Y));
        }
        
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static void RoundToZero(ref Vector2 vector, float cutoff)
        {
            if (vector.Length < cutoff)
            {
                vector.X = 0;
                vector.Y = 0;
            }
        }
    }
}
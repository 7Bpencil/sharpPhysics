using System;
using physics.Engine.Structs;

namespace physics.Engine.Helpers
{
    public static class PhysMath
    {
        public static void CorrectBoundingBox(AABB aabb)
        {
            aabb.Min = new Vector2(Math.Min(aabb.Min.X, aabb.Max.X), Math.Min(aabb.Min.Y, aabb.Max.Y));
            aabb.Max = new Vector2(Math.Max(aabb.Min.X, aabb.Max.X), Math.Max(aabb.Min.Y, aabb.Max.Y));
        }
        
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
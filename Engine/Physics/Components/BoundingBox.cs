﻿using System.Runtime.CompilerServices;

namespace Engine.Physics.Components
{
    /// <summary>
    /// axis-aligned bounding box
    /// </summary>
    public struct BoundingBox
    {
        public Vector2 Min;
        public Vector2 Max;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(ref BoundingBox a, ref BoundingBox b)
        {
            return Intersects(a.Min, a.Max, b.Min, b.Max);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(in Vector2 minA, in Vector2 maxA, in Vector2 minB, in Vector2 maxB)
        {
            return maxA.X >= minB.X & maxA.Y >= minB.Y &
                   maxB.X >= minA.X & maxB.Y >= minA.Y;
        }
    }
}

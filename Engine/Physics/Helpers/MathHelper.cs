using System;
using System.Runtime.CompilerServices;

namespace Engine.Physics.Helpers
{
    public static class MathHelper
    {
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static float Clamp(float val, float low, float high)
        {
            return Math.Max(low, Math.Min(val, high));
        }

        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public static void Clamp(ref float val, float low, float high)
        {
            val = Math.Max(low, Math.Min(val, high));
        }

    }
}

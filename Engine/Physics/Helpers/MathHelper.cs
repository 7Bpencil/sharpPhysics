using System;
using System.Runtime.CompilerServices;

namespace Engine.Physics.Helpers
{
    public static class MathHelper
    {
        public const float Pi = 3.141592653589793238462643383279502884197169399375105820974944592307816406286208998628034825342117067982148086513282306647093844609550582231725359408128481117450284102701938521105559644622948954930382f;

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

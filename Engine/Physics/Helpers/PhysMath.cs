using System.Runtime.CompilerServices;

namespace Engine.Physics.Helpers
{
    public static class PhysMath
    {
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
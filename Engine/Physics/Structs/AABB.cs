namespace Engine.Physics.Structs
{
    /// <summary>
    /// Axis Aligned Bounding Box
    /// </summary>
    public class AABB : Shape
    {
        public Vector2 Min;
        public Vector2 Max;

        public AABB(Vector2 min, Vector2 max)
        {
            Min = min;
            Max = max;
        }

        public Vector2 Size => new Vector2(Max.X - Min.X, Max.Y - Min.Y);

        public override float GetArea()
        {
            return (Max.X - Min.X) * (Max.Y - Min.Y);
        }

        public override void MoveBy(Vector2 delta)
        {
            Min += delta;
            Max += delta;
        }

        public override bool ContainsPoint(Vector2 point)
        {
            return point.X < Max.X && point.X > Min.X && point.Y < Max.Y && point.Y > Min.Y;
        }

        public override AABB GetBoundingBox()
        {
            return this;
        }
        
        public static bool operator ==(AABB left, AABB right)
        {
            return left.Min == right.Min && left.Max == right.Max;
        }

        public static bool operator !=(AABB left, AABB right)
        {
            return !(left == right);
        }
    }
}
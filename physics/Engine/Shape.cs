using physics.Engine;
using physics.Engine.Structs;

namespace App.Engine.Physics
{
    public abstract class Shape
    {
        public abstract float GetArea();
        public abstract void MoveBy(Vector2 delta);
        public abstract bool ContainsPoint(Vector2 point);
        public abstract AABB GetBoundingBox();
    }
}
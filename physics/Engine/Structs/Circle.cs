using App.Engine.Physics;
using Engine.Helpers;

namespace physics.Engine.Structs
{
    public class Circle : Shape
    {
        public Vector2 Center;
        public float Radius;
        private AABB boundingBox;

        public Circle(Vector2 center, float radius)
        {
            Center = center;
            Radius = radius;
            var radiusVector = new Vector2(Radius);
            boundingBox = new AABB(Center - radiusVector, Center + radiusVector);
        }

        public override float GetArea()
        {
            return MathHelper.Pi * Radius * Radius;
        }
        
        public override void MoveBy(Vector2 delta)
        {
            Center += delta;
            boundingBox.MoveBy(delta);
        }
        
        public override bool ContainsPoint(Vector2 point)
        {
            return Vector2.DistanceSquared(point, Center) < Radius * Radius;
        }
        
        public override AABB GetBoundingBox()
        {
            return boundingBox;
        }
    }
}
using System;
using App.Engine.Physics;
using physics.Engine.Structs;

namespace physics.Engine.Classes
{
    public class PhysicsObject
    {
        public bool Locked;
        public Shape Shape;
        public Vector2 Velocity;
        public Vector2 Center;
        public float Restitution;
        public float Mass;
        public float IMass;

        public PhysicsObject(Shape shape, Vector2 center, float r, bool locked, float m = 0)
        {
            Shape = shape;
            Velocity = Vector2.Zero;
            Center = center;
            Restitution = r;
            Mass = (int) m == 0 ? shape.GetArea() : m;
            IMass = 1 / Mass;
            Locked = locked;
        }

        public bool Contains(Vector2 point)
        {
            return Shape.ContainsPoint(point);
        }

        public void Move(float dt)
        {
            if (Mass >= 1000000)
            {
                return;
            }

            RoundSpeedToZero();
            
            Shape.MoveBy(Velocity * dt);
            Center += Velocity * dt;
        }

        private void RoundSpeedToZero()
        {
            if (Math.Abs(Velocity.X) + Math.Abs(Velocity.Y) < .01F)
            {
                Velocity = Vector2.Zero;
            }
        }

        public void Move(Vector2 dVector)
        {
            if (Locked)
            {
                return;
            }
            
            Shape.MoveBy(dVector);
            Center += dVector;
        }

        public AABB GetBoundingBox()
        {
            return Shape.GetBoundingBox();
        }
    }
}
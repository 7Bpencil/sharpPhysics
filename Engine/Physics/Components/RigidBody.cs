using System;

namespace Engine.Physics.Components
{
    public struct RigidBody
    {
        public bool Locked;
        public float Restitution;
        public float Mass;
        public float IMass;
        public ColliderType Type;

        public RigidBody(ColliderType type, float mass, float restitution, bool locked) : this()
        {
            if (mass <= 0) throw new ArgumentException();

            Type = type;
            Mass = mass;
            IMass = 1 / mass;
            Restitution = restitution;
            Locked = locked;
        }
    }

    public enum ColliderType
    {
        Circle = 1,
        Box = 2
    }

    public enum CollisionType
    {
        CircleCircle = ColliderType.Circle * 10 + ColliderType.Circle,
        BoxBox       = ColliderType.Box * 10 + ColliderType.Box,
        CircleBox    = ColliderType.Circle * 10 + ColliderType.Box,
        BoxCircle    = ColliderType.Box * 10 + ColliderType.Circle
    }
}

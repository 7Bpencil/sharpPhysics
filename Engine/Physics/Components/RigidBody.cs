using System;

namespace Engine.Physics.Components
{
    public struct RigidBody
    {
        public bool Locked;
        public float Restitution;
        public float Mass;
        public float IMass;

        public RigidBody(float mass, float restitution, bool locked)
        {
            if (mass <= 0) throw new ArgumentException("mass can't be negative");

            Mass = mass;
            IMass = 1 / mass;
            Restitution = restitution;
            Locked = locked;
        }
    }

}

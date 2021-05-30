using System.Collections.Generic;
using Engine.Physics.Components;

namespace Engine.Physics
{
    public class PhysicsSystemData
    {
        /// length is meters
        /// time is seconds
        /// velocity is meters/seconds
        /// mass is kg
        /// Maybe it's not really important in 2D but it's a less mess

        public float dt;
        public Vector2 Gravity;
        public BoundingBox Boundaries;

        public readonly List<CollisionPair> CollisionPairs;
        public readonly List<Manifold> Manifolds;

        public PhysicsSystemData(float dt, Vector2 gravity, BoundingBox boundaries)
        {
            this.dt = dt;
            Gravity = gravity;
            Boundaries = boundaries;

            CollisionPairs = new List<CollisionPair> {Capacity = 32};
            Manifolds = new List<Manifold> {Capacity = 32};
        }

    }
}

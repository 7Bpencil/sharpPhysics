using System.Collections.Generic;

namespace Engine.Physics.Components
{
    public class PhysicsSystemState
    {
        public readonly List<CollisionPair> CollisionPairs;
        public readonly List<Manifold> Manifolds;

        public PhysicsSystemState()
        {
            CollisionPairs = new List<CollisionPair> {Capacity = 32};
            Manifolds = new List<Manifold> {Capacity = 32};
        }
    }
}
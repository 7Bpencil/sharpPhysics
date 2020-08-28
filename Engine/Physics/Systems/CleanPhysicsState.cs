using Engine.Physics.Components;
using Leopotam.Ecs;

namespace Engine.Physics.Systems
{
    public class CleanPhysicsState : IEcsRunSystem
    {
        private PhysicsSystemState state = null;

        public void Run()
        {
            state.CollisionPairs.Clear();
            state.Manifolds.Clear();
        }
    }
}
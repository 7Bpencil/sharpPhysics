using Engine.Physics.Components;
using Leopotam.Ecs;

namespace Engine.Physics.Systems
{
    public class CleanPhysicsState : IEcsInitSystem, IEcsRunSystem
    {
        private PhysicsSystemState state = null;
        
        public void Init() { }

        public void Run()
        {
            state.CollisionPairs.Clear();
            state.Manifolds.Clear();
        }
    }
}
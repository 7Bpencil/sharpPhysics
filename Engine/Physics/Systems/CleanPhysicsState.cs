using Leopotam.EcsLite;

namespace Engine.Physics.Systems
{
    public class CleanPhysicsState : IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            var physicsData = systems.GetShared<SharedData>().PhysicsSystemData;

            physicsData.CollisionPairs.Clear();
            physicsData.Manifolds.Clear();
        }
    }
}

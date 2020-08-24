using Engine.Physics.Components;
using Engine.Physics.Components.RigidBody;
using Leopotam.Ecs;

namespace Engine.Physics.Systems
{
    public class DeleteUnusedObjects : IEcsRunSystem
    {
        private EcsFilter<RigidBody, Transform> rigidBodies = null;

        public void Run()
        {
            foreach (var idx in rigidBodies)
            {
                var center = rigidBodies.Get2(idx).Position;
                if (center.X > 2000 || center.Y < -2000 || center.X > 2000 || center.X < -2000)
                {
                    rigidBodies.GetEntity(idx).Destroy();
                }
            }
        }
    }
}
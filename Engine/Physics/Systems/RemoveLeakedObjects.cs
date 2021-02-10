using Engine.Physics.Components;
using Engine.Physics.Components.RigidBody;
using Leopotam.Ecs;

namespace Engine.Physics.Systems
{
    public class RemoveLeakedObjects : IEcsRunSystem
    {
        private EcsFilter<RigidBody, BoundingBox> bodies = null;

        public void Run()
        {
            var boundaries = new BoundingBox {Min = new Vector2(-200), Max = new Vector2(200)};

            foreach (var idx in bodies)
            {
                if (!BoundingBox.Intersects(ref bodies.Get2(idx), ref boundaries))
                {
                    bodies.GetEntity(idx).Destroy();
                }
            }
        }

    }
}

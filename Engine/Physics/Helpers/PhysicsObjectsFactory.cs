using Engine.Physics.Components;
using Engine.Physics.Components.Shapes;
using Leopotam.EcsLite;

namespace Engine.Physics.Helpers
{
    public class PhysicsObjectsFactory
    {
        protected SharedData sharedData;
        protected EcsWorld world;

        public PhysicsObjectsFactory(SharedData sharedData, EcsWorld world)
        {
            this.sharedData = sharedData;
            this.world = world;
        }

        public int CreateCircle(Vector2 center, float radius, float mass, float restitution, bool locked)
        {
            var e = world.NewEntity();

            sharedData.rigidBodies.Add(e) = new RigidBody(ColliderType.Circle, mass, restitution, locked);
            sharedData.poses.Add(e).Position = center;
            sharedData.velocities.Add(e);
            sharedData.circleShapes.Add(e).Radius = radius;
            sharedData.bboxes.Add(e);

            return e;
        }

        public int CreateBox(Vector2 center, float width, float height, float mass, float restitution, bool locked)
        {
            var e = world.NewEntity();

            sharedData.rigidBodies.Add(e) = new RigidBody(ColliderType.Box, mass, restitution, locked);
            sharedData.poses.Add(e).Position = center;
            sharedData.velocities.Add(e);
            sharedData.boxShapes.Add(e) = new Box(width, height);
            sharedData.bboxes.Add(e);

            return e;
        }

    }
}

using Engine.Physics.Components;
using Engine.Physics.Components.Shapes;
using Leopotam.Ecs;

namespace Engine.Physics.Helpers
{
    public static class PhysicsObjectsFactory
    {
        public static EcsEntity CreateCircle(Vector2 center, float radius, float mass, float restitution, bool locked, EcsWorld world)
        {
            var e = world.NewEntity();
            e
                .Replace(new RigidBody(ColliderType.Circle, mass, restitution, locked))
                .Replace(new Pose {Position = center})
                .Replace(new Velocity())
                .Replace(new Circle {Radius = radius})
                .Replace(new BoundingBox());

            return e;
        }

        public static EcsEntity CreateBox(Vector2 center, float width, float height, float mass, float restitution, bool locked, EcsWorld world)
        {
            var e = world.NewEntity();
            e
                .Replace(new RigidBody(ColliderType.Box, mass, restitution, locked))
                .Replace(new Pose {Position = center})
                .Replace(new Velocity())
                .Replace(new Box(width, height))
                .Replace(new BoundingBox());

            return e;
        }

    }
}

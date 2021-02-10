using Engine.Physics.Components;
using Engine.Physics.Components.Shapes;
using Engine.Physics.Components.RigidBody;
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

        public static EcsEntity CreateAttractor(Vector2 center, bool locked, EcsWorld world)
        {
            return CreateCircle(center, 0.7f, 20f, 0.95f, locked, world)
                .Replace(new Attractor());
        }

        public static EcsEntity CreateSmallBall(Vector2 center, EcsWorld world)
        {
            return CreateCircle(center, 0.15f, 1f, 0.7f, false, world);
        }

        public static EcsEntity CreateMediumBall(Vector2 center, EcsWorld world)
        {
            return CreateCircle(center, 0.5f, 2f, 0.95f, false, world);
        }

        public static EcsEntity CreateWall(Vector2 center, float width, float height, EcsWorld world)
        {
            return CreateBox(center, width, height, 1000f, 0.95f, true, world);
        }

        public static EcsEntity CreateWall(Vector2 min, Vector2 max, EcsWorld world)
        {
            return CreateWall((min + max) / 2, max.X - min.X, max.Y - min.Y, world);
        }

    }
}

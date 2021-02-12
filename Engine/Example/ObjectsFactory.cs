using Engine.Example.Components;
using Engine.Physics;
using Engine.Physics.Helpers;
using Leopotam.Ecs;

namespace Engine.Example
{
    public static class ObjectsFactory
    {
        public static EcsEntity CreateSmallBall(Vector2 center, EcsWorld world)
        {
            return PhysicsObjectsFactory.CreateCircle(center, 0.15f, 1f, 0.7f, false, world);
        }

        public static EcsEntity CreateMediumBall(Vector2 center, EcsWorld world)
        {
            return PhysicsObjectsFactory.CreateCircle(center, 0.3f, 2f, 0.95f, false, world);
        }

        public static EcsEntity CreateWall(Vector2 center, float width, float height, EcsWorld world)
        {
            return PhysicsObjectsFactory.CreateBox(center, width, height, 1000f, 0.95f, true, world);
        }

        public static EcsEntity CreateWall(Vector2 min, Vector2 max, EcsWorld world)
        {
            return CreateWall((min + max) / 2, max.X - min.X, max.Y - min.Y, world);
        }

        public static EcsEntity CreateAttractor(Vector2 center, bool locked, EcsWorld world)
        {
            return PhysicsObjectsFactory.CreateCircle(center, 0.7f, 20f, 0.95f, locked, world)
                .Replace(new Attractor());
        }
    }
}

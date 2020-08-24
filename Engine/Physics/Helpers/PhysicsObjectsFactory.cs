using Engine.Physics.Components;
using Engine.Physics.Components.Colliders;
using Engine.Physics.Components.RigidBody;
using Leopotam.Ecs;

namespace Engine.Physics.Helpers
{
    public static class PhysicsObjectsFactory
    {
        /// <summary>
        /// All units are in pixels 
        /// </summary>
        /// <param name="center"></param>
        /// <param name="world"></param>
        public static EcsEntity CreateSmallBall(Vector2 center, EcsWorld world, float pixelsToMeters)
        {
            return CreateCircle(
                center * pixelsToMeters, 
                10 * pixelsToMeters, 
                1f, 0.7f, false, world);
        }

        /// <summary>
        /// All units are in pixels
        /// </summary>
        /// <param name="center"></param>
        /// <param name="world"></param>
        public static EcsEntity CreateMedBall(Vector2 center, EcsWorld world, float pixelsToMeters)
        {
            return CreateCircle(
                center * pixelsToMeters, 
                30 * pixelsToMeters, 
                2f, 0.95f, false, world);
        }
        
        /// <summary>
        /// All units are in pixels
        /// </summary>
        /// <param name="center"></param>
        /// <param name="world"></param>
        public static EcsEntity CreateAttractor(Vector2 center, EcsWorld world, float pixelsToMeters)
        {
            return CreateCircle(
                    center * pixelsToMeters, 
                    50 * pixelsToMeters, 
                    1.5f, 0.95f, true, world)
                .Replace(new Attractor());
        }

        /// <summary>
        /// All units are in pixels
        /// </summary>
        /// <param name="center"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="world"></param>
        public static EcsEntity CreateWall(Vector2 center, float width, float height, EcsWorld world, float pixelsToMeters)
        {
            return CreateBox(
                center * pixelsToMeters, 
                width * pixelsToMeters, 
                height * pixelsToMeters, 
                1000f, 0.95f, true, world);
        }
        
        /// <summary>
        /// All units are in pixels
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="world"></param>
        public static EcsEntity CreateWall(Vector2 min, Vector2 max, EcsWorld world, float pixelsToMeters)
        {
            return CreateWall(
                (min + max) / 2, 
                max.X - min.X, 
                max.Y - min.Y,
                world, pixelsToMeters);
        }

        /// <summary>
        /// All units are in meters
        /// </summary>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        /// <param name="mass"></param>
        /// <param name="restitution"></param>
        /// <param name="locked"></param>
        /// <param name="world"></param>
        /// <returns></returns>
        public static EcsEntity CreateCircle(
            Vector2 center, float radius, float mass, float restitution, bool locked, EcsWorld world)
        {
            var e = world.NewEntity();
            e
                .Replace(new RigidBody(ColliderType.Circle, mass, restitution, locked))
                .Replace(new Transform {Position = center})
                .Replace(new Velocity())
                .Replace(new Circle {Radius = radius})
                .Replace(new AABB());
            
            return e;
        }

        /// <summary>
        /// All units are in meters
        /// </summary>
        /// <param name="center"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="mass"></param>
        /// <param name="restitution"></param>
        /// <param name="locked"></param>
        /// <param name="world"></param>
        /// <returns></returns>
        public static EcsEntity CreateBox(
            Vector2 center, float width, float height, float mass, float restitution, bool locked, EcsWorld world)
        {
            var e = world.NewEntity();
            e
                .Replace(new RigidBody(ColliderType.Box, mass, restitution, locked))
                .Replace(new Transform {Position = center})
                .Replace(new Velocity())
                .Replace(new Box(width, height))
                .Replace(new AABB());
            
            return e;
        }
    }
}
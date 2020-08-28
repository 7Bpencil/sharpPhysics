﻿using Engine.Physics.Components;
using Engine.Physics.Components.Colliders;
using Engine.Physics.Components.RigidBody;
using Leopotam.Ecs;

namespace Engine.Physics.Helpers
{
    public class PhysicsObjectsFactory
    {
        private PhysicsSettings settings;

        public PhysicsObjectsFactory(PhysicsSettings physicsSettings)
        {
            settings = physicsSettings;
        }

        /// <summary>
        /// All units are in pixels 
        /// </summary>
        /// <param name="center"></param>
        /// <param name="world"></param>
        public EcsEntity CreateSmallBall(Vector2 center, EcsWorld world)
        {
            return CreateCircle(
                center * settings.PixelsToMeters, 
                10 * settings.PixelsToMeters, 
                1f, 0.7f, false, world);
        }

        /// <summary>
        /// All units are in pixels
        /// </summary>
        /// <param name="center"></param>
        /// <param name="world"></param>
        public EcsEntity CreateMedBall(Vector2 center, EcsWorld world)
        {
            return CreateCircle(
                center * settings.PixelsToMeters, 
                30 * settings.PixelsToMeters, 
                2f, 0.95f, false, world);
        }
        
        /// <summary>
        /// All units are in pixels
        /// </summary>
        /// <param name="center"></param>
        /// <param name="locked"></param>
        /// <param name="world"></param>
        /// <param name="pixelsToMeters"></param>
        /// <returns></returns>
        public EcsEntity CreateAttractor(Vector2 center, bool locked, EcsWorld world)
        {
            return CreateCircle(
                    center * settings.PixelsToMeters, 
                    40 * settings.PixelsToMeters, 
                    1.5f, 0.95f, locked, world)
                .Replace(new Attractor());
        }

        /// <summary>
        /// All units are in pixels
        /// </summary>
        /// <param name="center"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="world"></param>
        public EcsEntity CreateWall(Vector2 center, float width, float height, EcsWorld world)
        {
            return CreateBox(
                center * settings.PixelsToMeters, 
                width * settings.PixelsToMeters, 
                height * settings.PixelsToMeters, 
                1000f, 0.95f, true, world);
        }
        
        /// <summary>
        /// All units are in pixels
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="world"></param>
        public EcsEntity CreateWall(Vector2 min, Vector2 max, EcsWorld world)
        {
            return CreateWall(
                (min + max) / 2, 
                max.X - min.X, 
                max.Y - min.Y,
                world);
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
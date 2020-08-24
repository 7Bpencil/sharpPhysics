﻿using System.Runtime.CompilerServices;
using Engine.Physics.Components;
using Engine.Physics.Components.RigidBody;
using Leopotam.Ecs;

namespace Engine.Physics.Systems
{
    public class ApplyFriction : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter<RigidBody, Velocity> rigidBodies = null;
        private PhysicsSettings settings = null;
        
        public void Init() { }

        public void Run()
        {
            var friction = settings.Friction;
            foreach (var idx in rigidBodies)
            {
                ref var velocity = ref rigidBodies.Get2(idx);
                AddFriction(ref velocity.Value, friction);
            }
        }

        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        private void AddFriction(ref Vector2 velocity, float friction)
        {
            velocity -= Vector2.Normalize(velocity) * friction;
        }
    }
}
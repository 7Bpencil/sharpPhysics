﻿using Engine.Example.Components;
using Engine.Physics;
using Engine.Physics.Components;
using Leopotam.Ecs;

namespace Engine.Example.Systems
{
    public class ApplyGravity : IEcsRunSystem
    {
        private EcsFilter<RigidBody, Velocity>.Exclude<Attractor> bodies = null;

        private PhysicsSettings settings = null;

        public void Run()
        {
            var gravityDt = settings.Gravity * settings.dt;

            foreach (var idx in bodies)
            {
                ref var body = ref bodies.Get1(idx);
                if (body.Locked) continue;

                ref var velocity = ref bodies.Get2(idx).Linear;
                velocity += gravityDt;
            }
        }
    }
}
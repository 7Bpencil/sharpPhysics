﻿using System;
using Engine.Physics.Components;
using Engine.Physics.Components.RigidBody;
using Leopotam.Ecs;

namespace Engine.Physics.Systems
{
    public class PrintDebug : IEcsRunSystem
    {
        private EcsFilter<RigidBody, Transform, Velocity> rigidBodies = null;

        public void Run()
        {
            foreach (var idx in rigidBodies)
            {
                Console.WriteLine("body idx: " + idx + 
                                  ", position: " + rigidBodies.Get2(idx).Position + 
                                  ", velocity: " + rigidBodies.Get3(idx).Value);
            }
            Console.WriteLine();
        }
    }
}
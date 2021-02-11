using System;
using Engine.Physics.Components;
using Leopotam.Ecs;

namespace Engine.Physics.Systems
{
    public class PrintDebug : IEcsRunSystem
    {
        private EcsFilter<RigidBody, Pose, Velocity> bodies = null;

        public void Run()
        {
            foreach (var idx in bodies)
            {
                Console.WriteLine($"body idx: {idx.ToString()}, " +
                                  $"position: {bodies.Get2(idx).Position.ToString()}, " +
                                  $"velocity: {bodies.Get3(idx).Linear.ToString()}");
            }
            Console.WriteLine();
        }
    }
}

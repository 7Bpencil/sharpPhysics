using System;
using Engine.Example.Components;
using Engine.Physics;
using Engine.Physics.Components;
using Leopotam.Ecs;

namespace Engine.Example.Systems
{
    public class InputSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter<Player, Velocity> players = null;
        private KeyState keys = null;

        private float moveSpeed;
        private float d;
        
        public void Init()
        {
            d = (float) (1 / Math.Sqrt(2));
            moveSpeed = 1f;
        }
        
        public void Run()
        {
            var delta = Vector2.Zero;
            
            if (keys.W) delta.Y -= d;
            if (keys.A) delta.X -= d;
            if (keys.S) delta.Y += d;
            if (keys.D) delta.X += d;

            delta *= moveSpeed;
            
            foreach (var idx in players)
            {
                ref var velocity = ref players.Get2(idx);
                velocity.Value += delta;
            }
        }
    }
}
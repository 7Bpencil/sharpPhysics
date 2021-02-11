using Engine.Example.Components;
using Engine.Physics;
using Engine.Physics.Components;
using Leopotam.Ecs;

namespace Engine.Example.Systems
{
    public class InputSystem : IEcsRunSystem
    {
        private EcsFilter<Player, Velocity> players = null;
        private KeyState keys = null;
        private PhysicsSettings settings = null;

        public void Run()
        {
            var moveSpeed = 60f;  // hardcoded
            var delta = Vector2.Zero;

            if (keys.W) delta.Y += 1;
            if (keys.A) delta.X -= 1;
            if (keys.S) delta.Y -= 1;
            if (keys.D) delta.X += 1;

            delta = delta.Normalized() * moveSpeed * settings.dt;

            foreach (var idx in players)
            {
                ref var velocity = ref players.Get2(idx);
                velocity.Linear += delta;
            }
        }
    }
}

using Engine.Game.Components;
using Leopotam.EcsLite;
using Engine.Physics;
using Engine.Physics.Components;

namespace Engine.Game.Systems
{
    public class InputSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter players;

        public void Init(EcsSystems systems)
        {
            var world = systems.GetWorld();

            players = world.Filter<Player>().Inc<Velocity>().End();
        }

        public void Run(EcsSystems systems)
        {
            var movementSpeed = 60f;  // hardcoded
            var sharedData = systems.GetShared<SharedData>();
            var velocities = sharedData.velocities;

            var velocityDelta = GetVelocityDelta(sharedData.keys, movementSpeed, sharedData.PhysicsSystemData.dt);
            foreach (var entity in players)
            {
                ref var velocity = ref velocities.Get(entity);
                velocity.Linear += velocityDelta;
            }
        }

        private static Vector2 GetVelocityDelta(KeyState keys, float movementSpeed, float dt)
        {
            var velocityDelta = Vector2.Zero;

            if (keys.W) velocityDelta.Y += 1;
            if (keys.A) velocityDelta.X -= 1;
            if (keys.S) velocityDelta.Y -= 1;
            if (keys.D) velocityDelta.X += 1;

            return velocityDelta.Normalized() * movementSpeed * dt;
        }

    }
}

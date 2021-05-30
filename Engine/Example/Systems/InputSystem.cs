using Engine.Example.Components;
using Engine.Physics;
using Engine.Physics.Components;
using Leopotam.EcsLite;

namespace Engine.Example.Systems
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
            var velocityDelta = GetVelocityDelta(sharedData.keys, movementSpeed, sharedData.PhysicsSystemData.dt);
            UpdatePlayersVelocities(velocityDelta, sharedData.velocities);
        }

        private Vector2 GetVelocityDelta(KeyState keys, float movementSpeed, float dt)
        {
            var velocityDelta = Vector2.Zero;

            if (keys.W) velocityDelta.Y += 1;
            if (keys.A) velocityDelta.X -= 1;
            if (keys.S) velocityDelta.Y -= 1;
            if (keys.D) velocityDelta.X += 1;

            return velocityDelta.Normalized() * movementSpeed * dt;
        }

        private void UpdatePlayersVelocities(Vector2 delta, EcsPool<Velocity> velocities)
        {
            foreach (var idx in players)
            {
                ref var velocity = ref velocities.Get(idx);
                velocity.Linear += delta;
            }
        }

    }
}

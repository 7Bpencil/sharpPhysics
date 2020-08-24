using Leopotam.Ecs;

namespace Engine.Physics.Components
{
    public struct CollisionPair
    {
        public EcsEntity BodyA;
        public EcsEntity BodyB;
    }
}
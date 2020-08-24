using Leopotam.Ecs;

namespace Engine.Physics.Components
{
    public struct Manifold
    {
        public EcsEntity BodyA;
        public EcsEntity BodyB;
        
        public float Penetration;
        public Vector2 Normal;
    }
}
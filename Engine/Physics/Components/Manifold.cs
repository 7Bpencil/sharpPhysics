namespace Engine.Physics.Components
{
    public struct Manifold
    {
        public int BodyA;
        public int BodyB;
        public float Penetration;
        public Vector2 Normal;
    }
}

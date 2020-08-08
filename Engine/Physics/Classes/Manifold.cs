namespace Engine.Physics.Classes
{
    public class Manifold
    {
        public PhysicsObject A;
        public PhysicsObject B;
        public float Penetration;
        public Vector2 Normal;
    }
}
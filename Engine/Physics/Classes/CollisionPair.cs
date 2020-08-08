namespace Engine.Physics.Classes
{
    public class CollisionPair
    {
        public readonly PhysicsObject A;
        public readonly PhysicsObject B;

        public CollisionPair(PhysicsObject A, PhysicsObject B)
        {
            this.A = A;
            this.B = B;
        }

        public static bool operator ==(CollisionPair left, CollisionPair right)
        {
            return left.A == right.A && left.B == right.B || left.A == right.B && left.B == right.A;
        }

        public static bool operator !=(CollisionPair left, CollisionPair right)
        {
            return !(left == right);
        }
    }
}

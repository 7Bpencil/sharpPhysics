namespace Engine.Physics
{
    public class PhysicsSettings
    {
        /// velocity is meters/sec
        /// length is meters
        /// mass is kg
        /// 1m = 64px
        /// Maybe it's not really important in 2D but it's less mess

        public float GravityScale = 0.1f;
        public float Friction = 0.1f;
        public int IterationsAmount = 10;
        public float MetersToPixels = 64f;
        public float PixelsToMeters;
        public float VelocityCoefficient;
        public float dt;

        public Vector2 Gravity;
        public float AccuracyTolerance;

        public PhysicsSettings()
        {
            dt = 1 / (60f * IterationsAmount);
            VelocityCoefficient = 2f / (IterationsAmount + 1);
            PixelsToMeters = 1 / MetersToPixels;
            AccuracyTolerance = 1 / (2 * MetersToPixels); // ~0.5px
            Gravity = new Vector2(0, 9.8f * GravityScale);
        }
    }
}

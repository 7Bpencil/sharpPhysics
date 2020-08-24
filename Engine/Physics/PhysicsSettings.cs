namespace Engine.Physics
{
    public class PhysicsSettings
    {
        /// velocity is meters/sec
        /// length is meters
        /// mass is kg
        /// 1m = 64px
        /// Maybe it's not very useful in 2D but it's less mess
    
        public float GravityScale = 0.1f;
        public float Friction = 0.2f;
        public float dt = 1 / (60f * 4f);
        public float MetersToPixels = 64f;
        public float PixelsToMeters;

        public Vector2 Gravity;
        public float AccuracyTolerance;

        public PhysicsSettings()
        {
            PixelsToMeters = 1 / MetersToPixels;
            AccuracyTolerance = 1 / (2 * MetersToPixels); // ~0.5px
            Gravity = new Vector2(0, 9.8f * GravityScale);
        }
    }
}
namespace Engine.Physics
{
    public class PhysicsSettings
    {
        /// length is meters
        /// time is seconds
        /// velocity is meters/seconds
        /// mass is kg
        /// Maybe it's not really important in 2D but it's a less mess

        public float dt;
        public Vector2 Gravity;

        public PhysicsSettings()
        {
            dt = 1 / 60f;
            Gravity = new Vector2(0, 9.80665f);  // gravity points up because Y axis points down in GDI+
        }
    }
}

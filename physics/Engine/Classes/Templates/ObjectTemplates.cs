namespace physics.Engine.Classes.ObjectTemplates
{
    public static class ObjectTemplates
    {
        public static PhysicsObject CreateSmallBall(Vector2 origin)
        {
            return PhysicsSystem.CreateStaticCircle(origin, 10, .7F, false);
        }

        public static PhysicsObject CreateSmallBall_Magnet(Vector2 origin)
        {
            var oPhysicsObject = PhysicsSystem.CreateStaticCircle(origin, 5, .95F, false);
            PhysicsSystem.ListGravityObjects.Add(oPhysicsObject);
            return oPhysicsObject;
        }

        public static PhysicsObject CreateMedBall(Vector2 origin)
        {
            return PhysicsSystem.CreateStaticCircle(origin, 30, .95F, false);
        }

        public static PhysicsObject CreateWater(Vector2 origin)
        {
            return PhysicsSystem.CreateStaticCircle(origin, 5, .99F, false);
        }

        public static PhysicsObject CreateAttractor(Vector2 origin)
        {
            var oPhysicsObject = PhysicsSystem.CreateStaticCircle(origin, 50, .95F, true);
            PhysicsSystem.ListGravityObjects.Add(oPhysicsObject);
            return oPhysicsObject;
        }

        public static PhysicsObject CreateWall(Vector2 min, Vector2 max)
        {
            return PhysicsSystem.CreateStaticBox(min, max, true, 1000000);
        }
    }
}
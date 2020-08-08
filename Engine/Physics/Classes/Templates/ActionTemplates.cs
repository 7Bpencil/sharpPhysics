namespace Engine.Physics.Classes.Templates
{
    public static class ActionTemplates
    {
        public static void launch(PhysicsSystem physSystem, PhysicsObject physObj, Vector2 StartPointF, Vector2 EndPointF)
        {
            physSystem.ActivateAtPoint(StartPointF);
            var delta = new Vector2(EndPointF.X - StartPointF.X, EndPointF.Y - StartPointF.Y);
            physSystem.AddVelocityToActive(-delta);
        }

        public static void PopAndMultiply(PhysicsSystem physSystem)
        {
            foreach (PhysicsObject obj in physSystem.GetMoveableObjects())
            {
                physSystem.ActivateAtPoint(obj.Center);
                var velocity = obj.Velocity;
                var origin = obj.Center;
                physSystem.RemoveActiveObject();
                physSystem.SetVelocity(ObjectTemplates.CreateSmallBall(origin), velocity);
                physSystem.SetVelocity(ObjectTemplates.CreateSmallBall(origin), velocity);
            }
        }
    }
}
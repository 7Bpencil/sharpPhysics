using Engine.Physics;
using Engine.Physics.Helpers;
using Leopotam.EcsLite;

namespace Engine.Example
{
    public class ObjectsFactory : PhysicsObjectsFactory
    {
        public ObjectsFactory(SharedData sharedData, EcsWorld world) : base(sharedData, world) { }

        public int CreateSmallBall(Vector2 center)
        {
            return CreateCircle(center, 0.15f, 1f, 0.7f, false);
        }

        public int CreateMediumBall(Vector2 center)
        {
            return CreateCircle(center, 0.3f, 2f, 0.95f, false);
        }

        public int CreateWall(Vector2 center, float width, float height)
        {
            return CreateBox(center, width, height, 1000f, 0.95f, true);
        }

        public int CreateWall(Vector2 min, Vector2 max)
        {
            return CreateWall((min + max) / 2, max.X - min.X, max.Y - min.Y);
        }

        public int CreateAttractor(Vector2 center, bool locked)
        {
            var e = CreateCircle(center, 0.7f, 20f, 0.95f, locked);
            sharedData.attractors.Add(e);

            return e;
        }

        public int MarkAsPlayer(int e)
        {
            sharedData.players.Add(e);
            return e;
        }

    }
}

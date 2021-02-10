namespace Engine.Physics.Components.Colliders
{
    public struct Box
    {
        public Vector2 HalfSize;

        public Box(float width, float height) : this()
        {
            HalfSize = new Vector2(width, height) * 0.5f;
        }
    }
}

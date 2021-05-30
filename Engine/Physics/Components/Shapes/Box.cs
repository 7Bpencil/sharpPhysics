namespace Engine.Physics.Components.Shapes
{
    public struct Box
    {
        public Vector2 HalfSize;

        public Box(float width, float height)
        {
            HalfSize = new Vector2(width, height) * 0.5f;
        }
    }
}

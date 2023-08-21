using SnakeGameV3.Interfaces;

namespace SnakeGameV3
{
    internal class ColliderConfig : IScalable
    {
        public ColliderConfig(ColliderType colliderType, float scale)
        {
            Type = colliderType;
            Scale = scale;
        }

        public ColliderType Type { get; }

        public float Scale { get; }
    }
}

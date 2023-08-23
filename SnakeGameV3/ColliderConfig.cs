namespace SnakeGameV3
{
    internal record ColliderConfig : Component
    {
        public ColliderConfig(ColliderType colliderType)
        {
            Type = colliderType;
        }

        public ColliderType Type { get; init; }
    }
}

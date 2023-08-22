namespace SnakeGameV3
{
    internal record ColliderConfig : ComponentConfig
    {
        public ColliderConfig(ColliderType colliderType)
        {
            Type = colliderType;
        }

        public ColliderType Type { get; }
    }
}

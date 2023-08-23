namespace SnakeGameV3.Texturing
{
    internal record TextureConfig : Component
    {
        public TextureConfig(TextureName name, ConsoleColor color)
        {
            Name = name;
            Color = color;
        }

        public TextureName Name { get; init; }

        public ConsoleColor Color { get; init; }
    }
}

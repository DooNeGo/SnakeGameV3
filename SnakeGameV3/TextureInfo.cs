using SnakeGameV3.Interfaces;

namespace SnakeGameV3
{
    internal class TextureInfo : IScalable
    {
        public TextureInfo(TextureName name, float scale, ConsoleColor color)
        {
            Name = name;
            Scale = scale;
            Color = color;
        }

        public TextureName Name { get; }

        public ConsoleColor Color { get; }

        public float Scale { get; }
    }
}

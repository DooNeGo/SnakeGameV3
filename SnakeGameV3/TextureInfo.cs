using SnakeGameV3.Interfaces;

namespace SnakeGameV3
{
    internal class TextureInfo : IScalable
    {
        public TextureInfo(TextureName name, float scale)
        {
            Name = name;
            Scale = scale;
        }

        public TextureName Name { get; }

        public float Scale { get; }
    }
}

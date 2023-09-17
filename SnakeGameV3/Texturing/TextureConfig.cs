using SnakeGameV3.Interfaces;

namespace SnakeGameV3.Texturing
{
    internal class TextureConfig : Component
    {
        public TextureConfig(TextureName name, ConsoleColor color, IReadOnlyGameObject parent) : base(parent)
        {
            Name = name;
            Color = color;
        }

        public TextureName Name { get; }

        public ConsoleColor Color { get; }

        public float Scale => _parent.Scale;

        public static bool operator ==(TextureConfig left, TextureConfig right)
        {
            if (left.Color == right.Color
                && left.Name == right.Name
                && left.Scale == right.Scale)
                return true;

            return false;
        }

        public static bool operator !=(TextureConfig left, TextureConfig right)
        {
            if (left == right)
                return false;

            return true;
        }

        public override bool Equals(object? obj)
        {
            return obj is TextureConfig textureConfig
                   && textureConfig == this;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Color, Scale);
        }
    }
}

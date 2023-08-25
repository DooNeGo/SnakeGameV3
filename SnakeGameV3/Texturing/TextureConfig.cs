namespace SnakeGameV3.Texturing
{
    internal class TextureConfig : Component
    {
        public TextureConfig(TextureName name, ConsoleColor color)
        {
            Name = name;
            Color = color;
        }

        public TextureName Name { get; }

        public ConsoleColor Color { get; }

        public static bool operator ==(TextureConfig left, TextureConfig right)
        {
            if (left.Color == right.Color
                && left.Name == right.Name)
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
            return HashCode.Combine(Name, Color);
        }
    }
}

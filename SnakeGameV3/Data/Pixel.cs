using System.Diagnostics.CodeAnalysis;

namespace SnakeGameV3.Data
{
    internal readonly struct Pixel
    {
        public Pixel(ConsoleColor color)
        {
            Color = color;
        }

        public readonly char Model { get; } = '█';

        public ConsoleColor Color { get; }

        public override bool Equals([NotNullWhen(true)] object? obj) => obj is Pixel pixel && pixel.Color == Color;

        public override int GetHashCode() => Color.GetHashCode();

        public static bool operator ==(Pixel left, Pixel right) => left.Color == right.Color;

        public static bool operator !=(Pixel left, Pixel right) => left.Color != right.Color;
    }
}
using System.Numerics;

namespace SnakeGameV3
{
    internal static class Vector2Extension
    {
        public static bool EqualsRound(this Vector2 left, Vector2 right)
        {
            return MathF.Round(left.X) == MathF.Round(right.X)
                && MathF.Round(left.Y) == MathF.Round(right.Y);
        }
    }
}
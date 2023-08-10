using System.Numerics;
using static SnakeGameV3.Config;

namespace SnakeGameV3
{
    internal static class Vector2Extension
    {
        public static Vector2 GetAbsolutePosition(this Vector2 relativePosition) => new(MathF.Round(relativePosition.X * GridCellWidth),
                                                                                        MathF.Round(relativePosition.Y * GridCellHeight));

        public static bool EqualsRound(this Vector2 left, Vector2 right) => MathF.Round(left.X) == MathF.Round(right.X)
                                                                            && MathF.Round(left.Y) == MathF.Round(right.Y);
    }
}
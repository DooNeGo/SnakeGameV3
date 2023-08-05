using static SnakeGameV3.Config;
using System.Numerics;

namespace SnakeGameV3
{
    internal static class Vector2Extension
    {
        public static Vector2 GetAbsolutePosition(this Vector2 relativePosition) => new(MathF.Round(relativePosition.X * GridCellWidth),
                                                                                        MathF.Round(relativePosition.Y * GridCellHeight));
    }
}
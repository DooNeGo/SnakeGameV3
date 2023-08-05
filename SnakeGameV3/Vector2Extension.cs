using static SnakeGameV3.Config;
using System.Numerics;

namespace SnakeGameV3
{
    internal static class Vector2Extension
    {
        public static Vector2 GetNormalPosition(this Vector2 position) => new(MathF.Round(position.X * GridCellWidth), MathF.Round(position.Y * GridCellHeight));
    }
}
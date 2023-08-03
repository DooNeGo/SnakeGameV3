using static SnakeGameV3.GameConstants;
using System.Numerics;

namespace SnakeGameV3
{
    internal static class Vector2Extension
    {
        public static Vector2 GetNormalCoord(this Vector2 vector) => new(MathF.Round(vector.X * GridCellWidth), MathF.Round(vector.Y * GridCellHeight));
    }
}
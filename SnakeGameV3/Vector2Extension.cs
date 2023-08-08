using SnakeGameV3.Model;
using System.Numerics;
using static SnakeGameV3.Config;

namespace SnakeGameV3
{
    internal static class Vector2Extension
    {
        public static Vector2 GetAbsolutePosition(this Vector2 relativePosition) => new(MathF.Round(relativePosition.X * GridCellWidth),
                                                                                        MathF.Round(relativePosition.Y * GridCellHeight));

        public static Vector2 GetProjectionOnTheGrid(this Vector2 position, Grid grid)
        {
            var projection = new Vector2(position.X % grid.Size.Width, position.Y % grid.Size.Height);

            if (position.X < 0)
            {
                projection += new Vector2(grid.Size.Width - 1, 0);
            }
            if (position.Y < 0)
            {
                projection += new Vector2(0, grid.Size.Height - 1);
            }

            return projection;
        }
    }
}
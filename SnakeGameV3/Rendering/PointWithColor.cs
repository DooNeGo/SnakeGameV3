using SnakeGameV3.Interfaces;
using System.Drawing;

namespace SnakeGameV3.Rendering
{
    internal readonly struct PointWithColor
    {
        public PointWithColor(Point point, ConsoleColor color)
        {
            Point = point;
            Color = color;
        }

        public Point Point { get; }

        public ConsoleColor Color { get; }
    }
}
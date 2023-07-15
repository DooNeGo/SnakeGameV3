using System.Drawing;

namespace SnakeGameV3.Rendering
{
    internal class ConsoleObject
    {
        public ConsoleObject(Point coordinates, ConsoleColor[,] model)
        {
            Coordinates = coordinates;
            Model = model;
        }

        public Point Coordinates { get; }

        public ConsoleColor[,] Model { get; }
    }
}

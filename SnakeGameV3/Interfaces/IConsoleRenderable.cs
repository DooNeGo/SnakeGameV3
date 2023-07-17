using System.Drawing;

namespace SnakeGameV3.Interfaces
{
    internal interface IConsoleRenderable
    {
        public Point Point { get; }

        public ConsoleColor[,] Model { get; }
    }
}
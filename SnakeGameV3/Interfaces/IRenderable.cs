using SnakeGameV3.Data;
using System.Drawing;

namespace SnakeGameV3.Interfaces
{
    internal interface IRenderable
    {
        public Point Coordinates { get; }

        public Pixel[,] Model { get; }
    }
}
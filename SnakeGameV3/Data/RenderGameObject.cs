using SnakeGameV3.Interfaces;
using System.Drawing;

namespace SnakeGameV3.Data
{
    internal class RenderGameObject : IRenderable
    {
        public RenderGameObject(Point coordinates, Pixel[,] model)
        {
            Coordinates = coordinates;
            Model = model;
        }

        public Point Coordinates { get; }

        public Pixel[,] Model { get; }
    }
}

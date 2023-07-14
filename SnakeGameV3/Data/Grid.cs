using System.Drawing;

namespace SnakeGameV3.Data
{
    internal class Grid
    {
        public Grid(int screenHeight, int screenWidth, int cellSize)
        {
            CellSize = cellSize;
            Height = screenHeight / cellSize;
            Width = screenWidth / cellSize;
        }

        public int Height { get; }
        public int Width { get; }

        public int CellSize { get; }
    }
}

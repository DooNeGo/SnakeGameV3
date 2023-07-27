using System.Drawing;

namespace SnakeGameV3.Rendering
{
    internal class ShapeFactory
    {
        private readonly Size _gridCellSize;

        public ShapeFactory(Size cellSize)
        {
            _gridCellSize = cellSize;
        }

        public ConsoleColor[,] GetSquare(ConsoleColor color)
        {
            ConsoleColor[,] shape = new ConsoleColor[_gridCellSize.Height, _gridCellSize.Width];

            for (var y = 0; y < shape.GetLength(0); y++)
                for (var x = 0; x < shape.GetLength(1); x++)
                    shape[y, x] = color;

            return shape;
        }
    }
}

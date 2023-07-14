using SnakeGameV3.Data;

namespace SnakeGameV3
{
    internal class ShapeFactory
    {
        public ShapeFactory(int gridCellSize)
        {
            _gridCellSize = gridCellSize;
        }

        private int _gridCellSize;

        public Pixel[,] GetSquare(ConsoleColor color)
        {
            Pixel[,] newSquare = new Pixel[_gridCellSize, _gridCellSize];

            for (int i = 0; i < _gridCellSize; i++)
                for (int j = 0; j < _gridCellSize; j++)
                    newSquare[i, j] = new Pixel(color);

            return newSquare;
        }
    }
}

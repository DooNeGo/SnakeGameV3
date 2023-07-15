namespace SnakeGameV3
{
    internal class ShapeFactory
    {
        public ShapeFactory(int gridCellSize)
        {
            _gridCellSize = gridCellSize;
        }

        private readonly int _gridCellSize;

        public ConsoleColor[,] GetSquare(ConsoleColor color)
        {
            ConsoleColor[,] shape = new ConsoleColor[_gridCellSize, _gridCellSize];

            for (var y = 0; y < _gridCellSize; y++)
                for (var x = 0; x < _gridCellSize; x++)
                    shape[y, x] = color;

            return shape;
        }

        public ConsoleColor[,] GetCircle(ConsoleColor color)
        {
            ConsoleColor[,] shape = new ConsoleColor[_gridCellSize, _gridCellSize];

            for (var y = 0; y < _gridCellSize / 2; y++)
            {
                for (var x = _gridCellSize / 2; x >= Math.Round((_gridCellSize - 1 - y * 2) / 2.0); x--)
                {
                    shape[y, x] = color;
                    shape[y, _gridCellSize - 1 - x] = color;
                    //shape[_gridCellSize - y, x] = color;
                    //shape[_gridCellSize - 1 - y, _gridCellSize - 1 - x] = color;
                }
            }

            return shape;
        }
    }
}

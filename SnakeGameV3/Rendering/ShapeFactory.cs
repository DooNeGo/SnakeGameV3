namespace SnakeGameV3.Rendering
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
    }
}

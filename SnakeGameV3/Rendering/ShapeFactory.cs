namespace SnakeGameV3.Rendering
{
    internal class ShapeFactory
    {
        private readonly int _gridCellSize;

        public ShapeFactory(int gridCellSize)
        {
            _gridCellSize = gridCellSize;
        }

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

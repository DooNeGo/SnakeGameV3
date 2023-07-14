using System.Drawing;

namespace SnakeGameV3.Data
{
    internal class ConsoleFrame
    {
        public ConsoleFrame(Grid grid, int screenHeight, int screenWidth, ConsoleColor backGroundColor)
        {
            _grid = grid;
            _frame = new Pixel[screenHeight, screenWidth];
            _backgroundColor = backGroundColor;

            InitializeFrame();
        }

        private readonly Grid _grid;

        private readonly ConsoleColor _backgroundColor;

        private readonly Pixel[,] _frame;

        public int Height => _frame.GetLength(0);

        public int Width => _frame.GetLength(1);

        public Pixel GetPixel(int x, int y) => _frame[y, x];

        private void InitializeFrame()
        {
            for (int y = 0; y < _frame.GetLength(0); y++)
                for (int x = 0; x < _frame.GetLength(1); x++)
                    _frame[y, x] = new Pixel(_backgroundColor);
        }

        public void Clear()
        {
            for (int y = 0; y < _frame.GetLength(0); y++)
                for (int x = 0; x < _frame.GetLength(1); x++)
                    if (_frame[y, x].Color != _backgroundColor)
                        _frame[y, x] = new Pixel(_backgroundColor);
        }

        public void Add(Point gridCoord, Pixel[,] gameObject)
        {
            if (gridCoord.X >= _grid.Width
                || gridCoord.X < 0
                || gridCoord.Y >= _grid.Height
                || gridCoord.Y < 0)
                throw new Exception();

            for (int y = 0; y < gameObject.GetLength(1); y++)
                for (int x = 0; x < gameObject.GetLength(0); x++)
                    _frame[gridCoord.Y * _grid.CellSize + y, gridCoord.X * _grid.CellSize + x] = gameObject[y ,x];
        }
    }
}

using SnakeGameV3.Data;
using System.Drawing;

namespace SnakeGameV3.Rendering
{
    internal class ConsoleFrame
    {
        public ConsoleFrame(Grid grid, int screenHeight, int screenWidth, ConsoleColor backGroundColor)
        {
            _grid = grid;
            _frame = new ConsoleColor[screenHeight, screenWidth];
            _backgroundColor = backGroundColor;

            InitializeFrame();
        }

        private readonly Grid _grid;

        private readonly ConsoleColor _backgroundColor;

        private readonly ConsoleColor[,] _frame;

        public int Height => _frame.GetLength(0);

        public int Width => _frame.GetLength(1);

        public ConsoleColor GetPixel(int x, int y) => _frame[y, x];

        private void InitializeFrame()
        {
            for (var y = 0; y < Height; y++)
                for (var x = 0; x < Width; x++)
                    _frame[y, x] = _backgroundColor;
        }

        public void Clear()
        {
            for (var y = 0; y < Height; y++)
                for (var x = 0; x < Width; x++)
                    if (_frame[y, x] != _backgroundColor)
                        _frame[y, x] = _backgroundColor;
        }

        public void Add(Point coordinates, ConsoleColor[,] model)
        {
            if (coordinates.X >= _grid.Size.Width
                || coordinates.X < 0
                || coordinates.Y >= _grid.Size.Height
                || coordinates.Y < 0)
                throw new Exception();

            for (var y = 0; y < model.GetLength(0); y++)
                for (var x = 0; x < model.GetLength(1); x++)
                    _frame[coordinates.Y * _grid.CellSize + y, coordinates.X * _grid.CellSize + x] = model[y, x];
        }
    }
}
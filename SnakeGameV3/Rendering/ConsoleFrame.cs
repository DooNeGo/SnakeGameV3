using SnakeGameV3.Data;
using System.Drawing;

namespace SnakeGameV3.Rendering
{
    internal class ConsoleFrame
    {
        private readonly Grid _grid;

        private readonly ConsoleColor _backgroundColor;

        private readonly ConsoleColor[,] _frame;

        public ConsoleFrame(Grid grid, int screenHeight, int screenWidth, ConsoleColor backGroundColor)
        {
            _grid = grid;
            Size = new Size(screenWidth, screenHeight);
            _backgroundColor = backGroundColor;
            _frame = new ConsoleColor[Size.Height, Size.Width];
        }

        public Size Size { get; }

        public ConsoleColor GetPixel(int x, int y) => _frame[y, x];

        public void Prepare()
        {
            for (var y = 0; y < Size.Height; y++)
                for (var x = 0; x < Size.Width; x++)
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
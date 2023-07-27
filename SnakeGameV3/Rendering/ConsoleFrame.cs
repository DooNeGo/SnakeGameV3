using SnakeGameV3.Model;
using System.Drawing;
using System.Numerics;

namespace SnakeGameV3.Rendering
{
    internal class ConsoleFrame
    {
        private readonly Grid _grid;
        private readonly ConsoleColor _backgroundColor;
        private readonly ConsoleColor[,] _frame;

        public ConsoleFrame(Grid grid, Size screenSize, ConsoleColor backGroundColor)
        {
            _grid = grid;
            Size = screenSize;
            _frame = new ConsoleColor[Size.Height, Size.Width];
            _backgroundColor = backGroundColor;
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

        public void Add(Vector2 position, ConsoleColor[,] model)
        {
            if (position.X >= _grid.Size.Width
                || position.X < 0
                || position.Y >= _grid.Size.Height
                || position.Y < 0)
                return;

            for (var y = 0; y < model.GetLength(0); y++)
                for (var x = 0; x < model.GetLength(1); x++)
                    _frame[(int)Math.Round(position.Y * _grid.CellSize.Height + y), (int)Math.Round(position.X * _grid.CellSize.Width + x)] = model[y, x];
        }
    }
}
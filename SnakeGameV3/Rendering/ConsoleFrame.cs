using System;
using System.Drawing;
using SnakeGameV3.Data;

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
            for (var y = 0; y < _frame.GetLength(0); y++)
                for (var x = 0; x < _frame.GetLength(1); x++)
                    _frame[y, x] = _backgroundColor;
        }

        public void Clear()
        {
            for (var y = 0; y < _frame.GetLength(0); y++)
                for (var x = 0; x < _frame.GetLength(1); x++)
                    if (_frame[y, x] != _backgroundColor)
                        _frame[y, x] = _backgroundColor;
        }

        public void Add(Point gridCoordinates, ConsoleColor[,] gameObject)
        {
            if (gridCoordinates.X >= _grid.Width
                || gridCoordinates.X < 0
                || gridCoordinates.Y >= _grid.Height
                || gridCoordinates.Y < 0)
                throw new Exception();

            for (var y = 0; y < gameObject.GetLength(0); y++)
                for (var x = 0; x < gameObject.GetLength(1); x++)
                    _frame[gridCoordinates.Y * _grid.CellSize + y, gridCoordinates.X * _grid.CellSize + x] = gameObject[y, x];
        }
    }
}
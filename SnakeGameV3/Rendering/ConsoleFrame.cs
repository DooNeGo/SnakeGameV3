using System.Drawing;
using System.Numerics;

namespace SnakeGameV3.Rendering
{
    internal class ConsoleFrame
    {
        private readonly ConsoleColor _backgroundColor;
        private readonly ConsoleColor[,] _frame;

        public ConsoleFrame(Size screenSize, ConsoleColor backGroundColor)
        {
            Size = screenSize;
            _backgroundColor = backGroundColor;
            _frame = new ConsoleColor[Size.Height, Size.Width];
        }

        public Size Size { get; }

        public ConsoleColor GetPixel(int x, int y) => _frame[y, x];

        public void Clear()
        {
            for (var y = 0; y < Size.Height; y++)
            {
                for (var x = 0; x < Size.Width; x++)
                {
                    if (_frame[y, x] != _backgroundColor)
                    {
                        _frame[y, x] = _backgroundColor;
                    }
                }
            }
        }

        public void Add(Vector2 position, ConsoleColor color, bool[,] model)
        {
            for (var y = 0; y < model.GetLength(0); y++)
            {
                float positionY = position.Y + y;

                for (var x = 0; x < model.GetLength(1); x++)
                {
                    float positionX = position.X + x;

                    if (positionY >= _frame.GetLength(0)
                        || positionX >= _frame.GetLength(1)
                        || positionX < 0
                        || positionY < 0
                        || model[y, x] == false)
                        continue;

                    _frame[(int)positionY, (int)positionX] = color;
                }
            }
        }
    }
}
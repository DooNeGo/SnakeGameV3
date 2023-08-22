using SnakeGameV3.Texturing;
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

        public void Add(Vector2 position, Texture texture)
        {
            for (var y = 0; y < texture.Size.Height; y++)
            {
                float positionY = position.Y + y;

                for (var x = 0; x < texture.Size.Width; x++)
                {
                    float positionX = position.X + x;

                    if (positionY >= _frame.GetLength(0)
                        || positionX >= _frame.GetLength(1)
                        || positionX < 0
                        || positionY < 0
                        || texture.GetPixel(x, y) == 0)
                        continue;

                    _frame[(int)positionY, (int)positionX] = texture.GetPixel(x, y);
                }
            }
        }
    }
}
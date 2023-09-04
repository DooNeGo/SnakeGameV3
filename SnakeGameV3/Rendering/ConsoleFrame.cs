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
            _frame = new ConsoleColor[screenSize.Height, screenSize.Width];
            _backgroundColor = backGroundColor;
        }

        public int Height => _frame.GetLength(0);

        public int Width => _frame.GetLength(1);

        public ConsoleColor GetPixel(int x, int y) => _frame[y, x];

        public void Clear()
        {
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
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
            for (var y = 0; y < texture.Height; y++)
            {
                var pixelPositionY = (int)(position.Y + y);

                if (pixelPositionY >= Height
                    || pixelPositionY < 0)
                    continue;

                for (var x = 0; x < texture.Width; x++)
                {
                    var pixelPositionX = (int)(position.X + x);

                    if (pixelPositionX >= Width
                        || pixelPositionX < 0
                        || texture.GetPixel(x, y) == 0)
                        continue;

                    _frame[pixelPositionY, pixelPositionX] = texture.GetPixel(x, y);
                }
            }
        }
    }
}
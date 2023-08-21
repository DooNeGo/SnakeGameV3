using System.Drawing;

namespace SnakeGameV3.Texturing
{
    internal class Texture
    {
        private readonly ConsoleColor[,] _pixels;

        public Texture(ConsoleColor[,] pixels)
        {
            _pixels = pixels;
            Size = new Size(_pixels.GetLength(1), _pixels.GetLength(0));
        }

        public Size Size { get; }

        public ConsoleColor GetPixel(int x, int y)
        {
            return _pixels[y, x];
        }
    }
}

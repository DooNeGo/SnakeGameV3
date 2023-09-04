namespace SnakeGameV3.Texturing
{
    internal class Texture
    {
        private readonly ConsoleColor[,] _pixels;

        public Texture(ConsoleColor[,] pixels)
        {
            _pixels = pixels;
        }

        public int Width => _pixels.GetLength(1);

        public int Height => _pixels.GetLength(0);

        public ConsoleColor GetPixel(int x, int y)
        {
            return _pixels[y, x];
        }
    }
}

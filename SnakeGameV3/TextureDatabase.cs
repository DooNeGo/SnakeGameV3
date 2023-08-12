using SnakeGameV3.Model;
using System.Drawing;

namespace SnakeGameV3
{
    internal enum Texture
    {
        SnakeHead,
        SnakeBody,
        Food,
    }

    internal class TextureDatabase
    {
        private readonly Grid _grid;
        private readonly Dictionary<Texture, bool[,]> _textures = new();

        public TextureDatabase(Grid grid)
        {
            _grid = grid;
        }

        public bool[,] GetTexture(Texture texture)
        {
            if (!_textures.ContainsKey(texture))
                LoadTexture(texture);

            return _textures[texture].Clone() as bool[,];
        }

        private void LoadTexture(Texture texture)
        {
            string filename = $"C:\\Users\\matve\\source\\repos\\SnakeGameV3\\SnakeGameV3\\Textures\\{texture}.bmp";
            Bitmap bitmap = (Bitmap)Image.FromFile(filename);
            bool[,] data = new bool[_grid.CellSize.Height, _grid.CellSize.Width];

            int offsetY = bitmap.Size.Height / _grid.CellSize.Height;
            int offsetX = bitmap.Size.Width / _grid.CellSize.Width;

            for (var y = 0; y < data.GetLength(0); y++)
            {
                for (var x = 0; x < data.GetLength(1); x++)
                {
                    Color pixel = bitmap.GetPixel(x * offsetX, y * offsetY);

                    if (pixel.R > 0 && pixel.B > 0 && pixel.G > 0)
                    {
                        data[y, x] = true;
                    }
                }
            }

            _textures[texture] = data;
        }
    }
}
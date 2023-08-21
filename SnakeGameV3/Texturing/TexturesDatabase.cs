using SnakeGameV3.Model;
using System.Drawing;

namespace SnakeGameV3.Texturing
{
    internal enum TextureName
    {
        SnakeHead,
        SnakeBody,
        Food,
        G,
        a,
        m,
        e,
        O,
        v,
        r,
        Space,
    }

    internal class TexturesDatabase
    {
        private readonly Grid _grid;
        private readonly Dictionary<TextureName, Texture> _textures = new();

        public TexturesDatabase(Grid grid)
        {
            _grid = grid;
        }

        public Texture GetTexture(TextureConfig textureConfig)
        {
            if (!_textures.ContainsKey(textureConfig.Name))
                LoadTexture(textureConfig);

            return _textures[textureConfig.Name];
        }

        private void LoadTexture(TextureConfig textureConfig)
        {
            DirectoryInfo directoryInfo = new($"..\\..\\..\\Textures\\{textureConfig.Name}.bmp");
            using Bitmap bitmap = (Bitmap)Image.FromFile(directoryInfo.FullName);

            var textureHeight = (int)(_grid.CellSize.Height * textureConfig.Scale);
            var textureWidth = (int)(_grid.CellSize.Width * textureConfig.Scale);

            ConsoleColor[,] pixels = new ConsoleColor[textureHeight, textureWidth];

            int offsetY = bitmap.Size.Height / pixels.GetLength(0);
            int offsetX = bitmap.Size.Width / pixels.GetLength(1);

            for (var y = 0; y < pixels.GetLength(0); y++)
            {
                for (var x = 0; x < pixels.GetLength(1); x++)
                {
                    Color pixel = bitmap.GetPixel(x * offsetX, y * offsetY);

                    if (pixel.R > 0 && pixel.B > 0 && pixel.G > 0)
                    {
                        pixels[y, x] = textureConfig.Color;
                    }
                }
            }

            _textures[textureConfig.Name] = new Texture(pixels);
        }
    }
}
using SnakeGameV3.Model;
using System.Drawing;

namespace SnakeGameV3
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
        private readonly Dictionary<TextureName, bool[,]> _textures = new();

        public TexturesDatabase(Grid grid)
        {
            _grid = grid;
        }

        public bool[,] GetTexture(TextureInfo textureInfo)
        {
            if (!_textures.ContainsKey(textureInfo.Name))
                LoadTexture(textureInfo);

            return _textures[textureInfo.Name].Clone() as bool[,];
        }

        private void LoadTexture(TextureInfo textureInfo)
        {
            string path = $"C:\\Users\\matve\\source\\repos\\SnakeGameV3\\SnakeGameV3\\Textures\\{textureInfo.Name}.bmp";
            using Bitmap bitmap = (Bitmap)Image.FromFile(path);

            var textureHeight = (int)(_grid.CellSize.Height * textureInfo.Scale);
            var textureWidth = (int)(_grid.CellSize.Width * textureInfo.Scale);

            bool[,] texture = new bool[textureHeight, textureWidth];

            int offsetY = bitmap.Size.Height / texture.GetLength(0);
            int offsetX = bitmap.Size.Width / texture.GetLength(1);

            for (var y = 0; y < texture.GetLength(0); y++)
            {
                for (var x = 0; x < texture.GetLength(1); x++)
                {
                    Color pixel = bitmap.GetPixel(x * offsetX, y * offsetY);

                    if (pixel.R > 0 && pixel.B > 0 && pixel.G > 0)
                    {
                        texture[y, x] = true;
                    }
                }
            }

            _textures[textureInfo.Name] = texture;
        }
    }
}
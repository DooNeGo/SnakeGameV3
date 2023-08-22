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
        private readonly Dictionary<TextureConfig, Texture> _transformedTextures = new();

        public TexturesDatabase(Grid grid)
        {
            _grid = grid;
            LoadTextures();
        }

        public Texture GetTexture(TextureConfig textureConfig)
        {

            return _textures[textureConfig.Name];
        }

        public Texture GetTransformedTexture(TextureConfig textureConfig, float scale)
        {
            if (_transformedTextures.ContainsKey(textureConfig))
                return _transformedTextures[textureConfig];

            Texture texture = _textures[textureConfig.Name];

            var transformedTextureHeight = (int)(_grid.CellSize.Height * scale);
            var transfomedTextureWidth = (int)(_grid.CellSize.Width * scale);
            var pixels = new ConsoleColor[transformedTextureHeight, transfomedTextureWidth];

            int offsetY = texture.Size.Height / pixels.GetLength(0);
            int offsetX = texture.Size.Width / pixels.GetLength(1);

            for (var y = 0; y < transformedTextureHeight; y++)
            {
                for (var x = 0; x < transfomedTextureWidth; x++)
                {
                    ConsoleColor color = texture.GetPixel(x * offsetX, y * offsetY);

                    if (color == ConsoleColor.White)
                        pixels[y, x] = textureConfig.Color;
                }
            }

            Texture transformedTexture = new(pixels);
            _transformedTextures.Add(textureConfig, transformedTexture);

            return transformedTexture;
        }

        private void LoadTextures()
        {
            TextureName[] names = Enum.GetValues<TextureName>();

            foreach (TextureName name in names)
            {
                DirectoryInfo directoryInfo = new($"..\\..\\..\\Textures\\{name}.bmp");
                using Bitmap bitmap = (Bitmap)Image.FromFile(directoryInfo.FullName);

                ConsoleColor[,] pixels = new ConsoleColor[bitmap.Size.Height, bitmap.Size.Width];

                for (var y = 0; y < pixels.GetLength(0); y++)
                {
                    for (var x = 0; x < pixels.GetLength(1); x++)
                    {
                        Color pixel = bitmap.GetPixel(x, y);

                        if (pixel.R > 0 || pixel.B > 0 || pixel.G > 0)
                        {
                            pixels[y, x] = ConsoleColor.White;
                        }
                    }
                }

                _textures[name] = new Texture(pixels);
            }
        }
    }
}
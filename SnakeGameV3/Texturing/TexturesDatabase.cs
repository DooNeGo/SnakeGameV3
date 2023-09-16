using SnakeGameV3.Model;
using System.Drawing;

namespace SnakeGameV3.Texturing
{
    internal enum TextureName
    {
        SnakeHead,
        SnakeBody,
        Food,
        GHigh,
        aLow,
        cLow,
        SHigh,
        mLow,
        eLow,
        OHigh,
        oLow,
        vLow,
        rLow,
        Space,
        Grid,
        Colon,
        Zero,
        One,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
    }

    internal class TexturesDatabase
    {
        private readonly Grid _grid;
        private readonly Dictionary<TextureName, Texture> _textures = new();
        private readonly Dictionary<ValueTuple<TextureConfig, float>, Texture> _transformedTextures = new();

        public TexturesDatabase(Grid grid)
        {
            _grid = grid;
            LoadTextures();
        }

        public Texture GetTransformedTexture(TextureConfig textureConfig, float scale)
        {
            if (_transformedTextures.ContainsKey((textureConfig, scale)))
                return _transformedTextures[(textureConfig, scale)];

            Texture texture = _textures[textureConfig.Name];

            var transformedTextureHeight = (int)(_grid.CellSize.Height * scale);
            var transfomedTextureWidth = (int)(_grid.CellSize.Width * scale);
            var pixels = new ConsoleColor[transformedTextureHeight, transfomedTextureWidth];

            var offsetY = (float)texture.Height / pixels.GetLength(0);
            var offsetX = (float)texture.Width / pixels.GetLength(1);

            for (var y = 0; y < transformedTextureHeight; y++)
            {
                for (var x = 0; x < transfomedTextureWidth; x++)
                {
                    ConsoleColor color = texture.GetPixel((int)(x * offsetX), (int)(y * offsetY));

                    if (color == ConsoleColor.White)
                        pixels[y, x] = textureConfig.Color;
                }
            }

            Texture transformedTexture = new(pixels);
            _transformedTextures.Add((textureConfig, scale), transformedTexture);

            return transformedTexture;
        }

        private void LoadTextures()
        {
            TextureName[] names = Enum.GetValues<TextureName>();

            foreach (TextureName name in names)
            {
                DirectoryInfo directoryInfo = new($"..\\..\\..\\Textures\\{name}.bmp");
                using Bitmap bitmap = (Bitmap)Image.FromFile(directoryInfo.FullName);

                ConsoleColor[,] pixels = new ConsoleColor[_grid.CellSize.Height * 10, _grid.CellSize.Width * 10];

                float offsetY = (float)bitmap.Size.Height / pixels.GetLength(0);
                float offsetX = (float)bitmap.Size.Width / pixels.GetLength(1);

                for (var y = 0; y < pixels.GetLength(0); y++)
                {
                    for (var x = 0; x < pixels.GetLength(1); x++)
                    {
                        Color pixel = bitmap.GetPixel((int)(x * offsetX), (int)(y * offsetY));

                        if (pixel.R == 255 && pixel.B == 255 && pixel.G == 255)
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
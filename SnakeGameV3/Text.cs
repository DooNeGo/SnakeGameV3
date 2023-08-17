using SnakeGameV3.Interfaces;
using SnakeGameV3.Model;
using System.Collections;
using System.Numerics;

namespace SnakeGameV3
{
    internal class Text : IRenderable
    {
        private readonly TextureInfo[] _texturesInfo;
        private readonly Vector2 _position;
        private readonly Grid _grid;

        public Text(string text, ConsoleColor color, float scale, Vector2 position, Grid grid)
        {
            _texturesInfo = new TextureInfo[text.Length];
            Scale = scale;
            _position = position;
            _grid = grid;
            InitializeTexturesInfo(text, color);
        }

        public float Scale { get; }

        IEnumerator IEnumerable.GetEnumerator()
        {
            for (var i = 0; i < _texturesInfo.Length; i++)
            {
                Vector2 position = new(_position.X - (_texturesInfo.Length / 2f - i) * Scale, _position.Y - 0.5f * Scale);
                yield return (_grid.GetAbsolutePosition(position), _texturesInfo[i]);
            }
        }

        IEnumerator<(Vector2, TextureInfo)> IEnumerable<(Vector2, TextureInfo)>.GetEnumerator()
        {
            for (var i = 0; i < _texturesInfo.Length; i++)
            {
                Vector2 position = new(_position.X - (_texturesInfo.Length / 2f - i) * Scale, _position.Y - 0.5f * Scale);
                yield return (_grid.GetAbsolutePosition(position), _texturesInfo[i]);
            }
        }

        private void InitializeTexturesInfo(string text, ConsoleColor color)
        {
            for (var i = 0; i < text.Length; i++)
            {
                string letter = text[i].ToString();

                if (letter == " ")
                    letter = "Space";

                _texturesInfo[i] = new TextureInfo(
                    Enum.Parse<TextureName>(letter),
                    Scale,
                    color);
            }
        }
    }
}

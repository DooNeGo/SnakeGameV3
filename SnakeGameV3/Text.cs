using SnakeGameV3.Interfaces;
using SnakeGameV3.Model;
using SnakeGameV3.Texturing;
using System.Collections;
using System.Numerics;

namespace SnakeGameV3
{
    internal class Text : IRenderable
    {
        private readonly GameObject[] _letters;
        private readonly Vector2 _textPosition;

        public Text(string text, ConsoleColor color, float scale, Vector2 position)
        {
            _letters = new GameObject[text.Length];
            Scale = scale;
            _textPosition = position;
            InitializeTexturesInfo(text, color);
        }

        public float Scale { get; }

        public bool IsNeedToProject => false;

        public IEnumerator<IReadOnlyGameObject> GetEnumerator()
        {
            foreach (GameObject gameObject in _letters)
            {
                yield return gameObject;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (GameObject gameObject in _letters)
            {
                yield return gameObject;
            }
        }

        private void InitializeTexturesInfo(string text, ConsoleColor color)
        {
            for (var i = 0; i < text.Length; i++)
            {
                string textureName = text[i].ToString();
                Vector2 position = new(_textPosition.X + (i - _letters.Length / 2f) * Scale,
                                       _textPosition.Y - 0.5f * Scale);

                if (textureName == " ")
                    textureName = "Space";

                TextureConfig textureInfo = new(
                    Enum.Parse<TextureName>(textureName),
                    Scale,
                    color);

                _letters[i] = new GameObject(position, textureInfo, ColliderType.Undefined);
            }
        }
    }
}

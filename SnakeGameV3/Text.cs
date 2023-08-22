using SnakeGameV3.Interfaces;
using SnakeGameV3.Model;
using SnakeGameV3.Texturing;
using System.Collections;
using System.Numerics;

namespace SnakeGameV3
{
    internal class Text : ICompositeObject
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

        public IEnumerator<IReadOnlyGameObject> GetGameObjectsWithComponent<T>()
        {
            foreach(IReadOnlyGameObject gameObject in _letters)
            {
                if (gameObject.GetComponent<T>() != null)
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

                TextureConfig textureInfo = new(Enum.Parse<TextureName>(textureName), color);

                _letters[i] = new GameObject(position, Scale);
                _letters[i].AddComponent(textureInfo);
            }
        }
    }
}

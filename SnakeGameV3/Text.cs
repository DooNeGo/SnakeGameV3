using SnakeGameV3.Interfaces;
using SnakeGameV3.Model;
using SnakeGameV3.Texturing;
using System.Numerics;

namespace SnakeGameV3
{
    internal class Text : ICompositeObject
    {
        private readonly GameObject[] _letters;
        private readonly Vector2 _textPosition;

        public Text(string text, ConsoleColor color, float scale, Vector2 position, Indexer indexer)
        {
            Scale = scale;
            _textPosition = position;
            _letters = new GameObject[text.Length];
            InitializeLetters(text, color, indexer);
        }

        public Text(string text, ConsoleColor color, Vector2 start, Vector2 end, Indexer indexer)
        {
            float distanceBeetweenStartAndEnd = end.X - start.X;
            Scale = distanceBeetweenStartAndEnd / text.Length;
            _textPosition = start with { X = start.X + distanceBeetweenStartAndEnd / 2 };
            _letters = new GameObject[text.Length];
            InitializeLetters(text, color, indexer);
        }

        public float Scale { get; }

        public bool IsNeedToProject => false;

        public IEnumerator<IReadOnlyGameObject> GetGameObjectsWithComponent<T>() where T : Component
        {
            foreach (IReadOnlyGameObject gameObject in _letters)
            {
                if (gameObject.GetComponent<T>() is not null)
                    yield return gameObject;
            }
        }

        private void InitializeLetters(string text, ConsoleColor color, Indexer indexer)
        {
            for (var i = 0; i < text.Length; i++)
            {
                string textureName = text[i].ToString();
                Vector2 position = new(_textPosition.X + (i - (_letters.Length - 1) / 2f) * Scale,
                                       _textPosition.Y);

                if (textureName == " ")
                    textureName = "Space";

                TextureConfig textureConfig = new(Enum.Parse<TextureName>(textureName), color);

                _letters[i] = new GameObject(position, Scale, indexer.GetUniqueIndex());
                _letters[i].AddComponent(textureConfig);
            }
        }
    }
}

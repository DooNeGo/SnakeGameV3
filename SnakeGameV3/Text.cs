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

        public Text(string text, ConsoleColor color, float scale, Vector2 position)
        {
            Scale = scale;
            _textPosition = position;
            _letters = new GameObject[text.Length];
            InitializeLetters(text, color);
        }

        public Text(string text, ConsoleColor color, Vector2 start, Vector2 end)
        {
            float distanceBeetweenStartAndEnd = end.X - start.X;
            Scale = distanceBeetweenStartAndEnd / text.Length;
            _textPosition = start with { X = start.X + distanceBeetweenStartAndEnd / 2 };
            _letters = new GameObject[text.Length];
            InitializeLetters(text, color);
        }

        public float Scale { get; }

        public IEnumerator<IReadOnlyGameObject> GetGameObjectsWithComponent<T>() where T : Component
        {
            foreach (IReadOnlyGameObject gameObject in _letters)
            {
                if (gameObject.GetComponent<T>() is not null)
                    yield return gameObject;
            }
        }

        private void InitializeLetters(string text, ConsoleColor color)
        {
            for (var i = 0; i < text.Length; i++)
            {
                string textureName = text[i].ToString();
                Vector2 position = new(_textPosition.X + (i - (_letters.Length - 1) / 2f) * Scale,
                                       _textPosition.Y);

                if (textureName == " ")
                    textureName = "Space";
                else if (textureName == ":")
                    textureName = "Colon";
                else if (textureName == "0")
                    textureName = "Zero";
                else if (textureName == "1")
                    textureName = "One";
                else if (textureName == "2")
                    textureName = "Two";
                else if (textureName == "3")
                    textureName = "Three";
                else if (textureName == "4")
                    textureName = "Four";
                else if (textureName == "5")
                    textureName = "Five";
                else if (textureName == "6")
                    textureName = "Six";
                else if (textureName == "7")
                    textureName = "Seven";
                else if (textureName == "8")
                    textureName = "Eight";
                else if (textureName == "9")
                    textureName = "Nine";
                else if (textureName == textureName.ToLower())
                    textureName += "Low";
                else
                    textureName += "High";

                _letters[i] = new GameObject(position, Scale);
                TextureConfig textureConfig = new(Enum.Parse<TextureName>(textureName), color, _letters[i]);
                _letters[i].AddComponent(textureConfig);
            }
        }
    }
}

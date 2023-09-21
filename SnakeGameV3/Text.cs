using SnakeGameV3.Components;
using SnakeGameV3.Model;
using SnakeGameV3.Texturing;
using System.Collections;
using System.Numerics;

namespace SnakeGameV3
{
    internal class Text : IEnumerable<GameObject>
    {
        private readonly GameObject[] _letters;
        private readonly Vector2 _textPosition;

        public Text(string text, ConsoleColor color, Vector2 start, Vector2 end)
        {
            float distanceBeetweenStartAndEnd = end.X - start.X;
            Scale = distanceBeetweenStartAndEnd / text.Length;
            _textPosition = start with { X = start.X + distanceBeetweenStartAndEnd / 2 };
            _letters = new GameObject[text.Length];
            InitializeLetters(text, color);
        }

        public float Scale { get; }

        public IEnumerator<GameObject> GetEnumerator()
        {
            for (var i = 0; i < _letters.Length; i++)
            {
                yield return _letters[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _letters.GetEnumerator();
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

                _letters[i] = new GameObject();
                TextureConfig textureConfig = _letters[i].AddComponent<TextureConfig>();
                textureConfig.Color = color;
                textureConfig.Name = Enum.Parse<TextureName>(textureName);

                Transform transform = _letters[i].AddComponent<Transform>();
                transform.Position = position;
                transform.Scale = Scale;
            }
        }
    }
}

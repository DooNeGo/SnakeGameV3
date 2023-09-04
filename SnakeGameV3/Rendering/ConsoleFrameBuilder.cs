using SnakeGameV3.Interfaces;
using SnakeGameV3.Model;
using SnakeGameV3.Texturing;
using System.Drawing;

namespace SnakeGameV3.Rendering
{
    internal class ConsoleFrameBuilder
    {
        private const char ConsolePixel = '█';

        private readonly ConsoleFrame[] _frames;
        private readonly TexturesDatabase _textureDatabase;
        private readonly Grid _grid;

        private Index _activeFrame = 0;
        private Index _inactiveFrame = 1;

        private DateTime _lastFrameTime;

        public ConsoleFrameBuilder(Size screenSize, ConsoleColor backgroundColor, Grid grid, Scene initialScene)
        {
            _grid = grid;
            ActiveScene = initialScene;
            _frames = new ConsoleFrame[2];
            _frames[0] = new ConsoleFrame(screenSize, backgroundColor);
            _frames[1] = new ConsoleFrame(screenSize, backgroundColor);
            _textureDatabase = new TexturesDatabase(_grid);
        }

        public TimeSpan DeltaTime => DateTime.UtcNow - _lastFrameTime;

        public Scene ActiveScene { get; set; }

        public void Update()
        {
            _lastFrameTime = DateTime.UtcNow;

            BuildImage();
            DrawImage();
        }

        private void BuildImage()
        {
            _frames[_activeFrame].Clear();

            IEnumerator<IReadOnlyGameObject> enumerator = ActiveScene.GetGameObjectsWithComponent<TextureConfig>();

            while (enumerator.MoveNext())
            {
                IReadOnlyGameObject gameObject = enumerator.Current;
                Texture texture = _textureDatabase.GetTransformedTexture(gameObject.GetComponent<TextureConfig>()!, gameObject.Scale);
                _frames[_activeFrame].Add(_grid.GetAbsolutePosition(gameObject.Position, gameObject.Scale), texture);
            }
        }

        private void DrawImage()
        {
            for (var y = 0; y < _frames[_activeFrame].Height; y++)
            {
                for (var x = 0; x < _frames[_activeFrame].Width; x++)
                {
                    if (_frames[_activeFrame].GetPixel(x, y) != _frames[_inactiveFrame].GetPixel(x, y))
                    {
                        DrawPixel(x, y);
                    }
                }
            }

            (_activeFrame, _inactiveFrame) = (_inactiveFrame, _activeFrame);
        }

        private void DrawPixel(int x, int y)
        {
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = _frames[_activeFrame].GetPixel(x, y);
            Console.Write(ConsolePixel);
        }
    }
}

using SnakeGameV3.Components;
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

        private Index _activeFrameIndex = 0;
        private Index _inactiveFrameIndex = 1;
        private DateTime _lastFrameTime;
        private Scene _activeScene;

        public ConsoleFrameBuilder(Size screenSize, ConsoleColor backgroundColor, Grid grid, Scene initialScene)
        {
            _grid = grid;
            _activeScene = initialScene;
            _frames = new ConsoleFrame[2];
            _frames[0] = new ConsoleFrame(screenSize, backgroundColor);
            _frames[1] = new ConsoleFrame(screenSize, backgroundColor);
            _textureDatabase = new TexturesDatabase(_grid);
            _textureDatabase.LoadSceneTextures(ActiveScene);
        }

        public TimeSpan DeltaTime => DateTime.UtcNow - _lastFrameTime;

        private ConsoleFrame ActiveFrame => _frames[_activeFrameIndex];

        private ConsoleFrame InactiveFrame => _frames[_inactiveFrameIndex];

        public Scene ActiveScene
        {
            get { return _activeScene; }
            set
            {
                _activeScene = value;
                _textureDatabase.LoadSceneTextures(_activeScene);
            }
        }

        public void Update()
        {
            _lastFrameTime = DateTime.UtcNow;

            BuildImage();
            DrawImage();
        }

        private void BuildImage()
        {
            ActiveFrame.Clear();

            foreach (GameObject gameObject in ActiveScene)
            {
                TextureConfig? textureConfig = gameObject.TryGetComponent<TextureConfig>();

                if (textureConfig is null)
                    continue;

                Texture texture = _textureDatabase.GetTransformedTexture(textureConfig);
                Transform transform = gameObject.GetComponent<Transform>();

                ActiveFrame.Add(_grid.GetAbsolutePosition(transform.Position, transform.Scale), texture);
            }
        }

        private void DrawImage()
        {
            for (var y = 0; y < ActiveFrame.Height; y++)
            {
                for (var x = 0; x < ActiveFrame.Width; x++)
                {
                    if (ActiveFrame.GetPixel(x, y) != InactiveFrame.GetPixel(x, y))
                    {
                        DrawPixel(x, y);
                    }
                }
            }

            (_activeFrameIndex, _inactiveFrameIndex) = (_inactiveFrameIndex, _activeFrameIndex);
        }

        private void DrawPixel(int x, int y)
        {
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = ActiveFrame.GetPixel(x, y);
            Console.Write(ConsolePixel);
        }
    }
}

using SnakeGameV3.Interfaces;
using System.Drawing;
using System.Numerics;

namespace SnakeGameV3.Rendering
{
    internal class ConsoleFrameBuilder
    {
        private const char ConsolePixel = '█';

        private readonly ConsoleFrame[] _frames;
        private readonly List<IRenderable> _entities = new();
        private readonly TexturesDatabase _textureDatabase;

        private Index _activeFrame = 0;
        private Index _inactiveFrame = 1;

        private DateTime _lastFrameTime;

        public ConsoleFrameBuilder(Size screenSize, ConsoleColor backgroundColor, TexturesDatabase textureDatabase)
        {
            _frames = new ConsoleFrame[2];
            _frames[0] = new ConsoleFrame(screenSize, backgroundColor);
            _frames[1] = new ConsoleFrame(screenSize, backgroundColor);
            _textureDatabase = textureDatabase;
        }

        public TimeSpan DeltaTime => DateTime.UtcNow - _lastFrameTime;

        public void Update()
        {
            _lastFrameTime = DateTime.UtcNow;

            BuildImage();
            DrawImage();
        }

        public void Add(IRenderable gameObject)
        {
            _entities.Add(gameObject);
        }

        public void Remove(IRenderable gameObject)
        {
            _entities.Remove(gameObject);
        }

        private void BuildImage()
        {
            _frames[_activeFrame].Clear();

            foreach (IRenderable entity in _entities)
            {
                foreach (ValueTuple<Vector2, ConsoleColor, TextureInfo> objectPart in gameObject)
                {
                    bool[,] model = _textureDatabase.GetTexture(objectPart.Item3);
                    _frames[_activeFrame].Add(objectPart.Item1, objectPart.Item2, model);
                }
            }
        }

        private void DrawImage()
        {
            for (var y = 0; y < _frames[_activeFrame].Size.Height; y++)
            {
                for (var x = 0; x < _frames[_activeFrame].Size.Width; x++)
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

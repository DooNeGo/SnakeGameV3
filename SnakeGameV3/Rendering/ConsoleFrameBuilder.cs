using SnakeGameV3.Interfaces;
using SnakeGameV3.Model;
using SnakeGameV3.Texturing;
using System.Drawing;
using System.Numerics;

namespace SnakeGameV3.Rendering
{
    internal class ConsoleFrameBuilder
    {
        private const char ConsolePixel = '█';

        private readonly ConsoleFrame[] _frames;
        private readonly List<ICompositeObject> _entities = new();
        private readonly TexturesDatabase _textureDatabase;
        private readonly Grid _grid;

        private Index _activeFrame = 0;
        private Index _inactiveFrame = 1;

        private DateTime _lastFrameTime;

        public ConsoleFrameBuilder(Size screenSize, ConsoleColor backgroundColor, Grid grid)
        {
            _frames = new ConsoleFrame[2];
            _frames[0] = new ConsoleFrame(screenSize, backgroundColor);
            _frames[1] = new ConsoleFrame(screenSize, backgroundColor);
            _grid = grid;
            _textureDatabase = new TexturesDatabase(_grid);
        }

        public TimeSpan DeltaTime => DateTime.UtcNow - _lastFrameTime;

        public void Update()
        {
            _lastFrameTime = DateTime.UtcNow;

            BuildImage();
            DrawImage();
        }

        public void Add(ICompositeObject gameObject)
        {
            _entities.Add(gameObject);
        }

        public void Remove(ICompositeObject gameObject)
        {
            _entities.Remove(gameObject);
        }

        private void BuildImage()
        {
            _frames[_activeFrame].Clear();

            foreach (ICompositeObject entity in _entities)
            {
                IEnumerator<IReadOnlyGameObject> enumerator = entity.GetGameObjectsWithComponent<TextureConfig>();

                while (enumerator.MoveNext())
                {
                    IReadOnlyGameObject gameObject = enumerator.Current;
                    Vector2 position = gameObject.Position;

                    if (entity.IsNeedToProject)
                        position = _grid.Project(position);

                    position = _grid.GetAbsolutePosition(position, entity.Scale);

                    Texture texture = _textureDatabase.GetTransformedTexture(gameObject.GetComponent<TextureConfig>()!, entity.Scale);
                    _frames[_activeFrame].Add(position, texture);
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

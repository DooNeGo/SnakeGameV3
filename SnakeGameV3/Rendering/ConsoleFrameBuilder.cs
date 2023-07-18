using SnakeGameV3.Data;
using System.Drawing;
using static SnakeGameV3.Constants.GameConstants;

namespace SnakeGameV3.Rendering
{
    internal class ConsoleFrameBuilder
    {
        public ConsoleFrameBuilder(Grid grid, int screenHeight, int screenWidth, ConsoleColor backgroundColor)
        {
            _frames = new ConsoleFrame[2];
            _frames[0] = new ConsoleFrame(grid, screenHeight, screenWidth, backgroundColor);
            _frames[1] = new ConsoleFrame(grid, screenHeight, screenWidth, backgroundColor);

            _shapeFactory = new ShapeFactory(grid.CellSize);
        }

        private readonly ConsoleFrame[] _frames;

        private readonly List<IEnumerable<KeyValuePair<Point, ConsoleColor>>> _gameObjects = new();

        private readonly ShapeFactory _shapeFactory;

        private Index _activeFrame = 0;
        private Index _inactiveFrame = 1;

        public void BuildImage()
        {
            _frames[_activeFrame].Clear();

            foreach (IEnumerable<KeyValuePair<Point, ConsoleColor>> frameObject in _gameObjects)
                foreach (KeyValuePair<Point, ConsoleColor> objectPart in frameObject)
                    _frames[_activeFrame].Add(objectPart.Key, _shapeFactory.GetSquare(objectPart.Value));
        }

        public void DrawImage()
        {
            for (var y = 0; y < _frames[_activeFrame].Height; y++)
            {
                for (var x = 0; x < _frames[_activeFrame].Width; x++)
                {
                    if (_frames[_activeFrame].GetPixel(x, y) != _frames[_inactiveFrame].GetPixel(x, y))
                    {
                        Console.SetCursorPosition(x, y);
                        Console.ForegroundColor = _frames[_activeFrame].GetPixel(x, y);
                        Console.Write(PixelModel);
                    }
                }
            }

            (_activeFrame, _inactiveFrame) = (_inactiveFrame, _activeFrame);
        }

        public void Add(IEnumerable<KeyValuePair<Point, ConsoleColor>> frameObject)
        {
            _gameObjects.Add(frameObject);
        }

        public void Remove(IEnumerable<KeyValuePair<Point, ConsoleColor>> frameObject)
        {
            _gameObjects.Remove(frameObject);
        }
    }
}

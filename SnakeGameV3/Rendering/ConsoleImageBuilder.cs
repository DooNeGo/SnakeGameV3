using SnakeGameV3.Data;
using System.Drawing;
using static SnakeGameV3.Constants.GameConstants;

namespace SnakeGameV3.Rendering
{
    internal class ConsoleImageBuilder
    {
        public ConsoleImageBuilder(Grid grid, int screenHeight, int screenWidth, ConsoleColor backgroundColor)
        {
            _frames = new ConsoleFrame[2];
            _frames[0] = new ConsoleFrame(grid, screenHeight, screenWidth, backgroundColor);
            _frames[1] = new ConsoleFrame(grid, screenHeight, screenWidth, backgroundColor);

        }

        private readonly ConsoleFrame[] _frames;

        private Index _activeFrame = 0;
        private Index _inactiveFrame = 1;

        public void BuildImage(List<ConsoleObject> renderList)
        {
            _frames[_activeFrame].Clear();

            foreach (ConsoleObject gameObject in renderList)
                _frames[_activeFrame].Add(gameObject.Coordinates, gameObject.Model);
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
    }
}

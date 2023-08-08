﻿using SnakeGameV3.Interfaces;
using System.Drawing;
using System.Numerics;
using static SnakeGameV3.Config;

namespace SnakeGameV3.Rendering
{
    internal class ConsoleFrameBuilder
    {
        private readonly ConsoleFrame[] _frames;
        private readonly List<IRenderable> _gameObjects = new();
        private readonly ShapeFactory _shapeFactory;

        private Index _activeFrame = 0;
        private Index _inactiveFrame = 1;

        private DateTime _lastFrameTime;

        public ConsoleFrameBuilder(Size screenSize, ConsoleColor backgroundColor)
        {
            _frames = new ConsoleFrame[2];
            _frames[0] = new ConsoleFrame(screenSize, backgroundColor);
            _frames[1] = new ConsoleFrame(screenSize, backgroundColor);

            _shapeFactory = new ShapeFactory(new Size(GridCellWidth, GridCellHeight));
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
            _gameObjects.Add(gameObject);
        }

        public void Remove(IRenderable gameObject)
        {
            _gameObjects.Remove(gameObject);
        }

        private void BuildImage()
        {
            _frames[_activeFrame].Clear();

            foreach (IRenderable frameObject in _gameObjects)
            {
                foreach (ValueTuple<Vector2, ConsoleColor> objectPart in frameObject)
                {
                    _frames[_activeFrame].Add(objectPart.Item1, _shapeFactory.GetSquare(objectPart.Item2));
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

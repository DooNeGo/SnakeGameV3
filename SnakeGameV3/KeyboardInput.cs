using System.Numerics;

namespace SnakeGameV3
{
    internal class KeyboardInput
    {
        private readonly PhysicsMovement _movement;

        private Vector2 _lastDirection = Vector2.Zero;
        private Vector2 _lastOffset = Vector2.Zero;

        private readonly Dictionary<ConsoleKey, Vector2> _directions = new(4);

        public KeyboardInput(PhysicsMovement movement)
        {
            _movement = movement;
            _directions.Add(ConsoleKey.UpArrow, -Vector2.UnitY);
            _directions.Add(ConsoleKey.DownArrow, Vector2.UnitY);
            _directions.Add(ConsoleKey.LeftArrow, -Vector2.UnitX);
            _directions.Add(ConsoleKey.RightArrow, Vector2.UnitX);
        }

        public void Update()
        {
            _lastDirection = ReadMovement();
            _movement.Move(_lastDirection);
        }

        private Vector2 ReadMovement()
        {
            ConsoleKey pressedKey = new();

            if (Console.KeyAvailable)
                pressedKey = Console.ReadKey().Key;

            //return pressedKey switch
            //{
            //    ConsoleKey.UpArrow when _lastDirection != Vector2.UnitY => -Vector2.UnitY,
            //    ConsoleKey.DownArrow when _lastDirection != -Vector2.UnitY => Vector2.UnitY,
            //    ConsoleKey.LeftArrow when _lastDirection != Vector2.UnitX => -Vector2.UnitX,
            //    ConsoleKey.RightArrow when _lastDirection != -Vector2.UnitX => Vector2.UnitX,
            //    ConsoleKey.Spacebar => Vector2.Zero,
            //    _ => _lastDirection
            //};

            //return pressedKey switch
            //{
            //    ConsoleKey.UpArrow => Vector2.Normalize(_lastDirection - Vector2.UnitY),
            //    ConsoleKey.DownArrow => Vector2.Normalize(_lastDirection + Vector2.UnitY),
            //    ConsoleKey.LeftArrow => Vector2.Normalize(_lastDirection - Vector2.UnitX),
            //    ConsoleKey.RightArrow => Vector2.Normalize(_lastDirection + Vector2.UnitX),
            //    _ when _lastDirection != Vector2.Zero => Vector2.Normalize(_lastDirection * Vector2.Abs(_lastDirection)),
            //    _ => _lastDirection
            //};

            if (!_directions.ContainsKey(pressedKey)
                && _lastDirection == Vector2.Zero)
            {
                return _lastDirection;
            }

            if (!_directions.ContainsKey(pressedKey)
                || _directions[pressedKey] == -_lastOffset)
            {
                return Vector2.Normalize(_lastDirection + _lastOffset);
            }

            return Vector2.Normalize(_lastDirection + _directions[pressedKey]);
        }
    }
}

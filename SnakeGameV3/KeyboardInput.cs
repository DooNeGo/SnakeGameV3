using System.Numerics;

namespace SnakeGameV3
{
    internal class KeyboardInput
    {
        public event Action<ConsoleKey>? KeyDown;

        private Vector2 _lastDirection = Vector2.Zero;
        private ConsoleKey _pressedKey = new();

        public void Update()
        {
            if (Console.KeyAvailable)
            {
                _pressedKey = Console.ReadKey().Key;
                KeyDown?.Invoke(_pressedKey);
            }
        }

        public Vector2 ReadMovement()
        {
            _lastDirection = _pressedKey switch
            {
                ConsoleKey.UpArrow => -Vector2.UnitY,
                ConsoleKey.DownArrow => Vector2.UnitY,
                ConsoleKey.LeftArrow => -Vector2.UnitX,
                ConsoleKey.RightArrow => Vector2.UnitX,
                _ => _lastDirection
            };

            return _lastDirection;
        }
    }
}

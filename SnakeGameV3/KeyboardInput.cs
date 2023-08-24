using System.Numerics;

namespace SnakeGameV3
{
    internal class KeyboardInput
    {
        private readonly PhysicsMovement _movement;

        private Vector2 _lastDirection = Vector2.Zero;

        public KeyboardInput(PhysicsMovement movement)
        {
            _movement = movement;
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

            return pressedKey switch
            {
                ConsoleKey.UpArrow when _lastDirection != Vector2.UnitY => -Vector2.UnitY,
                ConsoleKey.DownArrow when _lastDirection != -Vector2.UnitY => Vector2.UnitY,
                ConsoleKey.LeftArrow when _lastDirection != Vector2.UnitX => -Vector2.UnitX,
                ConsoleKey.RightArrow when _lastDirection != -Vector2.UnitX => Vector2.UnitX,
                ConsoleKey.Spacebar => Vector2.Zero,
                _ => _lastDirection
            };
        }
    }
}

using SnakeGameV3.Movements;
using System.Numerics;

namespace SnakeGameV3.Input
{
    internal class KeyboardInput
    {
        private readonly PhysicsMovement _movement;

        private Vector2 _lastDirection = new(0);

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
                ConsoleKey.UpArrow when _lastDirection.Y != 1 => new Vector2(0, -1),
                ConsoleKey.DownArrow when _lastDirection.Y != -1 => new Vector2(0, 1),
                ConsoleKey.LeftArrow when _lastDirection.X != 1 => new Vector2(-1, 0),
                ConsoleKey.RightArrow when _lastDirection.X != -1 => new Vector2(1, 0),
                ConsoleKey.Spacebar => new Vector2(0),
                _ => _lastDirection
            };
        }
    }
}

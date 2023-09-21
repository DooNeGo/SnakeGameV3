using System.Numerics;

namespace SnakeGameV3
{
    internal class KeyboardInput
    {
        private readonly PhysicsMovement _movement;

        private Vector2 _lastDirection = Vector2.Zero;
        private ConsoleKey _pressedKey = new();

        public KeyboardInput(PhysicsMovement movement)
        {
            _movement = movement;
            Task.Run(() =>
            {
                while (true)
                {
                    _pressedKey = Console.ReadKey().Key;
                }
            });
        }

        public void Update()
        {
            _lastDirection = ReadMovement();
            _movement.Move(_lastDirection);
        }

        private Vector2 ReadMovement()
        {
            //if (Console.KeyAvailable)
            //    _pressedKey = Console.ReadKey().Key;

            return _pressedKey switch
            {
                ConsoleKey.UpArrow => -Vector2.UnitY,
                ConsoleKey.DownArrow => Vector2.UnitY,
                ConsoleKey.LeftArrow => -Vector2.UnitX,
                ConsoleKey.RightArrow => Vector2.UnitX,
                ConsoleKey.Spacebar => Vector2.Zero,
                _ => _lastDirection
            };
        }
    }
}

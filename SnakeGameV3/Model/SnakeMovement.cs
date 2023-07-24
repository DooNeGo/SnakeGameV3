using SnakeGameV3.Interfaces;
using System.Numerics;

namespace SnakeGameV3.Model
{
    internal class SnakeMovement : IMovement
    {
        private readonly IMovable _snake;

        public SnakeMovement(IMovable snake)
        {
            _snake = snake;
        }

        public void Move(Vector2 direction)
        {
            if (_snake.DeltaTime < _snake.MoveLatency)
                return;

            Vector2 offset = direction * (_snake.MoveSpeed * _snake.DeltaTime / 1000);
            _snake.MoveToPosition(_snake.Position + offset);
        }
    }
}
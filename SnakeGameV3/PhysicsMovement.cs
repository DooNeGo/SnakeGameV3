using SnakeGameV3.Interfaces;
using System.Numerics;

namespace SnakeGameV3
{
    internal class PhysicsMovement
    {
        private readonly IMovable _body;

        private Vector2 _lastSmoothDirection = Vector2.Zero;
        private Vector2 _lastDirection = Vector2.Zero;

        public PhysicsMovement(IMovable body)
        {
            _body = body;
        }

        public void Move(Vector2 direction)
        {
            Vector2 offset = Vector2.Zero;

            //if ((int)_lastSmoothDirection.X == _lastDirection.X
            //    && (int)_lastSmoothDirection.Y == _lastDirection.Y)
            //    _lastDirection = direction;

            if (direction != Vector2.Zero)
            {
                _lastSmoothDirection = Vector2.Normalize(_lastSmoothDirection + direction / (10 / _body.MoveSpeed));
                offset = _lastSmoothDirection * (float)(_body.MoveSpeed * _body.DeltaTime.TotalSeconds);
            }

            _body.MoveTo(_body.Position + offset);
        }
    }
}
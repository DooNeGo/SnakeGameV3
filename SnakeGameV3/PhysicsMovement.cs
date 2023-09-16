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

            if (MathF.Abs(_lastSmoothDirection.X - _lastDirection.X) < 3e-3
                && (MathF.Abs(_lastSmoothDirection.Y - _lastDirection.Y) < 3e-3)
                && direction != -_lastDirection)
            {
                _lastDirection = direction;
            }

            if (_lastDirection != Vector2.Zero)
            {
                _lastSmoothDirection = Vector2.Normalize(_lastSmoothDirection + _lastDirection / (15 / _body.MoveSpeed));
                offset = _lastSmoothDirection * (float)(_body.MoveSpeed * _body.DeltaTime.TotalSeconds);
            }

            _body.MoveTo(_body.Position + offset);
        }
    }
}
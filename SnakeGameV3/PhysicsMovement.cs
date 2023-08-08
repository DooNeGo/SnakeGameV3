using SnakeGameV3.Interfaces;
using System.Numerics;

namespace SnakeGameV3
{
    internal class PhysicsMovement
    {
        private readonly IMovable _body;

        public PhysicsMovement(IMovable body)
        {
            _body = body;
        }

        public void Move(Vector2 direction)
        {
            Vector2 offset = direction * (float)(_body.MoveSpeed * _body.DeltaTime.TotalSeconds);
            _body.MoveToPosition(_body.Position + offset);
        }
    }
}
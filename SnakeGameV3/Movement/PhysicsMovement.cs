using SnakeGameV3.Interfaces;
using System.Numerics;

namespace SnakeGameV3.Movements
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
            Vector2 offset = direction * (_body.MoveSpeed * _body.DeltaTime / 1000);
            _body.MoveToPosition(_body.Position + offset);
        }
    }
}
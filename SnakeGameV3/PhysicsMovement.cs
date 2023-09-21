﻿using SnakeGameV3.Interfaces;
using System.Numerics;

namespace SnakeGameV3
{
    internal class PhysicsMovement
    {
        private readonly IMovable _body;
        private readonly float _slewingTime;

        private Vector2 _smoothDirection = Vector2.Zero;
        private Vector2 _lastDirection = Vector2.Zero;
        private Vector2 _penultimateDirection = Vector2.UnitX;

        public PhysicsMovement(IMovable body, float slewingTime)
        {
            _body = body;
            _slewingTime = slewingTime;
        }

        public void Move(Vector2 direction, TimeSpan delta)
        {
            if (_lastDirection == Vector2.Zero
                && direction != Vector2.Zero
                && _penultimateDirection + direction != Vector2.Zero
                || _lastDirection != Vector2.Zero
                && MathF.Abs(_lastDirection.X - _smoothDirection.X) < 1e-1
                && MathF.Abs(_lastDirection.Y - _smoothDirection.Y) < 1e-1
                && _lastDirection + direction != Vector2.Zero)
            {
                _penultimateDirection = _lastDirection;
                _lastDirection = direction;
            }

            if (_lastDirection != Vector2.Zero)
                _smoothDirection = Vector2.Normalize(_smoothDirection + _lastDirection * ((float)delta.TotalSeconds / _slewingTime * _body.MoveSpeed / _body.Scale));
            else
                _smoothDirection = Vector2.Zero;

            Vector2 offset = (float)delta.TotalSeconds * _body.MoveSpeed * _smoothDirection;
            _body.MoveTo(_body.Position + offset);
        }
    }
}
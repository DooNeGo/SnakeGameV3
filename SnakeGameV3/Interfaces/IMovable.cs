﻿using System.Numerics;

namespace SnakeGameV3.Interfaces
{
    internal interface IMovable
    {
        public float MoveSpeed { get; }

        public Vector2 Position { get; }

        public float Scale { get; }

        public void MoveTo(Vector2 position);
    }
}

using System.Numerics;

namespace SnakeGameV3.Interfaces
{
    internal interface IMovable
    {
        public void MoveToPosition(Vector2 point);

        public float MoveSpeed { get; }

        public Vector2 Position { get; }

        public long DeltaTime { get; }
    }
}

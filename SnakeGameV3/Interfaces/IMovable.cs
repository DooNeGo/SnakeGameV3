using SnakeGameV3.Enums;

namespace SnakeGameV3.Interfaces
{
    internal interface IMovable
    {
        public void Move(Direction direction);

        public bool IsReadyForMove { get; }

        public double MoveLatency { get; }
    }
}

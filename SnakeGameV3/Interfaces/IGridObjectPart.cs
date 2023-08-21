using System.Numerics;

namespace SnakeGameV3.Interfaces
{
    internal interface IGridObjectPart : IScalable
    {
        public Vector2 Position { get; }

        public ColliderType ColliderType { get; }
    }
}

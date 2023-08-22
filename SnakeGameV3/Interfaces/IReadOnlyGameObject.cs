using System.Numerics;

namespace SnakeGameV3.Interfaces
{
    internal interface IReadOnlyGameObject
    {
        public Vector2 Position { get; }

        public T? GetComponent<T>();
    }
}

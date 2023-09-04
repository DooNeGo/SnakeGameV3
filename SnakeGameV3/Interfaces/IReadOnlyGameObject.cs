using System.Numerics;

namespace SnakeGameV3.Interfaces
{
    internal interface IReadOnlyGameObject : IScalable
    {
        public Vector2 Position { get; }

        public int Id { get; }

        public T? GetComponent<T>() where T : Component;

        public IReadOnlyGameObject Clone(Vector2 position);
    }
}
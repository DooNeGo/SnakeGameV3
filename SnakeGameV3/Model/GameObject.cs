using SnakeGameV3.Interfaces;
using System.Numerics;

namespace SnakeGameV3.Model
{
    internal class GameObject : IReadOnlyGameObject
    {
        private readonly List<ComponentConfig> _components = new();

        public GameObject(Vector2 position)
        {
            Position = position;
        }

        public Vector2 Position { get; set; }

        public void AddComponent(ComponentConfig component)
        {
            _components.Add(component);
        }

        public void RemoveComponent(ComponentConfig component)
        {
            _components.Remove(component);
        }

        public T? GetComponent<T>()
        {
            foreach (ComponentConfig component in _components)
            {
                if (component is T value)
                    return value;
            }

            return default;
        }
    }
}

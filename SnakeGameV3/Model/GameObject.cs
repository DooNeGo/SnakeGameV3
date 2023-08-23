using SnakeGameV3.Interfaces;
using System.Numerics;

namespace SnakeGameV3.Model
{
    internal class GameObject : IReadOnlyGameObject
    {
        private readonly List<Component> _components = new();

        public GameObject(Vector2 position)
        {
            Position = position;
        }

        public Vector2 Position { get; set; }

        public void AddComponent(Component component)
        {
            _components.Add(component);
        }

        public void RemoveComponent(Component component)
        {
            _components.Remove(component);
        }

        public T? GetComponent<T>()
        {
            foreach (Component component in _components)
            {
                if (component is T value)
                    return value;
            }

            return default;
        }
    }
}

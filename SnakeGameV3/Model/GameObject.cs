using SnakeGameV3.Interfaces;
using System.Numerics;

namespace SnakeGameV3.Model
{
    internal class GameObject : IReadOnlyGameObject
    {
        private readonly List<Component> _components = new();

        public GameObject(Vector2 position, float scale)
        {
            Position = position;
            Scale = scale;
        }

        public Vector2 Position { get; set; }

        public float Scale { get; set; }

        public void AddComponent(Component component)
        {
            _components.Add(component);
        }

        public void RemoveComponent(Component component)
        {
            _components.Remove(component);
        }

        public T? GetComponent<T>() where T : Component
        {
            foreach (Component component in _components)
            {
                if (component is T value)
                    return value;
            }

            return default;
        }

        public IReadOnlyGameObject Clone(Vector2 position)
        {
            var clone = (GameObject)MemberwiseClone();
            clone.Position = position;

            return clone;
        }
    }
}

using SnakeGameV3.Components;

namespace SnakeGameV3.Model
{
    internal class GameObject : Component
    {
        private readonly List<Component> _components = new();

        public new T AddComponent<T>() where T : Component, new()
        {
            //ConstructorInfo? info = typeof(T).GetConstructor(new Type[] { typeof(GameObject) });
            //T component = (T)info!.Invoke(new object?[] { this });
            T component = new()
            {
                Parent = this
            };
            _components.Add(component);
            return component;
        }

        public void RemoveComponent<T>()
        {
            for (int i = 0; i < _components.Count; i++)
            {
                if (_components[i] is T)
                    _components.RemoveAt(i);
            }
        }

        public new T GetComponent<T>() where T : Component
        {
            for (int i = 0; i < _components.Count; i++)
            {
                if (_components[i] is T value)
                    return value;
            }

            throw new Exception();
        }

        public T? TryGetComponent<T>() where T : Component
        {
            for (int i = 0; i < _components.Count; i++)
            {
                if (_components[i] is T value)
                    return value;
            }

            return null;
        }

        public List<T> GetComponents<T>() where T : Component
        {
            List<T> components = new();

            for (int i = 0; i < _components.Count; i++)
            {
                if (_components[i] is T component)
                {
                    components.Add(component);
                }
            }

            return components;
        }
    }
}

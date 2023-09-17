using SnakeGameV3.Interfaces;

namespace SnakeGameV3
{
    internal abstract class Component
    {
        protected readonly IReadOnlyGameObject _parent;

        public Component(IReadOnlyGameObject parent)
        {
            _parent = parent;
        }
    }
}
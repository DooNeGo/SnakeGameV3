using SnakeGameV3.Interfaces;

namespace SnakeGameV3.Model
{
    internal struct Cell : ICollidable
    {
        public Cell()
        {
            IsCollidable = false;
            Boss = null;
        }

        public bool IsCollidable { get; private set; }

        public object? Boss { get; private set; }

        public void Occupy(ICollidable entity)
        {
            IsCollidable = entity.IsCollidable;
            Boss = entity;
        }

        public void Clear()
        {
            IsCollidable = false;
            Boss = null;
        }
    }
}

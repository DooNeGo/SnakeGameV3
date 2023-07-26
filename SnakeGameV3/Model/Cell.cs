using SnakeGameV3.Enums;
using SnakeGameV3.Interfaces;

namespace SnakeGameV3.Model
{
    internal class Cell
    {
        public Cell()
        {
            Type = PassType.Passable;
            Boss = null;
        }

        public PassType Type { get; private set; }

        public object? Boss { get; private set; }

        public void Occupy(ICellObject entity)
        {
            Type = entity.Type;
            Boss = entity;
        }

        public void Clear()
        {
            Type = PassType.Passable;
            Boss = null;
        }
    }
}

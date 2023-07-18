using SnakeGameV3.Enums;

namespace SnakeGameV3.Interfaces
{
    internal interface ICellObject
    {
        public bool IsCrashed { get; set; }

        public PassType Type { get; }
    }
}

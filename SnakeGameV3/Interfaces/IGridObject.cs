using SnakeGameV3.Enums;
using System.Numerics;

namespace SnakeGameV3.Interfaces
{
    internal interface IGridObject : IEnumerable<Vector2>
    {
        public PassType Type { get; }
    }
}

using System.Drawing;

namespace SnakeGameV3.Interfaces
{
    internal interface IGridObject : IPassable, IEnumerable<Point>
    {
        public bool IsCrashed { get; set; }
    }
}

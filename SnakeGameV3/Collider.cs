using System.Drawing;

namespace SnakeGameV3
{
    public class Collider
    {
        private readonly bool[,] _bounds;

        public Collider(bool[,] bounds)
        {
            _bounds = bounds;
            Size = new Size(_bounds.GetLength(1), _bounds.GetLength(0));
        }

        public Size Size { get; }

        public bool GetValue(int x, int y)
        {
            return _bounds[y, x];
        }
    }
}
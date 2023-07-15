using SnakeGameV3.Interfaces;
using System.Collections;
using System.Drawing;

namespace SnakeGameV3.Data
{
    internal class Food : IEnumerable<Point>, IPassable
    {
        public Food(ConsoleColor color, Grid grid)
        {
            Color = color;
            _grid = grid;
            RandCoordinates();
        }

        private readonly Random _random = new();

        private readonly Grid _grid;

        public Point Coordinates { get; private set; }

        public ConsoleColor Color { get; }

        public bool IsPassable { get; } = true;

        public void RandCoordinates()
        {
            do
            {
                Coordinates = new Point(_random.Next(1, _grid.Width - 2), _random.Next(1, _grid.Height - 2));
            } while (_grid.IsOccupiedCell[Coordinates.Y, Coordinates.X] == true);
        }

        public IEnumerator<Point> GetEnumerator()
        {
            yield return Coordinates;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            yield return Coordinates;
        }
    }
}
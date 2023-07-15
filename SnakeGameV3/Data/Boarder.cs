using SnakeGameV3.Interfaces;
using System.Collections;
using System.Drawing;

namespace SnakeGameV3.Data
{
    internal class Boarder : IEnumerable<Point>, IPassable
    {
        public Boarder(Grid grid, ConsoleColor color)
        {
            _grid = grid;
            Color = color;
            InitializeBoarder();
        }

        private List<Point> _points = new();

        private Grid _grid;

        public ConsoleColor Color { get; }

        public bool IsPassable { get; } = false;

        private void InitializeBoarder()
        {
            for (var i = 0; i < _grid.Width; i++)
            {
                _points.Add(new Point(i, 0));
                _points.Add(new Point(i, _grid.Height - 1));
            }

            for (var i = 0; i < _grid.Height; i++)
            {
                _points.Add(new Point(0, i));
                _points.Add(new Point(_grid.Width - 1, i));
            }
        }


        public IEnumerator<Point> GetEnumerator() => _points.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _points.GetEnumerator();
    }
}
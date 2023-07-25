using SnakeGameV3.Enums;
using SnakeGameV3.Interfaces;
using System.Collections;
using System.Numerics;

namespace SnakeGameV3.Data
{
    internal class Boarder : IGridObject, IEnumerable<ValueTuple<Vector2, ConsoleColor>>
    {
        private readonly List<Vector2> _points = new();
        private readonly Grid _grid;

        public Boarder(Grid grid, ConsoleColor color)
        {
            _grid = grid;
            Color = color;
            InitializeBoarder();
        }

        public ConsoleColor Color { get; }

        public PassType Type => PassType.Impassable;

        public bool IsCrashed { get; set; } = false;

        public IEnumerator<Vector2> GetEnumerator() => _points.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _points.GetEnumerator();

        IEnumerator<ValueTuple<Vector2, ConsoleColor>> IEnumerable<ValueTuple<Vector2, ConsoleColor>>.GetEnumerator()
        {
            foreach (Vector2 point in _points)
                yield return new ValueTuple<Vector2, ConsoleColor>(point, Color);
        }

        private void InitializeBoarder()
        {
            for (var i = 1; i < _grid.Size.Width - 1; i++)
            {
                _points.Add(new Vector2(i, 0));
                _points.Add(new Vector2(i, _grid.Size.Height - 1));
            }

            for (var i = 0; i < _grid.Size.Height; i++)
            {
                _points.Add(new Vector2(0, i));
                _points.Add(new Vector2(_grid.Size.Width - 1, i));
            }
        }
    }
}
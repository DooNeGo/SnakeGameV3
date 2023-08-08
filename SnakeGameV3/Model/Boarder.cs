using SnakeGameV3.Interfaces;
using System.Collections;
using System.Numerics;

namespace SnakeGameV3.Model
{
    internal class Boarder : IGridObject, IRenderable
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

        public bool IsCollidable => true;

        public IEnumerator<Vector2> GetEnumerator() => _points.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _points.GetEnumerator();

        IEnumerator<ValueTuple<Vector2, ConsoleColor>> IEnumerable<ValueTuple<Vector2, ConsoleColor>>.GetEnumerator()
        {
            foreach (Vector2 point in _points)
                yield return new ValueTuple<Vector2, ConsoleColor>(point, Color);
        }

        private void InitializeBoarder()
        {
            for (var i = 0; i < _grid.Size.Width; i++)
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
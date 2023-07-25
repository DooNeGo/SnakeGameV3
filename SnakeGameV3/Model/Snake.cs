using SnakeGameV3.Enums;
using SnakeGameV3.Interfaces;
using System.Collections;
using System.Diagnostics;
using System.Numerics;

namespace SnakeGameV3.Data
{
    internal class Snake : IMovable, IGridObject, IEnumerable<ValueTuple<Vector2, ConsoleColor>>
    {
        private readonly Stopwatch _stopwatch = Stopwatch.StartNew();
        private readonly List<Vector2> _body = new();
        private readonly Grid _grid;

        private Vector2 _head;

        public Snake(Vector2 startPosition, ConsoleColor headColor, ConsoleColor bodyColor, float speed, Grid grid)
        {
            _head = startPosition;
            HeadColor = headColor;
            BodyColor = bodyColor;
            MoveSpeed = speed;
            _grid = grid;

            _body.Add(new Vector2(_head.X - 1, _head.Y));
            _body.Add(new Vector2(_head.X - 2, _head.Y));
        }

        public Vector2 Position => _head;

        public ConsoleColor HeadColor { get; }

        public ConsoleColor BodyColor { get; }

        public float MoveSpeed { get; }

        public int MoveLatency => (int)(1000 / MoveSpeed);

        public PassType Type => PassType.Impassable;

        public long DeltaTime => _stopwatch.ElapsedMilliseconds;

        public bool IsCrashed { get; set; } = false;

        public void MoveToPosition(Vector2 nextPosition)
        {
            _stopwatch.Restart();

            Vector2 offset = nextPosition - _head;
            Vector2 direction = _head - _body[0];

            _body[0] += direction * Math.Abs(offset.Length());

            for (var i = 1; i < _body.Count; i++)
            {
                direction = _body[i - 1] - _body[i];
                _body[i] += direction * Math.Abs(offset.Length());
            }

            _head = nextPosition;
        }

        public void TryToEat(Food food)
        {
            if (Math.Round(_head.X) != Math.Round(food.Position.X) || Math.Round(_head.Y) != Math.Round(food.Position.Y))
                return;

            _body.Add(new Vector2(_grid.Size.Width, 0));

            food.RandCoordinates();
        }

        public IEnumerator<Vector2> GetEnumerator()
        {
            yield return _head;

            foreach (Vector2 position in _body)
                yield return position;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            yield return _head;

            foreach (Vector2 position in _body)
                yield return position;
        }

        IEnumerator<ValueTuple<Vector2, ConsoleColor>> IEnumerable<ValueTuple<Vector2, ConsoleColor>>.GetEnumerator()
        {
            yield return new ValueTuple<Vector2, ConsoleColor>(_head, HeadColor);

            foreach (Vector2 position in _body)
                yield return new ValueTuple<Vector2, ConsoleColor>(position, BodyColor);
        }
    }
}
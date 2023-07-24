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
        private readonly Queue<Vector2> _body = new();
        private readonly Grid _grid;

        private Vector2 _head;

        public Snake(Vector2 startPosition, ConsoleColor headColor, ConsoleColor bodyColor, float speed, Grid grid)
        {
            _head = startPosition;
            HeadColor = headColor;
            BodyColor = bodyColor;
            MoveSpeed = speed;
            MoveLatency = (int)(1000 / MoveSpeed);
            _grid = grid;

            _body.Enqueue(new Vector2(_head.X - 2, _head.Y));
            _body.Enqueue(new Vector2(_head.X - 1, _head.Y));
        }

        public Vector2 Position => _head;

        public ConsoleColor HeadColor { get; }

        public ConsoleColor BodyColor { get; }

        public float MoveSpeed { get; }

        public int MoveLatency { get; }

        public bool IsDied { get; private set; } = false;

        public PassType Type => PassType.Impassable;

        public long DeltaTime => _stopwatch.ElapsedMilliseconds;

        public void MoveToPosition(Vector2 nextPosition)
        {
            _stopwatch.Restart();

            if (_grid.IsOccupiedCell(nextPosition) && nextPosition != _head)
                IsDied = true;

            if ((int)nextPosition.X != (int)_head.X || (int)nextPosition.Y != (int)_head.Y)
            {
                _body.Dequeue();
                _body.Enqueue(_head);
            }

            _head = nextPosition;
        }

        public bool TryToEat(Food food)
        {
            if ((int)_head.X != (int)food.Position.X || (int)_head.Y != (int)food.Position.Y)
                return false;

            _body.Enqueue(new Vector2(_grid.Size.Width, 0));

            return true;
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
using SnakeGameV3.Interfaces;
using System.Collections;
using System.Numerics;

namespace SnakeGameV3.Model
{
    internal class Snake : IMovable, IGridObject, IRenderable
    {
        private readonly List<Vector2> _body = new();
        private readonly Grid _grid;

        private Vector2 _head;
        private Vector2 _headOffset;

        private DateTime _lastMoveTime;

        public Snake(Vector2 startPosition, ConsoleColor headColor, ConsoleColor bodyColor, float speed, Grid grid)
        {
            HeadColor = headColor;
            BodyColor = bodyColor;
            MoveSpeed = speed;
            _grid = grid;
            _head = new Vector2(startPosition.X, startPosition.Y);

            _body.Add(new Vector2(_head.X - 1, _head.Y));
            _body.Add(new Vector2(_head.X - 2, _head.Y));
        }

        public Vector2 Position => _head;

        public ConsoleColor HeadColor { get; }

        public ConsoleColor BodyColor { get; }

        public float MoveSpeed { get; }

        public bool IsCollidable => true;

        public TimeSpan DeltaTime => DateTime.UtcNow - _lastMoveTime;

        public bool IsCrashed { get; private set; } = false;

        public void MoveToPosition(Vector2 nextPosition)
        {
            _lastMoveTime = DateTime.UtcNow;

            if (nextPosition == _head)
                return;

            CheckPosition(nextPosition);

            ApplyOffsets(CalculateOffsets(nextPosition));

            _head = nextPosition;
        }

        public IEnumerator<Vector2> GetEnumerator()
        {
            yield return _grid.Project(_head);

            foreach (Vector2 position in _body)
                yield return _grid.Project(position);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            yield return _head;

            foreach (Vector2 position in _body)
                yield return position;
        }

        IEnumerator<ValueTuple<Vector2, ConsoleColor>> IEnumerable<ValueTuple<Vector2, ConsoleColor>>.GetEnumerator()
        {
            yield return new ValueTuple<Vector2, ConsoleColor>(_grid.Project(_head), HeadColor);

            foreach (Vector2 position in _body)
                yield return new ValueTuple<Vector2, ConsoleColor>(_grid.Project(position), BodyColor);
        }

        private Vector2[] CalculateOffsets(Vector2 nextPosition)
        {
            var offsets = new Vector2[_body.Count];

            _headOffset = nextPosition - _head;
            offsets[0] = _head - _body[0];

            for (var i = 1; i < _body.Count; i++)
            {
                offsets[i] = _body[i - 1] - _body[i];
            }

            return offsets;
        }

        private void ApplyOffsets(Vector2[] offsets)
        {
            for (var i = 0; i < offsets.Length; i++)
            {
                _body[i] += offsets[i] * _headOffset.Length();
            }
        }

        private void Eat(Food food)
        {
            if (_headOffset.X > 0)
                _body.Add(new Vector2(0, _grid.Size.Height / 2));
            else if (_headOffset.X < 0)
                _body.Add(new Vector2(_grid.Size.Width - 1, _grid.Size.Height / 2));
            else if (_headOffset.Y > 0)
                _body.Add(new Vector2(_grid.Size.Width / 2, 0));
            else
                _body.Add(new Vector2(_grid.Size.Width / 2, _grid.Size.Height - 1));

            food.RandCoordinates();
        }

        private void CheckPosition(Vector2 position)
        {
            Vector2 projection = _grid.Project(position);

            if (_grid.GetObjectInPosition(projection, this) is Food food)
                Eat(food);

            if (IsDied())
                IsCrashed = true;
        }

        private bool IsDied()
        {
            Vector2 headProjection = _grid.Project(_head);

            for (var i = 1; i < _body.Count; i++)
            {
                if (headProjection.EqualsRound(_grid.Project(_body[i])))
                    return true;
            }

            return false;
        }
    }
}
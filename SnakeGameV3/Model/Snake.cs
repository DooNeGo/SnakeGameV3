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

            _headOffset = nextPosition - _head;
            Vector2 offset = _head - _body[0];

            _head = nextPosition;
            _body[0] += offset * _headOffset.Length();

            for (var i = 1; i < _body.Count; i++)
            {
                offset = _body[i - 1] - _body[i];
                _body[i] += offset * _headOffset.Length();
            }
        }

        public IEnumerator<Vector2> GetEnumerator()
        {
            yield return _head.GetProjectionOnTheGrid(_grid);

            foreach (Vector2 position in _body)
                yield return position.GetProjectionOnTheGrid(_grid);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            yield return _head;

            foreach (Vector2 position in _body)
                yield return position;
        }

        IEnumerator<ValueTuple<Vector2, ConsoleColor>> IEnumerable<ValueTuple<Vector2, ConsoleColor>>.GetEnumerator()
        {
            yield return new ValueTuple<Vector2, ConsoleColor>(_head.GetProjectionOnTheGrid(_grid), HeadColor);

            foreach (Vector2 position in _body)
                yield return new ValueTuple<Vector2, ConsoleColor>(position.GetProjectionOnTheGrid(_grid), BodyColor);
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
            Vector2 projection = position.GetProjectionOnTheGrid(_grid);

            if (_grid.GetObjectInPosition(projection, this) is Food food)
                Eat(food);

            if (IsDied())
                IsCrashed = true;
        }

        private bool IsDied()
        {
            for (var i = 1; i < _body.Count; i++)
            {
                if (MathF.Round(_head.X) == MathF.Round(_body[i].X) && MathF.Round(_head.Y) == MathF.Round(_body[i].Y))
                    return true;
            }

            return false;
        }
    }
}
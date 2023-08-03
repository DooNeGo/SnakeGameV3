using SnakeGameV3.Interfaces;
using System.Collections;
using System.Diagnostics;
using System.Numerics;

namespace SnakeGameV3.Model
{
    internal class Snake : IMovable, IGridObject, IRenderable
    {
        public event Action? Die;

        private readonly Stopwatch _stopwatch = Stopwatch.StartNew();
        private readonly List<Vector2> _body = new();
        private readonly Grid _grid;

        private Vector2 _head;
        private Vector2 _headDirection;

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

        public bool IsCollidable => true;

        public long DeltaTime => _stopwatch.ElapsedMilliseconds;

        public bool IsCrashed { get; private set; } = false;

        //public AnimationType AnimationType => AnimationType.Snake;

        public void MoveToPosition(Vector2 nextPosition)
        {
            _stopwatch.Restart();

            if (nextPosition == _head)
                return;

            CheckPosition(nextPosition);

            _headDirection = nextPosition - _head;
            Vector2 direction = _head - _body[0];

            if (direction.X > 1 || direction.Y > 1)
                direction = Vector2.Normalize(direction);

            _head = nextPosition;
            _body[0] += direction * _headDirection.Length();

            for (var i = 1; i < _body.Count; i++)
            {
                direction = _body[i - 1] - _body[i];
                _body[i] += direction * _headDirection.Length();
            }
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

        private void Eat(Food food)
        {
            if (_headDirection.X > 0)
                _body.Add(new Vector2(0, _grid.Size.Height / 2));
            else if (_headDirection.X < 0)
                _body.Add(new Vector2(_grid.Size.Width - 1, _grid.Size.Height / 2));
            else if (_headDirection.Y > 0)
                _body.Add(new Vector2(_grid.Size.Width / 2, 0));
            else
                _body.Add(new Vector2(_grid.Size.Width / 2, _grid.Size.Height - 1));

            food.RandCoordinates();
        }

        private void CheckPosition(Vector2 position)
        {
            if (!_grid.IsOccupiedCell(position))
                return;

            Cell[,] positionCells = _grid.GetCells(position);

            if (IsDied(positionCells))
            {
                IsCrashed = true;
                Die?.Invoke();
            }

            foreach (Cell cell in positionCells)
            {
                if (cell.Boss is Food food)
                {
                    Eat(food);
                    break;
                }
            }
        }

        private bool IsDied(Cell[,] positionCells)
        {
            foreach(Cell cell in positionCells)
                if (_grid.IsOccupiedCell(Position) && cell.Boss is not Snake && cell.IsCollidable == true)
                    return true;

            for (var i = 1; i < _body.Count; i++)
            {
                if (Math.Round(_head.X) == Math.Round(_body[i].X) && Math.Round(_head.Y) == Math.Round(_body[i].Y))
                    return true;
            }

            return false;
        }
    }
}
using SnakeGameV3.Interfaces;
using System.Collections;
using System.Numerics;

namespace SnakeGameV3.Model
{
    internal class Snake : IMovable, IGridObject, IRenderable
    {
        private readonly List<Vector2> _body = new();
        private readonly Grid _grid;

        private readonly TextureInfo _headTextureInfo;
        private readonly TextureInfo _bodyTextureInfo;

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

            _headTextureInfo = new TextureInfo(TextureName.SnakeHead, Scale);
            _bodyTextureInfo = new TextureInfo(TextureName.SnakeBody, Scale);

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

        public float Scale => 1f;

        public void MoveToPosition(Vector2 nextPosition)
        {
            _lastMoveTime = DateTime.UtcNow;

            if (nextPosition == _head)
                return;

            CheckPosition(nextPosition);

            ApplyOffsets(CalculateOffsets(nextPosition));

            _head = nextPosition;
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
            Vector2 tailProjection = _grid.Project(_body[^1]);
            Vector2 offset = new(_body[^1].X - tailProjection.X, _body[^1].Y - tailProjection.Y);
            Vector2 projectionOnTheEdge = _grid.GetTheClosestProjectionOnTheEdge(tailProjection);

            _body.Add(projectionOnTheEdge + offset);

            food.RandCoordinates();
        }

        private void CheckPosition(Vector2 position)
        {
            Vector2 projection = _grid.Project(position);
            object? entityInPosition = _grid.GetObjectInPosition(projection, this);

            if (entityInPosition is Food food)
                Eat(food);

            if (IsDied() || entityInPosition is ICollidable entity && entity.IsCollidable)
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

        IEnumerator<(Vector2, ConsoleColor, TextureInfo)> IEnumerable<(Vector2, ConsoleColor, TextureInfo)>.GetEnumerator()
        {
            Vector2 projection = _grid.Project(_head);

            yield return new(_grid.GetAbsolutePosition(projection),
                             HeadColor,
                             _headTextureInfo);

            foreach (Vector2 position in _body)
            {
                projection = _grid.Project(position);

                yield return new(_grid.GetAbsolutePosition(projection),
                                 BodyColor,
                                 _bodyTextureInfo);
            }
        }

        public IEnumerator<Vector2> GetEnumerator()
        {
            Vector2 projection = _grid.Project(_head);
            yield return projection;

            foreach (Vector2 position in _body)
            {
                projection = _grid.Project(position);
                yield return projection;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            Vector2 projection = _grid.Project(_head);
            yield return projection;

            foreach (Vector2 position in _body)
            {
                projection = _grid.Project(position);
                yield return projection;
            }
        }
    }
}
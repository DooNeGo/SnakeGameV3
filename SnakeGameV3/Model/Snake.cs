using SnakeGameV3.Interfaces;
using SnakeGameV3.Texturing;
using System.Collections;
using System.Numerics;

namespace SnakeGameV3.Model
{
    internal class Snake : IMovable, IGridObject, IRenderable
    {
        private readonly List<GameObject> _body = new();
        private readonly Grid _grid;

        private readonly TextureConfig _headTextureConfig;
        private readonly TextureConfig _bodyTextureConfig;

        private readonly ColliderConfig _headColliderConfig;
        private readonly ColliderConfig _bodyColliderConfig;

        private DateTime _lastMoveTime;

        public Snake(Vector2 startPosition, ConsoleColor headColor, ConsoleColor bodyColor, float speed, Grid grid)
        {
            MoveSpeed = speed;
            _grid = grid;
            var headPosition = new Vector2(startPosition.X, startPosition.Y);

            _headTextureConfig = new TextureConfig(TextureName.SnakeHead, Scale, headColor);
            _bodyTextureConfig = new TextureConfig(TextureName.SnakeBody, Scale, bodyColor);

            _headColliderConfig = new ColliderConfig(ColliderType.Circle, Scale);
            _bodyColliderConfig = new ColliderConfig(ColliderType.Circle, Scale);

            _body.Add(new GameObject(headPosition, _headTextureConfig, ColliderType.Circle));
            _body.Add(new GameObject(headPosition with { X = headPosition.X - 1 * Scale }, _bodyTextureConfig, ColliderType.Circle));
            _body.Add(new GameObject(headPosition with { X = headPosition.X - 2 * Scale }, _bodyTextureConfig, ColliderType.Circle));
        }

        public Vector2 Position => Head.Position;

        public float MoveSpeed { get; }

        public TimeSpan DeltaTime => DateTime.UtcNow - _lastMoveTime;

        public bool IsDied { get; private set; } = false;

        public float Scale => 1f;

        public bool IsNeedToProject => true;

        private GameObject Head => _body[0];

        public void MoveTo(Vector2 nextPosition)
        {
            _lastMoveTime = DateTime.UtcNow;

            if (nextPosition == Position)
                return;

            CheckPosition(nextPosition);
            ApplyOffsets(CalculateOffsets(nextPosition));
        }

        private Vector2[] CalculateOffsets(Vector2 nextPosition)
        {
            var offsets = new Vector2[_body.Count];

            offsets[0] = nextPosition - Position;

            for (var i = 1; i < _body.Count; i++)
            {
                offsets[i] = _body[i - 1].Position - _body[i].Position;
            }

            return offsets;
        }

        private void ApplyOffsets(Vector2[] offsets)
        {
            Head.Position += offsets[0];

            for (var i = 1; i < offsets.Length; i++)
            {
                _body[i].Position += offsets[i] * offsets[0].Length() / Scale;
            }
        }

        private void Eat(Food food)
        {
            Vector2 tailProjection = _grid.Project(_body[^1].Position);
            Vector2 offset = new(_body[^1].Position.X - tailProjection.X, _body[^1].Position.Y - tailProjection.Y);
            Vector2 projectionOnTheEdge = _grid.GetTheClosestProjectionOnTheEdge(tailProjection);

            _body.Add(new GameObject(projectionOnTheEdge + offset, _bodyTextureConfig, ColliderType.Square));

            food.RandCoordinates();
        }

        private void CheckPosition(Vector2 position)
        {
            Vector2 projection = _grid.Project(position);
            IEnumerator<IGridObjectPart> enumerator = _grid.GetEachObjectInPosition(projection, Head);

            while (enumerator.MoveNext())
            {
                if (enumerator.Current is Food food)
                {
                    Eat(food);
                    break;
                }

                if (enumerator.Current != _body[1])
                {
                    IsDied = true;
                    break;
                }
            }
        }

        public IEnumerator<IReadOnlyGameObject> GetEnumerator()
        {
            return _body.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _body.GetEnumerator();
        }

        IEnumerator<IGridObjectPart> IEnumerable<IGridObjectPart>.GetEnumerator()
        {
            return _body.GetEnumerator();
        }
    }
}
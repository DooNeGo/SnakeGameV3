using SnakeGameV3.Interfaces;
using SnakeGameV3.Texturing;
using System.Numerics;

namespace SnakeGameV3.Model
{
    internal class Snake : IMovable, ICompositeObject
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

            _headTextureConfig = new TextureConfig(TextureName.SnakeHead, headColor);
            _bodyTextureConfig = new TextureConfig(TextureName.SnakeBody, bodyColor);

            _headColliderConfig = new ColliderConfig(ColliderType.Circle);
            _bodyColliderConfig = new ColliderConfig(ColliderType.Circle);

            GameObject head = new(startPosition);
            GameObject bodyPart1 = new(head.Position with { X = head.Position.X - 1 * Scale });
            GameObject bodyPart2 = new(head.Position with { X = head.Position.X - 2 * Scale });

            head.AddComponent(_headTextureConfig);
            head.AddComponent(_headColliderConfig);
            bodyPart1.AddComponent(_bodyTextureConfig);
            bodyPart2.AddComponent(_bodyTextureConfig);
            bodyPart2.AddComponent(_bodyColliderConfig);

            _body.Add(head);
            _body.Add(bodyPart1);
            _body.Add(bodyPart2);
        }

        public Vector2 Position => Head.Position;

        public float MoveSpeed { get; }

        public TimeSpan DeltaTime => DateTime.UtcNow - _lastMoveTime;

        public bool IsDied { get; private set; } = false;

        public float Scale { get; private set; } = 1.0f;

        public bool IsNeedToProject => true;

        private GameObject Head => _body[0];

        public void MoveTo(Vector2 nextPosition)
        {
            _lastMoveTime = DateTime.UtcNow;

            if (nextPosition == Position)
                return;

            CheckPosition(nextPosition);
            ApplyOffsets(CalculateOffsets(nextPosition));

            for (var i = 3; i < _body.Count; i++)
            {
                if (_body[i].GetComponent<ColliderConfig>() is null
                    && Vector2.Distance(_body[i].Position, _body[i - 1].Position) <= 0.8f * Scale)
                {
                    _body[i].AddComponent(_bodyColliderConfig);
                }
            }
        }

        private Vector2[] CalculateOffsets(Vector2 nextPosition)
        {
            var offsets = new Vector2[_body.Count];

            offsets[0] = nextPosition - Position;

            for (var i = 1; i < _body.Count; i++)
            {
                offsets[i] = _body[i - 1].Position - _body[i].Position;
                offsets[i] /= Scale;
            }

            return offsets;
        }

        private void ApplyOffsets(Vector2[] offsets)
        {
            Head.Position += offsets[0];

            for (var i = 1; i < offsets.Length; i++)
            {
                _body[i].Position += offsets[i] * offsets[0].Length();
            }
        }

        private void Eat(Food food)
        {
            Vector2 tailProjection = _grid.Project(_body[^1].Position);
            Vector2 offset = new(_body[^1].Position.X - tailProjection.X, _body[^1].Position.Y - tailProjection.Y);
            Vector2 projectionOnTheEdge = _grid.GetTheClosestProjectionOnTheEdge(tailProjection);

            GameObject newBodyPart = new(projectionOnTheEdge + offset);

            newBodyPart.AddComponent(_bodyTextureConfig);

            _body.Add(newBodyPart);

            food.RandCoordinates();
        }

        private void CheckPosition(Vector2 position)
        {
            Vector2 projection = _grid.Project(position);
            IEnumerator<IReadOnlyGameObject> enumerator = _grid.GetEachObjectInPosition(projection, Head, Scale);

            while (enumerator.MoveNext())
            {
                if (enumerator.Current is Food food)
                {
                    Eat(food);
                    break;
                }
                else
                {
                    IsDied = true;
                    break;
                }
            }
        }

        public IEnumerator<IReadOnlyGameObject> GetGameObjectsWithComponent<T>()
        {
            foreach (IReadOnlyGameObject gameObject in _body)
            {
                if (gameObject.GetComponent<T>() != null)
                    yield return gameObject;
            }
        }
    }
}
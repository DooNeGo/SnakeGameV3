using SnakeGameV3.Components;
using SnakeGameV3.Components.Colliders;
using SnakeGameV3.Interfaces;
using SnakeGameV3.Texturing;
using System.Collections;
using System.Numerics;

namespace SnakeGameV3.Model
{
    internal class Snake : IMovable, ICompositeObject
    {
        private readonly List<GameObject> _body = new();
        private readonly Grid _grid;
        private readonly ConsoleColor _headColor;
        private readonly ConsoleColor _bodyColor;

        private float _scale = 1f;
        private int _lastColliderIndex;
        private DateTime _lastMoveTime;

        public Snake(Vector2 startPosition, ConsoleColor headColor, ConsoleColor bodyColor, float speed, Grid grid)
        {
            MoveSpeed = speed;
            _grid = grid;
            _lastColliderIndex = 2;
            _headColor = headColor;
            _bodyColor = bodyColor;

            GameObject head = new();
            Transform headTransform = head.AddComponent<Transform>();
            Collider headCollider = head.AddComponent<CircleCollider>();
            TextureConfig headTexture = head.AddComponent<TextureConfig>();
            headTransform.Position = startPosition;
            headTransform.Scale = Scale;
            headCollider.CollisionEntry += OnCollisionEnter;
            headTexture.Color = _headColor;
            headTexture.Name = TextureName.SnakeHead;

            GameObject bodyPart1 = new();
            Transform bodyPart1Transform = bodyPart1.AddComponent<Transform>();
            TextureConfig bodyPart1Texture = bodyPart1.AddComponent<TextureConfig>();
            bodyPart1Transform.Position = headTransform.Position - Vector2.UnitX * Scale;
            bodyPart1Transform.Scale = Scale;
            bodyPart1Texture.Color = _bodyColor;
            bodyPart1Texture.Name = TextureName.SnakeBody;

            GameObject bodyPart2 = new();
            Transform bodyPart2Transform = bodyPart2.AddComponent<Transform>();
            TextureConfig bodyPart2Texture = bodyPart2.AddComponent<TextureConfig>();
            bodyPart2.AddComponent<CircleCollider>();
            bodyPart2Transform.Position = bodyPart1Transform.Position - Vector2.UnitX * Scale;
            bodyPart2Transform.Scale = Scale;
            bodyPart2Texture.Color = _bodyColor;
            bodyPart2Texture.Name = TextureName.SnakeBody;

            _body.Add(head);
            _body.Add(bodyPart1);
            _body.Add(bodyPart2);
        }

        public Vector2 Position => Head.GetComponent<Transform>().Position;

        public float MoveSpeed { get; private set; }

        public TimeSpan DeltaTime => DateTime.UtcNow - _lastMoveTime;

        public bool IsDied { get; private set; } = false;

        public int Score { get; private set; } = 0;

        private float Scale
        {
            get { return _scale; }
            set
            {
                _scale = value;
                foreach (GameObject body in _body)
                {
                    body.GetComponent<Transform>().Scale = _scale;
                }
            }
        }

        private GameObject Head => _body[0];

        public void MoveTo(Vector2 nextPosition)
        {
            _lastMoveTime = DateTime.UtcNow;

            if (nextPosition == Position)
                return;

            ApplyOffsets(CalculateOffsets(nextPosition));
            CheckColliders();
        }

        private void CheckColliders()
        {
            for (int i = _lastColliderIndex + 1; i < _body.Count; i++)
            {
                Transform transform1 = _body[i].GetComponent<Transform>();
                Transform transform2 = _body[i - 1].GetComponent<Transform>();

                if (_body[i].TryGetComponent<Collider>() is null
                    && Vector2.Distance(transform1.Position, transform2.Position) <= 0.6f * Scale)
                {
                    _body[i].AddComponent<CircleCollider>();
                    _lastColliderIndex = i;
                }
            }
        }

        private Vector2[] CalculateOffsets(Vector2 nextPosition)
        {
            var offsets = new Vector2[_body.Count];

            offsets[0] = nextPosition - Position;

            for (var i = 1; i < _body.Count; i++)
            {
                Transform transform1 = _body[i].GetComponent<Transform>();
                Transform transform2 = _body[i - 1].GetComponent<Transform>();

                offsets[i] = transform2.Position - transform1.Position;
                offsets[i] /= Scale;
            }

            return offsets;
        }

        private void ApplyOffsets(Vector2[] offsets)
        {
            Head.GetComponent<Transform>().Position += offsets[0];

            for (var i = 1; i < offsets.Length; i++)
            {
                _body[i].GetComponent<Transform>().Position += offsets[i] * offsets[0].Length();
            }
        }

        private void OnCollisionEnter(GameObject gameObject)
        {
            if (gameObject is Food food)
                Eat(food);
            else
                IsDied = true;
        }

        private void Eat(Food food)
        {
            float effectValue = food.GetComponent<Effect>().Value;

            switch (food.GetComponent<Effect>().Type)
            {

                case EffectType.Speed:
                    if (MoveSpeed + effectValue > 2)
                        MoveSpeed += effectValue;
                    break;

                case EffectType.Scale:
                    if (Scale + effectValue > 0.5f)
                        Scale += effectValue;
                    break;

                case EffectType.Length:
                    if (effectValue > 0)
                        AddNewBodyPart();
                    else if (_body.Count - 1 > 2)
                        _body.RemoveAt(_body.Count - 1);
                    break;
            }

            Score++;
        }

        private void AddNewBodyPart()
        {
            Transform tailTransform = _body[^1].GetComponent<Transform>();

            Vector2 tailProjection = _grid.Project(tailTransform.Position);
            Vector2 offset = new(tailTransform.Position.X - tailProjection.X, tailTransform.Position.Y - tailProjection.Y);
            Vector2 projectionOnTheEdge = _grid.GetTheClosestProjectionOnTheEdge(tailProjection);

            GameObject newBodyPart = new();

            Transform newBodyPartTransform = newBodyPart.AddComponent<Transform>();
            newBodyPartTransform.Position = projectionOnTheEdge + offset;
            newBodyPartTransform.Scale = Scale;

            TextureConfig newBodyPartTexture = newBodyPart.AddComponent<TextureConfig>();
            newBodyPartTexture.Color = _bodyColor;
            newBodyPartTexture.Name = TextureName.SnakeBody;

            _body.Add(newBodyPart);
        }

        private GameObject CloneWithProjection(GameObject gameObject)
        {
            Transform bodyTransform = gameObject.GetComponent<Transform>();
            Collider? bodyCollider = gameObject.TryGetComponent<Collider>();
            TextureConfig bodyTexture = gameObject.GetComponent<TextureConfig>();

            GameObject clone = new();

            Transform transform = clone.AddComponent<Transform>();
            transform.Rotation = bodyTransform.Rotation;
            transform.Position = _grid.Project(bodyTransform.Position);
            transform.Scale = bodyTransform.Scale;

            if (bodyCollider is not null)
            {
                Collider collider = bodyCollider switch
                {
                    BoxCollider => clone.AddComponent<BoxCollider>(),
                    CircleCollider => clone.AddComponent<CircleCollider>(),
                    _ => throw new NotImplementedException()
                };

                if (gameObject == Head)
                    collider.CollisionEntry += OnCollisionEnter;
            }

            TextureConfig texture = clone.AddComponent<TextureConfig>();
            texture.Color = bodyTexture.Color;
            texture.Name = bodyTexture.Name;

            return clone;
        }

        public IEnumerator<GameObject> GetGameObjectsWithComponent<T>() where T : Component
        {
            for (var i = 0; i < _body.Count; i++)
            {
                if (_body[i].TryGetComponent<T>() is not null)
                {
                    yield return CloneWithProjection(_body[i]);
                }
            }
        }

        public IEnumerator<GameObject> GetEnumerator()
        {
            for (int i = 0; i < _body.Count; i++)
            {
                yield return CloneWithProjection(_body[i]);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            for (int i = 0; i < _body.Count; i++)
            {
                yield return CloneWithProjection(_body[i]);
            }
        }
    }
}
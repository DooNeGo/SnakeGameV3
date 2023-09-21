using SnakeGameV3.Components;
using SnakeGameV3.Texturing;
using System.Collections;
using System.Drawing;
using System.Numerics;

namespace SnakeGameV3.Model
{
    internal class Grid : IEnumerable<GameObject>
    {
        private const float Scale = 1.0f;

        private readonly bool[,] _cells;
        private readonly GameObject[,] _gameObjects;

        public Grid(Size screenSize, Size cellSize)
        {
            CellSize = cellSize;
            Size = new Size(screenSize.Width / CellSize.Width, screenSize.Height / CellSize.Height);
            _cells = new bool[Size.Height, Size.Width];
            Center = new Vector2((Size.Width - 1) / 2f, (Size.Height - 1) / 2f);
            _gameObjects = new GameObject[Size.Height, Size.Width];
            InitializeGameObjects();
        }

        public Size Size { get; }

        public Size CellSize { get; }

        public Vector2 Center { get; }

        public Scene? ActiveScene { get; set; }

        public bool IsPositionOccupied(Vector2 position, float scale)
        {
            if (position.X >= _cells.GetLength(1)
                || position.Y >= _cells.GetLength(0)
                || position.X < 0
                || position.Y < 0)
                return false;

            Update();

            return _cells[(int)MathF.Round(position.Y), (int)MathF.Round(position.X)];
        }

        private void Update()
        {
            Clear();

            if (ActiveScene is null)
                throw new NullReferenceException(nameof(ActiveScene));

            foreach (GameObject gameObject in ActiveScene)
            {
                TryAddToGrid(gameObject);
            }
        }

        public Vector2 Project(Vector2 position)
        {
            Vector2 projection = new(position.X % (Size.Width - 1), position.Y % (Size.Height - 1));

            if (projection.X < 0)
                projection.X += Size.Width - 1;

            if (projection.Y < 0)
                projection.Y += Size.Height - 1;

            return projection;
        }

        public Vector2 GetTheClosestProjectionOnTheEdge(Vector2 position)
        {
            Vector2 projectionToUpperEdge = position with { Y = 0 };
            Vector2 projectionToLeftEdge = position with { X = 0 };
            Vector2 projectionToRightEdge = position with { X = Size.Width - 1 };
            Vector2 projectionToBottomEdge = position with { Y = Size.Height - 1 };

            float distanceToUpperEdge = Vector2.Distance(position, projectionToUpperEdge);
            float distanceToLeftEdge = Vector2.Distance(position, projectionToLeftEdge);
            float distanceToRightEdge = Size.Width - distanceToLeftEdge;
            float distanceToBottomEdge = Size.Height - distanceToUpperEdge;

            ValueTuple<float, Vector2>[] distancesWithProjections =
            {
                new(distanceToUpperEdge, projectionToUpperEdge),
                new(distanceToLeftEdge, projectionToLeftEdge),
                new(distanceToRightEdge, projectionToRightEdge),
                new(distanceToBottomEdge, projectionToBottomEdge),
            };

            return distancesWithProjections.Min().Item2;
        }

        public Vector2 GetAbsolutePosition(Vector2 relativePosition, float scale)
        {
            Vector2 offset = new(CellSize.Width * scale - CellSize.Width,
                                 CellSize.Height * scale - CellSize.Height);
            offset /= 2;

            return new(relativePosition.X * CellSize.Width - offset.X,
                       relativePosition.Y * CellSize.Height - offset.Y);
        }

        private void TryAddToGrid(GameObject gameObject)
        {
            Transform? transform = gameObject.TryGetComponent<Transform>();

            if (transform is null
                || transform.Position.X >= _cells.GetLength(1)
                || transform.Position.Y >= _cells.GetLength(0)
                || transform.Position.X < 0
                || transform.Position.Y < 0)
                return;

            _cells[(int)transform.Position.Y, (int)transform.Position.X] = true;
        }

        private void Clear()
        {
            for (var y = 0; y < _cells.GetLength(0); y++)
            {
                for (var x = 0; x < _cells.GetLength(1); x++)
                {
                    if (_cells[y, x] is true)
                        _cells[y, x] = false;
                }
            }
        }

        private void InitializeGameObjects()
        {
            for (var y = 0; y < Size.Height; y++)
            {
                for (var x = 0; x < Size.Width; x++)
                {
                    _gameObjects[y, x] = new GameObject();

                    Transform transform = _gameObjects[y, x].AddComponent<Transform>();
                    transform.Position = new Vector2(x, y);
                    transform.Scale = Scale;

                    TextureConfig textureConfig = _gameObjects[y, x].AddComponent<TextureConfig>();
                    textureConfig.Name = TextureName.Grid;
                    textureConfig.Color = ConsoleColor.White;
                }
            }
        }

        public IEnumerator<GameObject> GetEnumerator()
        {
            foreach (GameObject gameObject in _gameObjects)
            {
                yield return gameObject;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _gameObjects.GetEnumerator();
        }
    }
}
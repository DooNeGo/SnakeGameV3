using SnakeGameV3.Interfaces;
using SnakeGameV3.Texturing;
using System.Drawing;
using System.Numerics;

namespace SnakeGameV3.Model
{
    internal class Grid : ICompositeObject
    {
        private readonly List<ICompositeObject> _compositeObjects = new();
        private readonly bool[,] _cells;
        private readonly GameObject[,] _gameObjectCells;
        private readonly TextureConfig _textureConfig;

        public Grid(Size screenSize, Size cellSize)
        {
            CellSize = cellSize;
            Size = new Size(screenSize.Width / CellSize.Width, screenSize.Height / CellSize.Height);
            _cells = new bool[Size.Height, Size.Width];
            Center = new Vector2((Size.Width - 1) / 2f, (Size.Height - 1) / 2f);
            _gameObjectCells = new GameObject[Size.Height, Size.Width];
            _textureConfig = new TextureConfig(TextureName.Grid, ConsoleColor.White);
            InitializeGameObjects();
        }

        public Size Size { get; }

        public Size CellSize { get; }

        public Vector2 Center { get; }

        public bool IsNeedToProject => false;

        public float Scale => 1f;

        public bool IsPositionOccupied(Vector2 position, float scale)
        {
            if (scale <= 1)
                return _cells[(int)position.Y, (int)position.X];
            return false;
        }

        public void Update()
        {
            Clear();

            foreach (ICompositeObject compositeObject in _compositeObjects)
            {
                IEnumerator<IReadOnlyGameObject> enumerator = compositeObject.GetGameObjectsWithComponent<Collider>();

                while (enumerator.MoveNext())
                {
                    AddToGrid(enumerator.Current);
                }
            }
        }

        public void Add(ICompositeObject compositeObject)
        {
            _compositeObjects.Add(compositeObject);
        }

        public void Remove(ICompositeObject compositeObject)
        {
            _compositeObjects.Remove(compositeObject);
        }

        public Vector2 Project(Vector2 position)
        {
            Vector2 projection = new(position.X % Size.Width, position.Y % Size.Height);

            if (position.X < 0)
                projection.X += Size.Width - 1;

            if (position.Y < 0)
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
            offset /= 2f;
            return new(relativePosition.X * CellSize.Width - offset.X,
                       relativePosition.Y * CellSize.Height - offset.Y);
        }

        public IEnumerator<IReadOnlyGameObject> GetGameObjectsWithComponent<T>()
        {
            foreach (GameObject gameObject in _gameObjectCells)
            {
                if (gameObject.GetComponent<T>() is not null)
                    yield return gameObject;
            }
        }

        private void AddToGrid(IReadOnlyGameObject gameObject)
        {
            _cells[(int)gameObject.Position.Y, (int)gameObject.Position.X] = true;
        }

        private void Clear()
        {
            for (var y = 0; y < _cells.GetLength(0); y++)
            {
                for (var x = 0; x < _cells.GetLength(1); x++)
                {
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
                    _gameObjectCells[y, x] = new GameObject(new Vector2(x, y), Scale);
                    _gameObjectCells[y, x].AddComponent(_textureConfig);
                }
            }
        }
    }
}

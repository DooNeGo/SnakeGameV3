using SnakeGameV3.Interfaces;
using SnakeGameV3.Texturing;
using System.Drawing;
using System.Numerics;

namespace SnakeGameV3.Model
{
    internal class Grid : ICompositeObject
    {
        private const ColliderType defaultColliderType = ColliderType.Square;

        private readonly List<ICompositeObject> _compositeObjects = new();
        private readonly CollidersDatabase _collidersDatabase;
        private readonly Cell[,] _cells;
        private readonly GameObject[,] _gameObjectCells;
        private readonly TextureConfig _textureConfig;


        public Grid(Size screenSize, Size cellSize)
        {
            CellSize = cellSize;
            Size = new Size(screenSize.Width / CellSize.Width, screenSize.Height / CellSize.Height);
            _cells = new Cell[screenSize.Height, screenSize.Width];
            Center = new Vector2((Size.Width - 1) / 2f, (Size.Height - 1) / 2f);
            _collidersDatabase = new CollidersDatabase(this);
            _gameObjectCells = new GameObject[Size.Height, Size.Width];
            _textureConfig = new TextureConfig(TextureName.Grid, ConsoleColor.White);
            InitializeCells();
            InitializeGameObjects();
        }

        public Size Size { get; }

        public Size CellSize { get; }

        public Vector2 Center { get; }

        public bool IsNeedToProject => false;

        public float Scale => 1f;

        public bool IsPositionOccupied(Vector2 position, IReadOnlyGameObject? requester, float scale)
        {
            bool isOccupied = false;

            ForEachCellInPosition(position, requester, scale, cell =>
            {
                if (cell.Boss is not null)
                {
                    isOccupied = true;
                    return;
                }
            });

            return isOccupied;
        }

        public IEnumerator<IReadOnlyGameObject> GetEachObjectInPosition(Vector2 position, IReadOnlyGameObject? requester, float scale)
        {
            List<IReadOnlyGameObject> entities = new(3);
            IEnumerator<Cell> enumerator = GetEachCellInPosition(position, requester, scale);

            if (requester is not null)
                entities.Add(requester);

            while (enumerator.MoveNext())
            {
                if (enumerator.Current.Boss is null
                    || entities.Any(entity => entity == enumerator.Current.Boss))
                    continue;

                entities.Add(enumerator.Current.Boss);
                yield return enumerator.Current.Boss;
            }
        }

        public void Update()
        {
            Clear();

            foreach (ICompositeObject compositeObject in _compositeObjects)
            {
                IEnumerator<IReadOnlyGameObject> enumerator = compositeObject.GetGameObjectsWithComponent<ColliderConfig>();

                while (enumerator.MoveNext())
                {
                    IReadOnlyGameObject gameObject = enumerator.Current;
                    Vector2 position = gameObject.Position;

                    if (compositeObject.IsNeedToProject)
                    {
                        position = Project(position);
                    }

                    AddToGrid(position, gameObject, compositeObject.Scale);
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
            var projection = new Vector2(position.X % Size.Width, position.Y % Size.Height);

            if (position.X < 0)
            {
                projection.X += Size.Width - 1;
            }
            if (position.Y < 0)
            {
                projection.Y += Size.Height - 1;
            }

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

        private void AddToGrid(Vector2 position, IReadOnlyGameObject entity, float scale)
        {
            ForEachCellInPosition(position, entity, scale, cell =>
            {
                cell.Occupy(entity);
            });
        }

        private void InitializeCells()
        {
            for (var y = 0; y < _cells.GetLength(0); y++)
            {
                for (var x = 0; x < _cells.GetLength(1); x++)
                {
                    _cells[y, x] = new Cell();
                }
            }
        }

        private void Clear()
        {
            for (var y = 0; y < _cells.GetLength(0); y++)
            {
                for (var x = 0; x < _cells.GetLength(1); x++)
                {
                    _cells[y, x].Clear();
                }
            }
        }

        private IEnumerator<Cell> GetEachCellInPosition(Vector2 relativePosition, IReadOnlyGameObject? requester, float scale = 1f)
        {
            ColliderConfig colliderConfig = requester switch
            {
                null => new ColliderConfig(defaultColliderType),
                _ => requester.GetComponent<ColliderConfig>()
                ?? new ColliderConfig(defaultColliderType),
            };

            Collider collider = _collidersDatabase.GetTransformedCollider(colliderConfig, scale);
            Vector2 absolutePosition = GetAbsolutePosition(relativePosition, scale);

            for (var y = 0; y < collider.Size.Height; y++)
            {
                int positionY = (int)(absolutePosition.Y + y);

                for (var x = 0; x < collider.Size.Width; x++)
                {
                    int positionX = (int)(absolutePosition.X + x);

                    if (positionY >= _cells.GetLength(0)
                        || positionX >= _cells.GetLength(1)
                        || positionX < 0
                        || positionY < 0
                        || !collider.GetValue(x, y))
                        continue;

                    yield return _cells[positionY, positionX];
                }
            }
        }

        private void ForEachCellInPosition(Vector2 relativePosition, IReadOnlyGameObject? requester, float scale, Action<Cell> action)
        {
            IEnumerator<Cell> enumerator = GetEachCellInPosition(relativePosition, requester, scale);

            while (enumerator.MoveNext())
            {
                action(enumerator.Current);
            }
        }

        public IEnumerator<IReadOnlyGameObject> GetGameObjectsWithComponent<T>()
        {
            foreach (GameObject gameObject in _gameObjectCells)
            {
                if (gameObject.GetComponent<T>() is not null)
                    yield return gameObject;
            }
        }

        private void InitializeGameObjects()
        {
            for (var y = 0; y < Size.Height; y++)
            {
                for (var x = 0; x < Size.Width; x++)
                {
                    _gameObjectCells[y, x] = new GameObject(new Vector2(x, y));
                    _gameObjectCells[y, x].AddComponent(_textureConfig);
                }    
            }
        }

        private class Cell
        {
            public Cell()
            {
                Boss = null;
            }

            public IReadOnlyGameObject? Boss { get; private set; }

            public void Occupy(IReadOnlyGameObject entity)
            {
                Boss = entity;
            }

            public void Clear()
            {
                Boss = null;
            }
        }
    }
}

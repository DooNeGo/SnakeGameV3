using SnakeGameV3.Interfaces;
using System.Drawing;
using System.Numerics;

namespace SnakeGameV3.Model
{
    internal class Grid
    {
        private const float defaultScale = 1f;
        private const ColliderType defaultColliderType = ColliderType.Square;

        private readonly List<ICompositeObject> _gridObjects = new();
        private readonly CollidersDatabase _collidersDatabase;
        private readonly Cell[,] _cells;

        public Grid(Size screenSize, Size cellSize)
        {
            CellSize = cellSize;
            Size = new Size(screenSize.Width / CellSize.Width, screenSize.Height / CellSize.Height);
            _cells = new Cell[screenSize.Height, screenSize.Width];
            Center = new Vector2(Size.Width / 2f, Size.Height / 2f);
            _collidersDatabase = new CollidersDatabase(this);
            InitializeCells();
        }

        public Size Size { get; }

        public Size CellSize { get; }

        public Vector2 Center { get; }

        public bool IsPositionOccupied(Vector2 position, IReadOnlyGameObject? requester)
        {
            bool isOccupied = false;

            ForEachCellInPosition(position, requester, (cell) =>
            {
                if (cell.Boss is not null)
                {
                    isOccupied = true;
                    return;
                }
            });

            return isOccupied;
        }

        public IEnumerator<IReadOnlyGameObject> GetEachObjectInPosition(Vector2 position, IReadOnlyGameObject? requester)
        {
            List<IReadOnlyGameObject> entities = new(3);
            IEnumerator<Cell> enumerator = GetEachCellInPosition(position, requester);

            if (requester != null)
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

            foreach (ICompositeObject gridObject in _gridObjects)
            {
                IEnumerator<IReadOnlyGameObject> enumerator = gridObject.GetGameObjectsWithComponent<ColliderConfig>();

                while(enumerator.MoveNext())
                {
                    IReadOnlyGameObject gameObject = enumerator.Current;
                    Vector2 position = gameObject.Position;

                    if (gridObject.IsNeedToProject)
                    {
                        position = Project(position);
                    }

                    AddToGrid(position, gameObject);
                }
            }
        }

        public void Add(ICompositeObject gridObject)
        {
            _gridObjects.Add(gridObject);
        }

        public void Remove(ICompositeObject gridObject)
        {
            _gridObjects.Remove(gridObject);
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

        public Vector2 GetAbsolutePosition(Vector2 relativePosition)
        {
            return new(MathF.Round(relativePosition.X * CellSize.Width),
                       MathF.Round(relativePosition.Y * CellSize.Height));
        }

        private void AddToGrid(Vector2 position, IReadOnlyGameObject entity)
        {
            ForEachCellInPosition(position, entity, (cell) =>
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

        private IEnumerator<Cell> GetEachCellInPosition(Vector2 relativePosition, IReadOnlyGameObject? requester)
        {
            float scale;
            ColliderConfig colliderConfig;

            if (requester != null)
            {
                scale = requester.Scale;
                colliderConfig = requester.GetComponent<ColliderConfig>()!;
            }
            else
            {
                scale = defaultScale;
                colliderConfig = new(defaultColliderType);
            }

            Collider collider = _collidersDatabase.GetTransformedCollider(colliderConfig, scale);
            Vector2 absolutePosition = GetAbsolutePosition(relativePosition);

            for (var y = 0; y < collider.Size.Height; y++)
            {
                float positionY = absolutePosition.Y + y;

                for (var x = 0; x < collider.Size.Width; x++)
                {
                    float positionX = absolutePosition.X + x;

                    if (positionY >= _cells.GetLength(0)
                        || positionX >= _cells.GetLength(1)
                        || positionX < 0
                        || positionY < 0
                        || !collider.GetValue(x, y))
                        continue;

                    yield return _cells[(int)positionY, (int)positionX];
                }
            }
        }

        private void ForEachCellInPosition(Vector2 relativePosition, IReadOnlyGameObject? requester, Action<Cell> action)
        {
            IEnumerator<Cell> enumerator = GetEachCellInPosition(relativePosition, requester);

            while (enumerator.MoveNext())
            {
                action(enumerator.Current);
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

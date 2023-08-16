using SnakeGameV3.Interfaces;
using System.Drawing;
using System.Numerics;

namespace SnakeGameV3.Model
{
    internal class Grid
    {
        private const float defaultScale = 1f;

        private readonly List<IGridObject> _gridObjects = new();
        private readonly Cell[,] _cells;

        public Grid(Size screenSize, Size cellSize)
        {
            CellSize = cellSize;
            Size = new Size(screenSize.Width / CellSize.Width, screenSize.Height / CellSize.Height);
            _cells = new Cell[screenSize.Height, screenSize.Width];
            Center = new Vector2(Size.Width / 2f, Size.Height / 2f);
            InitializeCells();
        }

        public Size Size { get; }

        public Size CellSize { get; }

        public Vector2 Center { get; }

        public bool IsPositionOccupied(Vector2 position, IScalable? requester)
        {
            bool isOccupied = false;
            float scale = defaultScale;

            if (requester != null)
                scale = requester.Scale;

            ForEachCellInPosition(position, scale, (cell) =>
            {
                if (cell.Boss is not null)
                {
                    isOccupied = true;
                    return;
                }
            });

            return isOccupied;
        }

        public object? GetObjectInPosition(Vector2 position, IScalable? requester)
        {
            object? entity = null;
            float scale = defaultScale;

            if (requester != null)
                scale = requester.Scale;

            ForEachCellInPosition(position, scale, (cell) =>
            {
                if (cell.Boss != null
                && cell.Boss != requester)
                {
                    entity = cell.Boss;
                    return;
                }
            });

            return entity;
        }

        public IEnumerator<object> GetEachObjectInPosition(Vector2 position, IScalable? requester)
        {
            float scale = defaultScale;
            HashSet<object> entities = new();

            if (requester != null)
            {
                scale = requester.Scale;
                entities.Add(requester);
            }

            IEnumerator<Cell> enumerator = GetEachCellInPosition(position, scale);

            while (enumerator.MoveNext())
            {
                if (enumerator.Current.Boss is null
                    || entities.Contains(enumerator.Current.Boss))
                    continue;

                entities.Add(enumerator.Current.Boss);
                yield return enumerator.Current.Boss;
            }
        }

        public void Update()
        {
            Clear();

            foreach (IGridObject gridObject in _gridObjects)
            {
                foreach (Vector2 position in gridObject)
                {
                    AddToGrid(position, gridObject);
                }
            }
        }

        public void Add(IGridObject gridObject)
        {
            _gridObjects.Add(gridObject);
        }

        public void Remove(IGridObject gridObject)
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

            ValueTuple<float, Vector2>[] distancesWithOffsets =
            {
                new(distanceToUpperEdge, projectionToUpperEdge),
                new(distanceToLeftEdge, projectionToLeftEdge),
                new(distanceToRightEdge, projectionToRightEdge),
                new(distanceToBottomEdge, projectionToBottomEdge),
            };

            return distancesWithOffsets.Min().Item2;
        }

        public Vector2 GetAbsolutePosition(Vector2 relativePosition)
        {
            return new(MathF.Round(relativePosition.X * CellSize.Width),
                       MathF.Round(relativePosition.Y * CellSize.Height));
        }

        private void AddToGrid(Vector2 position, IGridObject entity)
        {
            ForEachCellInPosition(position, entity.Scale, (cell) =>
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
                    if (_cells[y, x].Boss is not null)
                    {
                        _cells[y, x].Clear();
                    }
                }
            }
        }

        private IEnumerator<Cell> GetEachCellInPosition(Vector2 relativePosition, float scale)
        {
            Vector2 absolutePosition = GetAbsolutePosition(relativePosition);

            for (var y = 0; y < CellSize.Height * scale; y++)
            {
                float positionY = absolutePosition.Y + y;

                for (var x = 0; x < CellSize.Width * scale; x++)
                {
                    float positionX = absolutePosition.X + x;

                    if (positionY >= _cells.GetLength(0)
                        || positionX >= _cells.GetLength(1)
                        || positionX < 0
                        || positionY < 0)
                        continue;

                    yield return _cells[(int)positionY, (int)positionX];
                }
            }
        }

        private void ForEachCellInPosition(Vector2 relativePosition, float scale, Action<Cell> action)
        {
            IEnumerator<Cell> enumerator = GetEachCellInPosition(relativePosition, scale);

            while (enumerator.MoveNext())
            {
                action(enumerator.Current);
            }
        }

        private struct Cell : ICollidable
        {
            public Cell()
            {
                IsCollidable = false;
                Boss = null;
            }

            public bool IsCollidable { get; private set; }

            public object? Boss { get; private set; }

            public void Occupy(ICollidable entity)
            {
                IsCollidable = entity.IsCollidable;
                Boss = entity;
            }

            public void Clear()
            {
                IsCollidable = false;
                Boss = null;
            }
        }
    }
}

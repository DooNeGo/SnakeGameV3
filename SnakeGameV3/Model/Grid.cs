using SnakeGameV3.Interfaces;
using System.Drawing;
using System.Numerics;

namespace SnakeGameV3.Model
{
    internal class Grid
    {
        private readonly List<IGridObject> _gridObjects = new();
        private readonly Cell[,] _cells;

        public Grid(Size screenSize, Size cellSize)
        {
            CellSize = cellSize;
            Size = new Size(screenSize.Width / CellSize.Width, screenSize.Height / CellSize.Height);
            _cells = new Cell[screenSize.Height, screenSize.Width];

            InitializeCells();
        }

        public Size Size { get; }

        public Size CellSize { get; }

        public bool IsCellOccupied(Vector2 position)
        {
            bool isOccupied = false;

            ForCell(position, (positionX, positionY) =>
            {
                if (_cells[(int)positionY, (int)positionX].Boss is not null)
                {
                    isOccupied = true;
                    return;
                }
            });

            return isOccupied;
        }

        public object? GetObjectInPosition(Vector2 position, object requester)
        {
            object? entity = null;

            ForCell(position, (positionX, positionY) =>
            {
                Cell cell = _cells[(int)positionY, (int)positionX];

                if (cell.Boss != null
                && cell.Boss != requester)
                {
                    entity = cell.Boss;
                    return;
                }
            });

            return entity;
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

        private void AddToGrid(Vector2 position, IGridObject entity)
        {
            ForCell(position, (positionX, positionY) =>
            {
                _cells[(int)positionY, (int)positionX].Occupy(entity);
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

        private void ForCell(Vector2 relativePosition, Action<float, float> action)
        {
            Vector2 absolutePosition = relativePosition.GetAbsolutePosition();

            for (var y = 0; y < CellSize.Height; y++)
            {
                var positionY = absolutePosition.Y + y;

                for (var x = 0; x < CellSize.Width; x++)
                {
                    var positionX = absolutePosition.X + x;

                    if (positionY >= _cells.GetLength(0)
                        || positionX >= _cells.GetLength(1)
                        || positionX < 0
                        || positionY < 0)
                        continue;

                    action(positionX, positionY);
                }
            }
        }

        internal struct Cell : ICollidable
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

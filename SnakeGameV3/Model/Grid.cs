using SnakeGameV3.Enums;
using SnakeGameV3.Interfaces;
using System.Drawing;
using System.Numerics;

namespace SnakeGameV3.Data
{
    internal class Grid
    {
        private readonly List<IGridObject> _gridObjects = new();

        private readonly ICellObject?[,] _cells;

        public Grid(int screenHeight, int screenWidth, int cellSize)
        {
            CellSize = cellSize;
            Size = new(screenWidth / cellSize, screenHeight / cellSize);
            _cells = new ICellObject?[Size.Height, Size.Width];

            InitializeCells();
        }

        public Size Size { get; }

        public int CellSize { get; }

        public bool IsOccupiedCell(int x, int y) => _cells[y, x] != null;

        public void Update()
        {
            Clear();

            foreach (IGridObject gridObject in _gridObjects)
            {
                foreach (Vector2 point in gridObject)
                {
                    if (_cells[(int)point.Y, (int)point.X] != null && _cells[(int)point.Y, (int)point.X]!.Type == PassType.Impassable)
                    {
                        gridObject.IsCrashed = true;
                        _cells[(int)point.Y, (int)point.X]!.IsCrashed = true;
                        break;
                    }

                    OccupyCell(point, gridObject);
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

        private void OccupyCell(Vector2 point, ICellObject cellObject)
        {
            _cells[(int)point.Y, (int)point.X] = cellObject;
        }

        private void InitializeCells()
        {
            for (var y = 0; y < Size.Height; y++)
                for (var x = 0; x < Size.Width; x++)
                    _cells[y, x] = null;
        }

        private void Clear()
        {
            for (var y = 0; y < Size.Height; y++)
                for (var x = 0; x < Size.Width; x++)
                    if (_cells[y, x] != null)
                        _cells[y, x] = null;
        }
    }
}

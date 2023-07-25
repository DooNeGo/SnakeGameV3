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
            _cells = new ICellObject[Size.Height, Size.Width];

            InitializeCells();
        }

        public Size Size { get; }

        public int CellSize { get; }

        public bool IsOccupiedCell(Vector2 position) => _cells[(int)Math.Round(position.Y), (int)Math.Round(position.X)] != null;

        public void Update()
        {
            Clear();

            foreach (IGridObject gridObject in _gridObjects)
            {
                foreach (Vector2 position in gridObject)
                {
                    if (position.X >= Size.Width || position.Y >= Size.Height)
                        continue;

                    //if (IsOccupiedCell(position))
                    //{
                    //    gridObject.IsCrashed = true;
                    //    _cells[(int)Math.Round(position.Y), (int)Math.Round(position.X)]!.IsCrashed = true;
                    //    break;
                    //}

                    OccupyCell(position, gridObject);
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

        private void OccupyCell(Vector2 position, ICellObject cellObject)
        {
            _cells[(int)Math.Round(position.Y), (int)Math.Round(position.X)] = cellObject;
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

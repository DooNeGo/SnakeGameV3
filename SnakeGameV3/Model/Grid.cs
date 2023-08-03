using SnakeGameV3.Interfaces;
using System.Collections;
using System.Drawing;
using System.Numerics;

namespace SnakeGameV3.Model
{
    internal class Grid : IEnumerable<Cell>
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

        public bool IsOccupiedCell(Vector2 position)
        {
            Vector2 convertedPosition = position.GetNormalCoord();

            for (var y = 0; y < CellSize.Height; y++)
                for (var x = 0; x < CellSize.Height; x++)
                    if (_cells[(int)convertedPosition.Y + y, (int)convertedPosition.X + x].Boss is not null)
                        return true;

            return false;
        }

        public Cell[,] GetCells(Vector2 position)
        {
            Vector2 convertedPosition = position.GetNormalCoord();
            var cells = new Cell[CellSize.Height, CellSize.Width];

            for (var y = 0; y < CellSize.Height; y++)
                for (var x = 0; x < CellSize.Width; x++)
                    cells[y, x] = _cells[(int)convertedPosition.Y + y, (int)convertedPosition.X + x];

            return cells;
        }

        public void Update()
        {
            Clear();

            foreach (IGridObject gridObject in _gridObjects)
            {
                foreach (Vector2 position in gridObject)
                {
                    if (position.X >= Size.Width
                        || position.Y >= Size.Height
                        || position.X < 0
                        || position.Y < 0)
                        continue;

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
            Vector2 convertedPosition = position.GetNormalCoord();

            for (var y = 0; y < CellSize.Height; y++)
                for (var x = 0; x < CellSize.Width; x++)
                    _cells[(int)convertedPosition.Y + y, (int)convertedPosition.X + x].Occupy(entity);
        }

        private void InitializeCells()
        {
            for (var y = 0; y < _cells.GetLength(0); y++)
                for (var x = 0; x < _cells.GetLength(1); x++)
                    _cells[y, x] = new Cell();
        }

        private void Clear()
        {
            for (var y = 0; y < _cells.GetLength(0); y++)
                for (var x = 0; x < _cells.GetLength(1); x++)
                    if (_cells[y, x].Boss is not null)
                        _cells[y, x].Clear();
        }

        public IEnumerator<Cell> GetEnumerator()
        {
            for (var y = 0; y < _cells.GetLength(0); y++)
            {
                for (var x = 0; x < _cells.GetLength(1); x++)
                {
                    yield return _cells[y, x];
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            for (var y = 0; y < _cells.GetLength(0); y++)
            {
                for (var x = 0; x < _cells.GetLength(1); x++)
                {
                    yield return _cells[y, x];
                }
            }
        }
    }
}

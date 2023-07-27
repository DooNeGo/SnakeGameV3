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
            Size = new(screenSize.Width / CellSize.Width, screenSize.Height / CellSize.Height);
            _cells = new Cell[Size.Height, Size.Width];

            InitializeCells();
        }

        public Size Size { get; }

        public Size CellSize { get; }

        public bool IsOccupiedCell(Vector2 position) => _cells[(int)Math.Round(position.Y), (int)Math.Round(position.X)].Boss is not null;

        public Cell GetCell(Vector2 position) => _cells[(int)Math.Round(position.Y), (int)Math.Round(position.X)];

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

                    _cells[(int)Math.Round(position.Y), (int)Math.Round(position.X)].Occupy(gridObject);
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

        private void InitializeCells()
        {
            for (var y = 0; y < _cells.GetLength(0); y++)
                for (var x = 0; x < _cells.GetLength(1); x++)
                    _cells[y, x] = new Cell();
        }

        private void Clear()
        {
            for (var y = 0; y < Size.Height; y++)
                for (var x = 0; x < Size.Width; x++)
                    if (_cells[y, x].Boss is not null)
                        _cells[y, x].Clear();
        }

        public IEnumerator<Cell> GetEnumerator()
        {
            for (var y = 0; y < _cells.GetLength(0); y++)
            {
                for (var x = 0; x <= _cells.GetLength(1); x++)
                {
                    yield return _cells[y, x];
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            for (var y = 0; y < _cells.GetLength(0); y++)
            {
                for (var x = 0; x <= _cells.GetLength(1); x++)
                {
                    yield return _cells[y, x];
                }
            }
        }
    }
}

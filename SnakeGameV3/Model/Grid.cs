using SnakeGameV3.Enums;
using SnakeGameV3.Interfaces;
using System.Drawing;
using System.Numerics;

namespace SnakeGameV3.Data
{
    internal class Grid
    {
        private readonly List<IGridObject> _gridObjects = new();
        private readonly PassType[,] _cells;

        public Grid(int screenHeight, int screenWidth, int cellSize)
        {
            CellSize = cellSize;
            Size = new(screenWidth / cellSize, screenHeight / cellSize);
            _cells = new PassType[Size.Height, Size.Width];

            InitializeCells();
        }

        public Size Size { get; }

        public int CellSize { get; }

        public bool IsOccupiedCell(Vector2 position) => _cells[(int)position.Y, (int)position.X] == PassType.Impassable;

        public void Update()
        {
            Clear();

            foreach (IGridObject gridObject in _gridObjects)
            {
                foreach (Vector2 position in gridObject)
                {
                    if (position.X >= Size.Width || position.Y >= Size.Height)
                        continue;

                    OccupyCell(position, gridObject.Type);
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

        private void OccupyCell(Vector2 position, PassType passType)
        {
            _cells[(int)position.Y, (int)position.X] = passType;
        }

        private void InitializeCells()
        {
            for (var y = 0; y < Size.Height; y++)
                for (var x = 0; x < Size.Width; x++)
                    _cells[y, x] = PassType.Passable;
        }

        private void Clear()
        {
            for (var y = 0; y < Size.Height; y++)
                for (var x = 0; x < Size.Width; x++)
                    if (_cells[y, x] != PassType.Passable)
                        _cells[y, x] = PassType.Passable;
        }
    }
}

using System.Drawing;

namespace SnakeGameV3.Data
{
    internal class Grid
    {
        public Grid(int screenHeight, int screenWidth, int cellSize)
        {
            CellSize = cellSize;
            Height = screenHeight / cellSize;
            Width = screenWidth / cellSize;
            IsOccupiedCell = new bool[Height, Width];
        }

        public List<IEnumerable<Point>> GameObjectsCoordinates { get; private set; } = new();

        public int Height { get; }

        public int Width { get; }

        public int CellSize { get; }

        public bool[,] IsOccupiedCell { get; private set; }

        private void OccupyCell(Point coordinates)
        {
            IsOccupiedCell[coordinates.Y, coordinates.X] = true;
        }

        public void Update()
        {
            Clear();

            foreach(IEnumerable<Point> gameObjectCoordinates in GameObjectsCoordinates)
                foreach(Point coordinates in gameObjectCoordinates)
                    OccupyCell(coordinates);
        }

        public void Clear()
        {
            for (var y = 0; y < Height; y++)
                for (var x = 0; x < Width; x++)
                    IsOccupiedCell[y, x] = false;
        }
    }
}

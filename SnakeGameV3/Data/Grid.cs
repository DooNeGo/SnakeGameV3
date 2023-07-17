﻿using SnakeGameV3.Enums;
using SnakeGameV3.Interfaces;
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
            _cells = new PassType?[Height, Width];
        }

        private readonly List<IGridObject> _gridObjects = new();

        private readonly PassType?[,] _cells;

        public int Height { get; }

        public int Width { get; }

        public int CellSize { get; }

        public bool IsOccupiedCell(int x, int y) => _cells[y, x] != null;

        private void OccupyCell(Point coordinates, PassType type)
        {
            _cells[coordinates.Y, coordinates.X] = type;
        }

        private void InitializeCells()
        {
            for (var y = 0; y < Height; y++)
                for (var x = 0; x < Width; x++)
                    _cells[y, x] = null;
        }

        public void Update()
        {
            InitializeCells();

            foreach (IGridObject gridObject in _gridObjects)
            {
                foreach (Point coordinates in gridObject)
                {
                    if (_cells[coordinates.Y, coordinates.X] == PassType.Impassable)
                    {
                        gridObject.IsCrashed = true;
                        break;
                    }

                    OccupyCell(coordinates, gridObject.Type);
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
    }
}

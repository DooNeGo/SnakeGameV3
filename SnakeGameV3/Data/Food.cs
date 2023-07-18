﻿using SnakeGameV3.Enums;
using SnakeGameV3.Interfaces;
using SnakeGameV3.Rendering;
using System.Collections;
using System.Drawing;

namespace SnakeGameV3.Data
{
    internal class Food : IGridObject, IEnumerable<PointWithColor>
    {
        public Food(ConsoleColor color, Grid grid)
        {
            Color = color;
            _grid = grid;
            RandCoordinates();
        }

        private readonly Random _random = new();

        private readonly Grid _grid;

        public Point Point { get; private set; }

        public ConsoleColor Color { get; }

        public bool IsCrashed { get; set; } = false;

        public PassType Type => PassType.Passable;

        public void RandCoordinates()
        {
            do
            {
                Point = new Point(_random.Next(1, _grid.Width - 2), _random.Next(1, _grid.Height - 2));
            } while (_grid.IsOccupiedCell(Point.X, Point.Y) == true);
        }

        public IEnumerator<Point> GetEnumerator()
        {
            yield return Point;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            yield return Point;
        }

        IEnumerator<PointWithColor> IEnumerable<PointWithColor>.GetEnumerator()
        {
            yield return new PointWithColor(Point, Color);
        }
    }
}
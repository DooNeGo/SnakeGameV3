﻿using SnakeGameV3.Enums;
using SnakeGameV3.Interfaces;
using SnakeGameV3.Rendering;
using System.Collections;
using System.Drawing;

namespace SnakeGameV3.Data
{
    internal class Boarder : IGridObject, IEnumerable<KeyValuePair<Point, ConsoleColor>>
    {
        public Boarder(Grid grid, ConsoleColor color)
        {
            _grid = grid;
            Color = color;
            InitializeBoarder();
        }

        private readonly List<Point> _points = new();

        private readonly Grid _grid;

        public ConsoleColor Color { get; }

        public bool IsCrashed { get; set; } = false;

        public PassType Type => PassType.Impassable;

        private void InitializeBoarder()
        {
            for (var i = 0; i < _grid.Width; i++)
            {
                _points.Add(new Point(i, 0));
                _points.Add(new Point(i, _grid.Height - 1));
            }

            for (var i = 0; i < _grid.Height; i++)
            {
                _points.Add(new Point(0, i));
                _points.Add(new Point(_grid.Width - 1, i));
            }
        }

        public IEnumerator<Point> GetEnumerator() => _points.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _points.GetEnumerator();

        IEnumerator<KeyValuePair<Point, ConsoleColor>> IEnumerable<KeyValuePair<Point, ConsoleColor>>.GetEnumerator()
        {
            foreach (Point point in _points)
                yield return new KeyValuePair<Point, ConsoleColor>(point, Color);
        }
    }
}
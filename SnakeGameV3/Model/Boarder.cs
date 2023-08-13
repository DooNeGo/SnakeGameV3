﻿using SnakeGameV3.Interfaces;
using System.Collections;
using System.Numerics;

namespace SnakeGameV3.Model
{
    internal class Boarder
    {
        private readonly List<Vector2> _body = new();
        private readonly Grid _grid;

        public Boarder(Grid grid, ConsoleColor color)
        {
            _grid = grid;
            Color = color;
            InitializeBoarder();
        }

        public ConsoleColor Color { get; }

        public bool IsCollidable => true;

        public IEnumerator<Vector2> GetEnumerator() => _body.GetEnumerator();

        private void InitializeBoarder()
        {
            for (var i = 0; i < _grid.Size.Width; i++)
            {
                _body.Add(new Vector2(i, 0));
                _body.Add(new Vector2(i, _grid.Size.Height - 1));
            }

            for (var i = 0; i < _grid.Size.Height; i++)
            {
                _body.Add(new Vector2(0, i));
                _body.Add(new Vector2(_grid.Size.Width - 1, i));
            }
        }
    }
}
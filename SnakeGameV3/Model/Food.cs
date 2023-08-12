using SnakeGameV3.Interfaces;
using System.Collections;
using System.Numerics;

namespace SnakeGameV3.Model
{
    internal class Food : IGridObject, IRenderable
    {
        private readonly Random _random = new();
        private readonly Grid _grid;
        private readonly Texture _texture = Texture.Food;

        public Food(ConsoleColor color, Grid grid)
        {
            Color = color;
            _grid = grid;
        }

        public Vector2 Position { get; private set; }

        public ConsoleColor Color { get; }

        public bool IsCollidable => false;

        public void RandCoordinates()
        {
            do
            {
                Position = new Vector2(_random.Next(1, _grid.Size.Width - 2), _random.Next(1, _grid.Size.Height - 2));
            } while (_grid.IsPositionOccupied(Position));
        }

        public IEnumerator<Vector2> GetEnumerator()
        {
            yield return Position;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            yield return Position;
        }

        IEnumerator<ValueTuple<Vector2, ConsoleColor, Texture>> IEnumerable<ValueTuple<Vector2, ConsoleColor, Texture>>.GetEnumerator()
        {
            Vector2 projection = _grid.Project(Position);

            yield return new ValueTuple<Vector2, ConsoleColor, Texture>(_grid.GetAbsolutePosition(projection), Color, _texture);
        }
    }
}
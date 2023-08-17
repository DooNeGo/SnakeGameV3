using SnakeGameV3.Interfaces;
using System.Collections;
using System.Numerics;

namespace SnakeGameV3.Model
{
    internal class Food : IGridObject, IRenderable
    {
        private readonly Random _random = new();
        private readonly Grid _grid;
        private readonly TextureInfo _textureInfo;

        public Food(ConsoleColor color, Grid grid)
        {
            _grid = grid;
            _textureInfo = new TextureInfo(TextureName.Food, Scale, color);
        }

        public Vector2 Position { get; private set; }

        public bool IsCollidable => false;

        public float Scale => 0.5f;

        public IEnumerator<(Vector2, float)> GetEnumerator()
        {
            yield return new(Position, _textureInfo.Scale);
        }

        public void RandCoordinates()
        {
            do
            {
                Position = new Vector2(_random.Next(1, _grid.Size.Width - 2), _random.Next(1, _grid.Size.Height - 2));
            } while (_grid.IsPositionOccupied(Position, this));
        }

        IEnumerator<(Vector2, TextureInfo)> IEnumerable<(Vector2, TextureInfo)>.GetEnumerator()
        {
            yield return new(_grid.GetAbsolutePosition(Position), _textureInfo);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            yield return Position;
        }

        IEnumerator<Vector2> IEnumerable<Vector2>.GetEnumerator()
        {
            yield return Position;
        }
    }
}
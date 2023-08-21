using SnakeGameV3.Interfaces;
using SnakeGameV3.Texturing;
using System.Collections;
using System.Numerics;

namespace SnakeGameV3.Model
{
    internal class Food : GameObject, IRenderable, IGridObject
    {
        private readonly Random _random = new();
        private readonly Grid _grid;

        public Food(TextureConfig textureConfig, Grid grid, ColliderType colliderType) :
            base(Vector2.Zero, textureConfig, colliderType)
        {
            _grid = grid;
        }

        public bool IsNeedToProject => false;

        public void RandCoordinates()
        {
            do
            {
                Position = new Vector2(_random.Next(1, _grid.Size.Width - 2), _random.Next(1, _grid.Size.Height - 2));
            } while (_grid.IsPositionOccupied(Position, this));
        }

        public IEnumerator<IGridObjectPart> GetEnumerator()
        {
            yield return this;
        }

        IEnumerator<IReadOnlyGameObject> IEnumerable<IReadOnlyGameObject>.GetEnumerator()
        {
            yield return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            yield return this;
        }
    }
}
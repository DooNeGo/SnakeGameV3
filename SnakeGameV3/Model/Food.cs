using SnakeGameV3.Interfaces;
using System.Numerics;

namespace SnakeGameV3.Model
{
    internal class Food : GameObject, ICompositeObject
    {
        private readonly Random _random = new();
        private readonly Grid _grid;

        public Food(Grid grid, float scale) :
            base(Vector2.Zero)
        {
            _grid = grid;
            Scale = scale;
        }

        public bool IsNeedToProject => false;

        public float Scale { get; }

        public void RandCoordinates()
        {
            do
            {
                Position = new Vector2(_random.Next(1, _grid.Size.Width - 2), _random.Next(1, _grid.Size.Height - 2));
            } while (_grid.IsPositionOccupied(Position, this, Scale));
        }

        public IEnumerator<IReadOnlyGameObject> GetGameObjectsWithComponent<T>()
        {
            if (GetComponent<T>() != null)
            {
                yield return this;
            }
        }
    }
}
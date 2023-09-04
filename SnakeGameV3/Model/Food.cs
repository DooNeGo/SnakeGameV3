using SnakeGameV3.Interfaces;
using SnakeGameV3.Texturing;
using System.Numerics;

namespace SnakeGameV3.Model
{
    internal class Food : GameObject, ICompositeObject
    {
        private readonly Random _random = new();
        private readonly Grid _grid;

        public Food(Grid grid, float scale, ConsoleColor color, Indexer indexer) :
            base(Vector2.Zero, scale, indexer.GetUniqueIndex())
        {
            _grid = grid;

            Collider collider = new(ColliderType.Square, this);
            collider.CollisionEntry += OnCollisionEnter;

            AddComponent(new TextureConfig(TextureName.Food, color));
            AddComponent(collider);

            RandCoordinates();
        }

        public bool IsNeedToProject => true;

        public IEnumerator<IReadOnlyGameObject> GetGameObjectsWithComponent<T>() where T : Component
        {
            if (GetComponent<T>() is not null)
            {
                yield return this;
            }
        }

        private void RandCoordinates()
        {
            do
            {
                Position = new Vector2(_random.Next(1, _grid.Size.Width - 2), _random.Next(1, _grid.Size.Height - 2));
            } while (_grid.IsPositionOccupied(Position, Scale));
        }

        private void OnCollisionEnter(IReadOnlyGameObject gameObject)
        {
            RandCoordinates();
        }
    }
}
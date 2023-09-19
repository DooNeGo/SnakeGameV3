using SnakeGameV3.Interfaces;
using SnakeGameV3.Texturing;
using System.Numerics;

namespace SnakeGameV3.Model
{
    internal class Food : GameObject, ICompositeObject
    {
        public event Action<IReadOnlyGameObject>? Collect;

        public Food(Vector2 position, float scale, ConsoleColor color, Effect effect, int lifeTime) :
            base(position, scale)
        {

            Collider collider = new(ColliderType.Square, this);
            collider.CollisionEntry += OnCollisionEnter;

            AddComponent(new TextureConfig(TextureName.Food, color, this));
            AddComponent(collider);

            Effect = effect;
            LifeTime = lifeTime;
        }

        public Effect Effect { get; }

        public int LifeTime { get; }

        public IEnumerator<IReadOnlyGameObject> GetGameObjectsWithComponent<T>() where T : Component
        {
            if (GetComponent<T>() is not null)
            {
                yield return this;
            }
        }

        private void OnCollisionEnter(IReadOnlyGameObject gameObject)
        {
            Collect?.Invoke(gameObject);
        }
    }
}
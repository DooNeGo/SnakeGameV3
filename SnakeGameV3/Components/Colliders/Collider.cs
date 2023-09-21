using SnakeGameV3.Model;
using System.Numerics;

namespace SnakeGameV3.Components.Colliders
{
    internal abstract class Collider : Component
    {
        public event Action<GameObject>? CollisionEntry;

        public void InvokeCollision(GameObject gameObject)
        {
            CollisionEntry?.Invoke(gameObject);
        }

        public abstract float GetDistanceToEdge(Vector2 position);
    }
}
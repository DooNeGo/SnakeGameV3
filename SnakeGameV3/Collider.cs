using SnakeGameV3.Interfaces;
using System.Numerics;

namespace SnakeGameV3
{
    internal enum ColliderType
    {
        Square,
        Circle
    }

    internal class Collider : Component
    {
        public event Action<Collider>? CollisionEntry;

        public Collider(ColliderType colliderType, IReadOnlyGameObject parent)
        {
            Type = colliderType;
            Parent = parent;
        }

        public ColliderType Type { get; }

        public IReadOnlyGameObject Parent { get; }

        public void GetCollision(Collider collider)
        {
            CollisionEntry?.Invoke(collider);
        }

        public float GetDistanceToEdge(Collider collider)
        {
            if (Type == ColliderType.Circle)
            {
                return Parent.Scale / 2;
            }
            else if (Type == ColliderType.Square)
            {
                Vector2 VectorToCollider = Vector2.Normalize(Parent.Position - collider.Parent.Position);

                float cosBeetweenVectors = Vector2.Dot(Vector2.UnitY, VectorToCollider);
                float angleBeetweenVectors = MathF.Acos(cosBeetweenVectors);

                return Parent.Scale / 2 * MathF.Sin(angleBeetweenVectors);
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
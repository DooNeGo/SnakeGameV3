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
        public event Action<IReadOnlyGameObject>? CollisionEntry;

        private readonly IReadOnlyGameObject _parent;

        public Collider(ColliderType colliderType, IReadOnlyGameObject parent)
        {
            Type = colliderType;
            _parent = parent;
        }

        public ColliderType Type { get; }

        public void InvokeCollision(IReadOnlyGameObject gameObject)
        {
            CollisionEntry?.Invoke(gameObject);
        }

        public float GetDistanceToEdge(Vector2 position)
        {
            if (Type == ColliderType.Circle)
            {
                return _parent.Scale / 2;
            }
            else if (Type == ColliderType.Square)
            {
                Vector2 VectorToCollider = Vector2.Normalize(_parent.Position - position);
                VectorToCollider = Vector2.Abs(VectorToCollider);
                Vector2 UnitVector;

                if (VectorToCollider.X > VectorToCollider.Y)
                    UnitVector = Vector2.UnitX;
                else
                    UnitVector = Vector2.UnitY;

                float cosBeetweenVectors = Vector2.Dot(UnitVector, VectorToCollider);

                return _parent.Scale / 2 / cosBeetweenVectors;
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
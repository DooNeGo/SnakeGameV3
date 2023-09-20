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

        private readonly ColliderType _type;

        public Collider(ColliderType colliderType, IReadOnlyGameObject parent) : base(parent)
        {
            _type = colliderType;
        }

        public void InvokeCollision(IReadOnlyGameObject gameObject)
        {
            CollisionEntry?.Invoke(gameObject);
        }

        public float GetDistanceToEdge(Vector2 position)
        {
            switch (_type)
            {
                case ColliderType.Square:
                    Vector2 VectorToCollider = Vector2.Normalize(_parent.Position - position);
                    VectorToCollider = Vector2.Abs(VectorToCollider);
                    Vector2 UnitVector;

                    if (VectorToCollider.X > VectorToCollider.Y)
                        UnitVector = Vector2.UnitX;
                    else
                        UnitVector = Vector2.UnitY;

                    float cosBeetweenVectors = Vector2.Dot(UnitVector, VectorToCollider);

                    return _parent.Scale / 2 / cosBeetweenVectors;

                case ColliderType.Circle:
                    return _parent.Scale / 2;

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
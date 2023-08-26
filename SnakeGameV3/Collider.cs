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

        public Vector2 Position => Parent.Position;

        public IReadOnlyGameObject Parent { get; }

        public void InvokeCollision(Collider collider)
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
                Vector2 VectorToCollider = Vector2.Normalize(Position - collider.Position);
                Vector2 UnitVector;

                if (MathF.Abs(VectorToCollider.X) > MathF.Abs(VectorToCollider.Y))
                    UnitVector = Vector2.UnitX;
                else
                    UnitVector = Vector2.UnitY;

                float cosBeetweenVectors = Vector2.Dot(UnitVector, VectorToCollider);

                return Parent.Scale / 2 / MathF.Abs(cosBeetweenVectors);
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
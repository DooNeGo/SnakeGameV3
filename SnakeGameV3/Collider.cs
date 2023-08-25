using SnakeGameV3.Interfaces;
using System.Numerics;

namespace SnakeGameV3
{
    internal enum ColliderType
    {
        Square,
        Circle
    }

    internal record Collider : Component
    {
        private event Action<Collider>? CollisionEntry;

        public Collider(ColliderType colliderType, IReadOnlyGameObject parent)
        {
            Type = colliderType;
            Parent = parent;
        }

        public ColliderType Type { get; init; }

        public IReadOnlyGameObject Parent { get; init; }

        public void OnCollisionEntry(Action<Collider> action)
        {
            CollisionEntry += action;
        }

        public bool CheckCollision(Collider collider)
        {
            float distanceToEdge1 = GetDistanceToEdge(collider);
            float distanceToEdge2 = collider.GetDistanceToEdge(this);
            float distanceBeetween = Vector2.Distance(Parent.Position, collider.Parent.Position);

            if (distanceToEdge1 + distanceToEdge2 >= distanceBeetween)
            {
                CollisionEntry?.Invoke(collider);
                return true;
            }

            return false;
        }

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
                Vector2 UnitVector = Vector2.Zero;

                if (VectorToCollider.X >= 0
                    && VectorToCollider.Y >= 0)
                {
                    UnitVector = Vector2.UnitY;
                }
                else if (VectorToCollider.X >= 0
                    && VectorToCollider.Y <= 0)
                {
                    UnitVector = -Vector2.UnitY;
                }
                else if (VectorToCollider.X < 0
                    && VectorToCollider.Y < 0)
                {
                    UnitVector = -Vector2.UnitY;
                }
                else if (VectorToCollider.X < 0
                    && VectorToCollider.Y > 0)
                {
                    UnitVector = Vector2.UnitY;
                }

                float cosBeetweenVectors = Vector2.Dot(UnitVector, VectorToCollider);
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
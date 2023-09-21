using System.Numerics;

namespace SnakeGameV3.Components.Colliders
{
    internal class BoxCollider : Collider
    {
        public override float GetDistanceToEdge(Vector2 position)
        {
            Vector2 VectorToCollider = Vector2.Normalize(Parent!.GetComponent<Transform>()!.Position - position);
            VectorToCollider = Vector2.Abs(VectorToCollider);
            Vector2 UnitVector;

            if (VectorToCollider.X > VectorToCollider.Y)
                UnitVector = Vector2.UnitX;
            else
                UnitVector = Vector2.UnitY;

            float cosBeetweenVectors = Vector2.Dot(UnitVector, VectorToCollider);

            return GetComponent<Transform>()!.Scale / 2 / cosBeetweenVectors;
        }
    }
}

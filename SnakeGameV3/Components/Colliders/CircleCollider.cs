using System.Numerics;

namespace SnakeGameV3.Components.Colliders
{
    internal class CircleCollider : Collider
    {
        public override float GetDistanceToEdge(Vector2 position)
        {
            return GetComponent<Transform>().Scale / 2;
        }
    }
}
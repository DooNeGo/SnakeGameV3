using System.Numerics;

namespace SnakeGameV3.Components
{
    internal class Transform : Component
    {
        public Vector2 Position { get; set; }

        public Quaternion Rotation { get; set; }

        public float Scale { get; set; }
    }
}
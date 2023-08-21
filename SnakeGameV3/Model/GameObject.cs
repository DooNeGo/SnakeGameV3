using SnakeGameV3.Interfaces;
using SnakeGameV3.Texturing;
using System.Numerics;

namespace SnakeGameV3.Model
{
    internal class GameObject : IReadOnlyGameObject, IGridObjectPart
    {
        public GameObject(Vector2 position, TextureConfig textureConfig, ColliderType colliderType)
        {
            Position = position;
            TextureConfig = textureConfig;
            ColliderType = colliderType;
        }

        public Vector2 Position { get; set; }

        public TextureConfig TextureConfig { get; set; }

        public float Scale => TextureConfig.Scale;

        public ColliderType ColliderType { get; }
    }
}

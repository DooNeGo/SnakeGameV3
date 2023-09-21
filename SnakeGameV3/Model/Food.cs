using SnakeGameV3.Components;
using SnakeGameV3.Components.Colliders;
using SnakeGameV3.Texturing;
using System.Numerics;

namespace SnakeGameV3.Model
{
    internal class Food : GameObject
    {
        public Food(Vector2 position, float scale, ConsoleColor color, int lifeTime)
        {
            LifeTime = lifeTime;
            Transform transform = AddComponent<Transform>();
            transform.Position = position;
            transform.Scale = scale;

            TextureConfig textureConfig = AddComponent<TextureConfig>();
            textureConfig.Color = color;
            textureConfig.Name = TextureName.Food;

            AddComponent<BoxCollider>();
        }

        public int LifeTime { get; }
    }
}
using System.Numerics;
using SnakeGameV3.Texturing;

namespace SnakeGameV3.Interfaces
{
    internal interface IReadOnlyGameObject : IScalable
    {
        public Vector2 Position { get; }

        public TextureConfig TextureConfig { get; }
    }
}

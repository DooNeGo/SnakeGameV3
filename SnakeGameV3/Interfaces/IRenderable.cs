using System.Numerics;

namespace SnakeGameV3.Interfaces
{
    internal interface IRenderable : IEnumerable<ValueTuple<Vector2, ConsoleColor, TextureInfo>>
    {
        //public AnimationType AnimationType { get; }
    }
}

using SnakeGameV3.Components;
using SnakeGameV3.Model;

namespace SnakeGameV3.Interfaces
{
    internal interface ICompositeObject
    {
        public IEnumerator<GameObject> GetGameObjectsWithComponent<T>() where T : Component;
    }
}
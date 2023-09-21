using SnakeGameV3.Components;
using SnakeGameV3.Interfaces;

namespace SnakeGameV3.Model
{
    internal class Scene : ICompositeObject
    {
        private readonly ICompositeObject[] _compositeObjects;

        public Scene(params ICompositeObject[] compositeObjects)
        {
            _compositeObjects = compositeObjects;
        }

        public IEnumerator<GameObject> GetGameObjectsWithComponent<T>() where T : Component
        {
            foreach (ICompositeObject compositeObject in _compositeObjects)
            {
                IEnumerator<GameObject> enumerator = compositeObject.GetGameObjectsWithComponent<T>();

                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current;
                }
            }
        }
    }
}
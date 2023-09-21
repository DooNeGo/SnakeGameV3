using SnakeGameV3.Components;
using SnakeGameV3.Interfaces;
using System.Collections;

namespace SnakeGameV3.Model
{
    internal class Scene : ICompositeObject
    {
        private readonly ICompositeObject[] _compositeObjects;
        private readonly List<GameObject> _gameObjects = new();

        public Scene(params ICompositeObject[] compositeObjects)
        {
            _compositeObjects = compositeObjects;
            Update();
        }

        public void Update()
        {
            _gameObjects.Clear();

            for (int i = 0; i < _compositeObjects.Length; i++)
            {
                _gameObjects.AddRange(_compositeObjects[i]);
            }
        }

        public IEnumerator<GameObject> GetGameObjectsWithComponent<T>() where T : Component
        {
            for (var i = 0; i < _gameObjects.Count; i++)
            {
                if (_gameObjects[i].TryGetComponent<T>() is not null)
                {
                    yield return _gameObjects[i];
                }
            }
        }

        public IEnumerator<GameObject> GetEnumerator()
        {
            return _gameObjects.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _gameObjects.GetEnumerator();
        }
    }
}
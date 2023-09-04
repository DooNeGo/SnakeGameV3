using SnakeGameV3.Interfaces;
using SnakeGameV3.Model;
using System.Numerics;

namespace SnakeGameV3
{
    internal class CollisionHandler
    {
        private readonly List<IReadOnlyGameObject> _gameObjects = new();

        public CollisionHandler(Scene initialScene)
        {
            ActiveScene = initialScene;
        }

        public Scene ActiveScene { get; set; }

        public void Update()
        {
            UpdateCollidersList();
            CheckCollisions();
        }

        private void UpdateCollidersList()
        {
            _gameObjects.Clear();

            IEnumerator<IReadOnlyGameObject> enumerator = ActiveScene.GetGameObjectsWithComponent<Collider>();

            while (enumerator.MoveNext())
            {
                _gameObjects.Add(enumerator.Current);
            }
        }

        private void CheckCollisions()
        {
            for (var i = 0; i < _gameObjects.Count - 1; i++)
            {
                Collider collider1 = _gameObjects[i].GetComponent<Collider>()!;

                for (var j = i + 1; j < _gameObjects.Count; j++)
                {
                    Collider collider2 = _gameObjects[j].GetComponent<Collider>()!;

                    float distanceToEdge1 = collider1.GetDistanceToEdge(_gameObjects[j].Position);
                    float distanceToEdge2 = collider2.GetDistanceToEdge(_gameObjects[i].Position);

                    float distanceBeetween = Vector2.Distance(_gameObjects[i].Position, _gameObjects[j].Position);

                    if (distanceBeetween <= distanceToEdge1 + distanceToEdge2)
                    {
                        collider1.InvokeCollision(_gameObjects[j]);
                        collider2.InvokeCollision(_gameObjects[i]);
                    }
                }
            }
        }
    }
}

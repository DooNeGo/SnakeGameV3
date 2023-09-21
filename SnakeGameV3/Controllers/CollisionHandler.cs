using SnakeGameV3.Components;
using SnakeGameV3.Components.Colliders;
using SnakeGameV3.Model;
using System.Numerics;

namespace SnakeGameV3.Controllers
{
    internal class CollisionHandler
    {
        private readonly List<GameObject> _gameObjects = new();

        public CollisionHandler(Scene initialScene)
        {
            ActiveScene = initialScene;
        }

        public Scene ActiveScene { get; set; }

        public void Update()
        {
            UpdateGameObjectsList();
            CheckCollisions();
        }

        private void UpdateGameObjectsList()
        {
            _gameObjects.Clear();

            IEnumerator<GameObject> enumerator = ActiveScene.GetGameObjectsWithComponent<Collider>();

            while (enumerator.MoveNext())
            {
                _gameObjects.Add(enumerator.Current);
            }
        }

        private void CheckCollisions()
        {
            for (var i = 0; i < _gameObjects.Count - 1; i++)
            {
                Collider collider1 = _gameObjects[i].GetComponent<Collider>();
                Transform transform1 = _gameObjects[i].GetComponent<Transform>();

                for (var j = i + 1; j < _gameObjects.Count; j++)
                {
                    Collider collider2 = _gameObjects[j].GetComponent<Collider>();
                    Transform transform2 = _gameObjects[j].GetComponent<Transform>();

                    float distanceToEdge1 = collider1.GetDistanceToEdge(transform2.Position);
                    float distanceToEdge2 = collider2.GetDistanceToEdge(transform1.Position);
                    float distanceBeetween = Vector2.Distance(transform1.Position, transform2.Position);

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

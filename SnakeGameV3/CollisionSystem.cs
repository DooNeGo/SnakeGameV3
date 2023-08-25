using SnakeGameV3.Interfaces;
using System.Numerics;

namespace SnakeGameV3
{
    internal class CollisionSystem
    {
        private readonly List<ICompositeObject> _compositeObjects = new();
        private readonly List<Collider> _colliders = new();

        public void Update()
        {
            UpdateCollidersList();
            CheckCollisions();
        }

        public void Add(ICompositeObject compositeObject)
        {
            _compositeObjects.Add(compositeObject);
        }

        public void Remove(ICompositeObject compositeObject)
        {
            _compositeObjects.Remove(compositeObject);
        }

        private void UpdateCollidersList()
        {
            _colliders.Clear();

            foreach (ICompositeObject compositeObject in _compositeObjects)
            {
                IEnumerator<IReadOnlyGameObject> enumerator = compositeObject.GetGameObjectsWithComponent<Collider>();

                while (enumerator.MoveNext())
                {
                    _colliders.Add(enumerator.Current.GetComponent<Collider>()!);
                }
            }
        }

        private void CheckCollisions()
        {
            for (var i = 0; i < _colliders.Count - 1; i++)
            {
                float distanceToEdge1 = _colliders[i].GetDistanceToEdge(_colliders[i + 1]);

                for (var j = i + 1; j < _colliders.Count; j++)
                {
                    float distanceToEdge2 = _colliders[j].GetDistanceToEdge(_colliders[i]);
                    float distanceBeetween = Vector2.Distance(_colliders[i].Parent.Position, _colliders[j].Parent.Position);

                    if (distanceToEdge1 + distanceToEdge2 >= distanceBeetween)
                    {
                        _colliders[i].GetCollision(_colliders[j]);
                        _colliders[j].GetCollision(_colliders[i]);
                    }
                }
            }
        }
    }
}

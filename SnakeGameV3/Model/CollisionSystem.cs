using SnakeGameV3.Interfaces;

namespace SnakeGameV3.Model
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
                for (var j = i + 1; j < _colliders.Count; j++)
                {
                    if (_colliders[i].CheckCollision(_colliders[j]))
                    {
                        _colliders[j].GetCollision(_colliders[i]);
                    }
                }
            }
        }
    }
}

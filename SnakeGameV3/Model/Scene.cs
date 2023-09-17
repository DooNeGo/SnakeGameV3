using SnakeGameV3.Interfaces;

namespace SnakeGameV3.Model
{
    internal class Scene
    {
        private readonly List<ICompositeObject> _compositeObjects = new();
        private readonly Grid _grid;

        public Scene(Grid grid)
        {
            _grid = grid;
        }

        public void Add(ICompositeObject compositeObject)
        {
            _compositeObjects.Add(compositeObject);
        }

        public void Remove(ICompositeObject compositeObject)
        {
            _compositeObjects.Remove(compositeObject);
        }

        public IEnumerator<IReadOnlyGameObject> GetGameObjectsWithComponent<T>() where T : Component
        {
            foreach (ICompositeObject compositeObject in _compositeObjects)
            {
                IEnumerator<IReadOnlyGameObject> enumerator = compositeObject.GetGameObjectsWithComponent<T>();

                while (enumerator.MoveNext())
                {
                    IReadOnlyGameObject gameObject = enumerator.Current;
                    yield return gameObject.Clone(_grid.Project(gameObject.Position));
                }
            }
        }
    }
}
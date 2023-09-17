using SnakeGameV3.Interfaces;

namespace SnakeGameV3.Model
{
    internal class Scene
    {
        private readonly ICompositeObject[] _compositeObjects;
        private readonly Grid _grid;

        public Scene(Grid grid, params ICompositeObject[] compositeObjects)
        {
            _compositeObjects = compositeObjects;
            _grid = grid;
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
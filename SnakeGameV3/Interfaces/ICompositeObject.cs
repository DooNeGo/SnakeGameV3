namespace SnakeGameV3.Interfaces
{
    internal interface ICompositeObject : IScalable
    {
        public IEnumerator<IReadOnlyGameObject> GetGameObjectsWithComponent<T>() where T : Component;
    }
}

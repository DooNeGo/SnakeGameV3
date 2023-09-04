namespace SnakeGameV3.Interfaces
{
    internal interface ICompositeObject : IProjectable, IScalable
    {
        public IEnumerator<IReadOnlyGameObject> GetGameObjectsWithComponent<T>() where T : Component;
    }
}

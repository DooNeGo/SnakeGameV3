namespace SnakeGameV3.Interfaces
{
    internal interface IFactory<T>
    {
        public T[,] GetSquare(T color);
    }
}

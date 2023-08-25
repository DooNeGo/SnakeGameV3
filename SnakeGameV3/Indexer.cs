namespace SnakeGameV3
{
    internal class Indexer
    {
        private int _nextIndex = int.MinValue;

        public int GetUniqueIndex()
        {
            return _nextIndex++;
        }
    }
}

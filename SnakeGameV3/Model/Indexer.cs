namespace SnakeGameV3.Model
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

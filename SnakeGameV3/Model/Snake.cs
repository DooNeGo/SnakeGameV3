using SnakeGameV3.Enums;
using SnakeGameV3.Interfaces;
using System.Collections;
using System.Diagnostics;
using System.Numerics;

namespace SnakeGameV3.Data
{
    internal class Snake : IMovable, IGridObject, IEnumerable<ValueTuple<Vector2, ConsoleColor>>
    {
        private readonly Stopwatch _stopwatch = Stopwatch.StartNew();

        private bool _isAte = false;

        public Snake(int x, int y, ConsoleColor headColor, ConsoleColor bodyColor, float speed)
        {
            Position = new Vector2(x, y);
            HeadColor = headColor;
            BodyColor = bodyColor;
            MoveSpeed = speed;

            Body.Enqueue(new Vector2(Position.X - 2, Position.Y));
            Body.Enqueue(new Vector2(Position.X - 1, Position.Y));
        }

        public Vector2 Position { get; private set; }

        public Queue<Vector2> Body { get; } = new();

        public ConsoleColor HeadColor { get; }

        public ConsoleColor BodyColor { get; }

        public float MoveSpeed { get; }

        public bool IsCrashed { get; set; } = false;

        public PassType Type => PassType.Impassable;

        public long DeltaTime => _stopwatch.ElapsedMilliseconds;

        public void MoveToPosition(Vector2 nextPosition)
        {
            _stopwatch.Restart();

            if (nextPosition == Position)
                return;

            if ((int)nextPosition.X != (int)Position.X || (int)nextPosition.Y != (int)Position.Y)
            {
                Body.Dequeue();
                Body.Enqueue(Position);
            }

            Position = nextPosition;
        }

        public bool TryToEat(Food food)
        {
            if (Position != food.Position)
                return false;

            _isAte = true;

            return true;
        }

        public IEnumerator<Vector2> GetEnumerator()
        {
            foreach (Vector2 point in Body)
                yield return point;
            yield return Position;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (Vector2 point in Body)
                yield return point;
            yield return Position;
        }

        IEnumerator<ValueTuple<Vector2, ConsoleColor>> IEnumerable<ValueTuple<Vector2, ConsoleColor>>.GetEnumerator()
        {
            foreach (Vector2 point in Body)
                yield return new ValueTuple<Vector2, ConsoleColor>(point, BodyColor);
            yield return new ValueTuple<Vector2, ConsoleColor>(Position, HeadColor);
        }
    }
}
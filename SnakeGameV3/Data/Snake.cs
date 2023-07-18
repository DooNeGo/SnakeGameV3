using SnakeGameV3.Enums;
using SnakeGameV3.Interfaces;
using System.Collections;
using System.Diagnostics;
using System.Drawing;

namespace SnakeGameV3.Data
{
    internal class Snake : IMovable, IGridObject, IEnumerable<KeyValuePair<Point, ConsoleColor>>
    {
        public Snake(int x, int y, ConsoleColor headColor, ConsoleColor bodyColor, double speed)
        {
            Head = new Point(x, y);
            HeadColor = headColor;
            BodyColor = bodyColor;
            Speed = speed;

            Body.Enqueue(new Point(Head.X - 2, Head.Y));
            Body.Enqueue(new Point(Head.X - 1, Head.Y));
        }

        private readonly Stopwatch _stopwatch = Stopwatch.StartNew();

        private bool _isAte = false;

        public Point Head { get; private set; }

        public Queue<Point> Body { get; } = new();

        public ConsoleColor HeadColor { get; }

        public ConsoleColor BodyColor { get; }

        public double LostMoves { get; private set; } = 0;

        public double Speed { get; }

        public double MoveLatency => 1000 / Speed;

        public bool IsReadyForMove => _stopwatch.ElapsedMilliseconds >= MoveLatency;

        public bool IsCrashed { get; set; } = false;

        public PassType Type => PassType.Impassable;

        public void Move(Direction? direction)
        {
            if (direction is not Direction.Up
                and not Direction.Down
                and not Direction.Left
                and not Direction.Right
                and not null)
                throw new Exception();

            LostMoves += _stopwatch.ElapsedMilliseconds / MoveLatency - 1;

            _stopwatch.Restart();

            if (direction == null)
                return;

            if (!_isAte)
                Body.Dequeue();
            else
                _isAte = false;

            Body.Enqueue(new Point(Head.X, Head.Y));

            Head = direction switch
            {
                Direction.Up => new Point(Head.X, Head.Y - 1),
                Direction.Down => new Point(Head.X, Head.Y + 1),
                Direction.Left => new Point(Head.X - 1, Head.Y),
                Direction.Right => new Point(Head.X + 1, Head.Y),
                _ => throw new NotImplementedException(),
            };
        }

        public bool TryToEat(Food food)
        {
            if (Head != food.Point)
                return false;

            _isAte = true;

            return true;
        }

        public IEnumerator<Point> GetEnumerator()
        {
            yield return Head;

            foreach (Point point in Body)
                yield return point;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            yield return Head;

            foreach (Point point in Body)
                yield return point;
        }

        IEnumerator<KeyValuePair<Point, ConsoleColor>> IEnumerable<KeyValuePair<Point, ConsoleColor>>.GetEnumerator()
        {
            yield return new KeyValuePair<Point, ConsoleColor>(Head, HeadColor);

            foreach (Point point in Body)
                yield return new KeyValuePair<Point, ConsoleColor>(point, BodyColor);
        }
    }
}
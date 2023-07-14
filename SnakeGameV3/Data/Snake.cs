using SnakeGameV3.Enums;
using SnakeGameV3.Interfaces;
using System.Collections;
using System.Diagnostics;
using System.Drawing;

namespace SnakeGameV3.Data
{
    internal class Snake : IMovable, IEnumerable<Point>
    {
        public Snake(int x, int y, ConsoleColor headColor, ConsoleColor bodyColor, double speed)
        {
            Head = new Point(x, y);
            Body.Enqueue(new Point(Head.X - 2, Head.Y));
            Body.Enqueue(new Point(Head.X - 1, Head.Y));
            HeadColor = headColor;
            BodyColor = bodyColor;
            Speed = speed;
        }

        private readonly Stopwatch _stopwatch = Stopwatch.StartNew();

        public Point Head { get; private set; }

        public Queue<Point> Body { get; } = new();

        public double LostMoves { get; private set; } = 0;

        public ConsoleColor HeadColor { get; }
        public ConsoleColor BodyColor { get; }

        public double Speed { get; }

        public bool IsReadyForMove => _stopwatch.ElapsedMilliseconds >= MoveLatency;

        public double MoveLatency => 1000 / Speed;

        public void Move(Direction direction)
        {
            if (direction != Direction.Up
                && direction != Direction.Down
                && direction != Direction.Left
                && direction != Direction.Right
                && direction != Direction.Nowhere)
                throw new Exception();

            if (direction == Direction.Nowhere)
            {
                _stopwatch.Restart();
                return;
            }

            LostMoves += _stopwatch.ElapsedMilliseconds / MoveLatency - 1;

            _stopwatch.Restart();

            Body.Dequeue();
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
    }
}
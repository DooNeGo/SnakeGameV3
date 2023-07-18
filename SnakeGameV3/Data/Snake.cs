using SnakeGameV3.Enums;
using SnakeGameV3.Interfaces;
using SnakeGameV3.Rendering;
using System.Collections;
using System.Diagnostics;
using System.Drawing;

namespace SnakeGameV3.Data
{
    internal class Snake : IMovable, IGridObject, IEnumerable<PointWithColor>
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

        IEnumerator<PointWithColor> IEnumerable<PointWithColor>.GetEnumerator()
        {
            yield return new PointWithColor(Head, HeadColor);

            foreach (Point point in Body)
                yield return new PointWithColor(point, BodyColor);
        }
    }
}
using SnakeGameV3.Data;
using SnakeGameV3.Enums;
using SnakeGameV3.Rendering;
using System.Diagnostics;
using static SnakeGameV3.Constants.GameConstants;

namespace SnakeGameV3.Controllers
{
    internal class GameController
    {
        private ConsoleKey _pressedKey = new();

        public void StartGame()
        {
            PrepareConsole();

            Direction currentDirection = Direction.Null;

            Stopwatch stopwatch = Stopwatch.StartNew();

            Grid grid = new(ScreenHeight, ScreenWidth, GridCellSize);

            Snake snake = new(3, 4, SnakeHeadColor, SnakeBodyColor, SnakeSpeed);
            Food food = new(FoodColor, grid);
            Boarder boarder = new(grid, BoarderColor);

            ConsoleFrameBuilder builder = new(grid, ScreenHeight, ScreenWidth, BackgroundColor);
            ShapeFactory factory = new(grid.CellSize);

            grid.Add(boarder);
            grid.Add(food);
            grid.Add(snake);

            builder.Add(boarder);
            builder.Add(food);
            builder.Add(snake);

            while (true)
            {
                while (snake.IsReadyForMove || snake.LostMoves >= 1)
                {
                    currentDirection = ReadMovement(currentDirection);
                    snake.Move(currentDirection);
                    grid.Update();
                }

                if (snake.IsCrashed)
                    return;

                if (stopwatch.ElapsedMilliseconds < FrameDelay)
                    continue;

                stopwatch.Restart();

                builder.BuildImage();
                builder.DrawImage();
            }
        }

        private void PrepareConsole()
        {
            Console.CursorVisible = false;
            Console.SetWindowSize(ScreenWidth, ScreenHeight);
            Console.SetBufferSize(ScreenWidth, ScreenHeight);
        }

        private Direction ReadMovement(Direction lastDirection)
        {
            if (Console.KeyAvailable)
                _pressedKey = Console.ReadKey().Key;

            return _pressedKey switch
            {
                ConsoleKey.UpArrow when lastDirection != Direction.Down => Direction.Up,
                ConsoleKey.DownArrow when lastDirection != Direction.Up => Direction.Down,
                ConsoleKey.LeftArrow when lastDirection != Direction.Right => Direction.Left,
                ConsoleKey.RightArrow when lastDirection != Direction.Left => Direction.Right,
                ConsoleKey.Spacebar => Direction.Null,
                _ => lastDirection
            };
        }
    }
}

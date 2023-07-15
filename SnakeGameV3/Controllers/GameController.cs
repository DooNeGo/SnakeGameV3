using SnakeGameV3.Data;
using SnakeGameV3.Enums;
using SnakeGameV3.Rendering;
using System.Diagnostics;
using System.Drawing;
using static SnakeGameV3.Constants.GameConstants;

namespace SnakeGameV3.Controllers
{
    internal class GameController
    {
        private ConsoleKey _pressedKey = new();

        public void StartGame()
        {
            PrepareConsole();

            Direction currentDirection = Direction.Nowhere;

            Grid grid = new(ScreenHeight, ScreenWidth, GridCellSize);
            Snake snake = new(3, 4, SnakeHeadColor, SnakeBodyColor, SnakeSpeed);
            Food food = new(FoodColor, grid);
            Boarder boarder = new(grid, BoarderColor);
            ConsoleImageBuilder builder = new(grid, ScreenHeight, ScreenWidth, BackgroundColor);
            ShapeFactory factory = new(grid.CellSize);
            Stopwatch stopwatch = Stopwatch.StartNew();
            List<ConsoleObject> renderList = new();

            grid.GameObjectsCoordinates.Add(snake);
            grid.GameObjectsCoordinates.Add(food);
            grid.GameObjectsCoordinates.Add(boarder);

            while (true)
            {
                while (snake.IsReadyForMove || snake.LostMoves >= 1)
                {
                    currentDirection = ReadMovement(currentDirection);
                    snake.Move(currentDirection);
                    grid.Update();
                }

                if (stopwatch.ElapsedMilliseconds < FrameDelay)
                    continue;

                stopwatch.Restart();

                foreach (Point coordinates in boarder)
                    renderList.Add(new ConsoleObject(coordinates, factory.GetSquare(boarder.Color)));
                renderList.Add(new ConsoleObject(food.Coordinates, factory.GetSquare(food.Color)));
                renderList.Add(new ConsoleObject(snake.Head, factory.GetSquare(snake.HeadColor)));
                foreach (Point coordinates in snake.Body)
                    renderList.Add(new ConsoleObject(coordinates, factory.GetSquare(snake.BodyColor)));

                builder.BuildImage(renderList);
                builder.DrawImage();

                renderList.Clear();
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
                ConsoleKey.Spacebar => Direction.Nowhere,
                _ => lastDirection
            };
        }
    }
}

using SnakeGameV3.Constants;
using SnakeGameV3.Data;
using SnakeGameV3.Enums;
using SnakeGameV3.Interfaces;
using SnakeGameV3.Rendering;
using System.Diagnostics;
using System.Drawing;

namespace SnakeGameV3.Controllers
{
    internal class GameController
    {
        private readonly GameConstants _constants = new();

        private ConsoleKey _pressedKey = new();

        public void StartGame()
        {
            PrepareConsole();

            Direction currentDirection = Direction.Nowhere;

            Grid grid = new(_constants.ScreenHeight, _constants.ScreenWidth, _constants.GridCellSize);
            Snake snake = new(3, 4, _constants.SnakeHeadColor, _constants.SnakeBodyColor, _constants.SnakeSpeed);
            ConsoleImageBuilder builder = new(grid, _constants.ScreenHeight, _constants.ScreenWidth, _constants.BackgroundColor);
            ShapeFactory factory = new(grid.CellSize);
            Stopwatch stopwatch = Stopwatch.StartNew();

            List<IRenderable> renderList = new();

            while (true)
            {
                while (snake.IsReadyForMove || snake.LostMoves >= 1)
                {
                    currentDirection = ReadMovement(currentDirection);
                    snake.Move(currentDirection);
                }

                if (stopwatch.ElapsedMilliseconds < _constants.FrameDelay)
                    continue;

                stopwatch.Restart();

                renderList.Add(new RenderGameObject(snake.Head, factory.GetSquare(snake.HeadColor)));
                foreach (Point coordinates in snake.Body)
                    renderList.Add(new RenderGameObject(coordinates, factory.GetSquare(snake.BodyColor)));

                builder.BuildImage(renderList);
                builder.DrawImage();

                renderList.Clear();
            }
        }

        private void PrepareConsole()
        {
            Console.CursorVisible = false;
            Console.SetWindowSize(_constants.ScreenWidth, _constants.ScreenHeight);
            Console.SetBufferSize(_constants.ScreenWidth, _constants.ScreenHeight);
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

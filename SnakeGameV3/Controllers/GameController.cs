using SnakeGameV3.Data;
using SnakeGameV3.Rendering;
using System.Diagnostics;
using static SnakeGameV3.Constants.GameConstants;

namespace SnakeGameV3.Controllers
{
    internal class GameController
    {
        public void StartGame()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            Grid grid = new(ScreenHeight, ScreenWidth, GridCellSize);

            Food food = new(FoodColor, grid);
            Boarder boarder = new(grid, BoarderColor);

            Snake snake = new(3, 4, SnakeHeadColor, SnakeBodyColor, SnakeSpeed);
            SnakeMovement movement = new(snake);
            KeyboardInput input = new(movement);

            ConsoleFrameBuilder builder = new(grid, ScreenHeight, ScreenWidth, BackgroundColor);

            grid.Add(boarder);
            grid.Add(food);
            grid.Add(snake);

            builder.Add(boarder);
            builder.Add(food);
            builder.Add(snake);

            while (!snake.IsCrashed)
            {
                if (stopwatch.ElapsedMilliseconds >= FrameDelay)
                {
                    stopwatch.Restart();


                    builder.BuildImage();
                    builder.DrawImage();

                }

                grid.Update();
                input.Update();
            }
        }
    }
}

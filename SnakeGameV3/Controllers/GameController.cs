using SnakeGameV3.Data;
using SnakeGameV3.Input;
using SnakeGameV3.Model;
using SnakeGameV3.Rendering;
using System.Diagnostics;
using System.Numerics;
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
            Snake snake = new(new Vector2(3, 4), SnakeHeadColor, SnakeBodyColor, SnakeSpeed, grid);
            SnakeMovement movement = new(snake);
            KeyboardInput input = new(movement);
            ConsoleFrameBuilder builder = new(grid, ScreenHeight, ScreenWidth, BackgroundColor);

            grid.Add(boarder);
            grid.Add(food);
            grid.Add(snake);

            builder.Add(boarder);
            builder.Add(food);
            builder.Add(snake);

            while (!snake.IsDied)
            {
                if (stopwatch.ElapsedMilliseconds >= FrameDelay)
                {
                    stopwatch.Restart();

                    builder.BuildImage();
                    builder.DrawImage();
                }

                grid.Update();
                input.Update();

                if (snake.TryToEat(food))
                    food.RandCoordinates();
            }
        }
    }
}

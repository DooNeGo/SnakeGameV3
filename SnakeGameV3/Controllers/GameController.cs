using static SnakeGameV3.GameConstants;

using SnakeGameV3.Model;
using SnakeGameV3.Rendering;
using System.Diagnostics;
using System.Drawing;
using System.Numerics;

namespace SnakeGameV3.Controllers
{
    internal class GameController
    {
        private readonly Grid _grid;
        private readonly Food _food;
        private readonly Snake _snake;
        private readonly Boarder _boarder;
        private readonly ConsoleFrameBuilder _builder;

        public GameController()
        {
            Size screenSize = new(ScreenWidth, ScreenHeight);

            _grid = new(screenSize, new Size(GridCellWidth, GridCellHeight));
            _food = new(FoodColor, _grid);
            _snake = new(new Vector2(3, 4), SnakeHeadColor, SnakeBodyColor, SnakeSpeed, _grid);
            _boarder = new(_grid, BoarderColor);
            _builder = new(_grid, screenSize, BackgroundColor);
        }

        public void StartGame()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            PhysicsMovement snakeMovement = new(_snake);
            KeyboardInput input = new(snakeMovement);

            _food.RandCoordinates();

            _grid.Add(_snake);
            _grid.Add(_boarder);
            _grid.Add(_food);

            _builder.Add(_food);
            _builder.Add(_snake);
            _builder.Add(_boarder);

            _snake.Die += OnSnakeDie;

            while (!_snake.IsCrashed)
            {
                if (stopwatch.ElapsedMilliseconds >= FrameDelay)
                {
                    stopwatch.Restart();

                    _builder.UpdateFrame();
                    _grid.Update();
                    input.Update();
                }
            }
        }

        private void OnSnakeDie()
        {
            _grid.Remove(_snake);
            _grid.Remove(_food);
            _builder.Remove(_snake);
            _builder.Remove(_food);

            _builder.UpdateFrame();

            _snake.Die -= OnSnakeDie;

            Console.ReadKey();
        }
    }
}

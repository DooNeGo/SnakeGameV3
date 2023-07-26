using SnakeGameV3.Input;
using SnakeGameV3.Model;
using SnakeGameV3.Movements;
using SnakeGameV3.Rendering;
using System.Diagnostics;
using System.Numerics;
using static SnakeGameV3.Constants.GameConstants;

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
            _grid = new(ScreenHeight, ScreenWidth, GridCellSize); ;
            _food = new(FoodColor, _grid);
            _snake = new(new Vector2(3, 4), SnakeHeadColor, SnakeBodyColor, SnakeSpeed, _grid);
            _boarder = new(_grid, BoarderColor);
            _builder = new(_grid, ScreenHeight, ScreenWidth, BackgroundColor);
        }

        public void StartGame()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            PhysicsMovement movement = new(_snake);
            KeyboardInput input = new(movement);

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

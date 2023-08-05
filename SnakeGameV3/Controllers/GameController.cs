using static SnakeGameV3.Config;
using SnakeGameV3.Model;
using SnakeGameV3.Rendering;
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

            _grid = new Grid(screenSize, new Size(GridCellWidth, GridCellHeight));
            _food = new Food(FoodColor, _grid);
            _snake = new Snake(new Vector2(3, 4), SnakeHeadColor, SnakeBodyColor, SnakeSpeed, _grid);
            _boarder = new Boarder(_grid, BoarderColor);
            _builder = new ConsoleFrameBuilder(_grid, screenSize, BackgroundColor);
        }

        public void StartGame()
        {
            PhysicsMovement snakeMovement = new(_snake);
            KeyboardInput input = new(snakeMovement);

            _grid.Add(_snake);
            _grid.Add(_boarder);
            _grid.Add(_food);

            _builder.Add(_food);
            _builder.Add(_snake);
            _builder.Add(_boarder);

            _grid.Update();
            _food.RandCoordinates();

            while (!_snake.IsCrashed)
            {
                if (_builder.DeltaTime.TotalMilliseconds >= FrameDelay)
                {
                    _builder.Update();
                    input.Update();
                    _grid.Update();
                }
            }

            _grid.Remove(_snake);
            _grid.Remove(_food);
            _builder.Remove(_snake);
            _builder.Remove(_food);

            _builder.Update();

            Console.ReadKey();
        }
    }
}

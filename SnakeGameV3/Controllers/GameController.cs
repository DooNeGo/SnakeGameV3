using SnakeGameV3.Model;
using SnakeGameV3.Rendering;
using System.Drawing;
using System.Numerics;
using static SnakeGameV3.Config;

namespace SnakeGameV3.Controllers
{
    internal class GameController
    {
        private readonly Grid _grid;
        private readonly Food _food;
        private readonly Snake _snake;
        private readonly ConsoleFrameBuilder _builder;
        private readonly CollisionSystem _collisionSystem;

        public GameController()
        {
            Size screenSize = new(ScreenWidth, ScreenHeight);

            _grid = new Grid(screenSize, new Size(GridCellWidth, GridCellHeight));
            _food = new Food(_grid, 1f, FoodColor);
            _snake = new Snake(new Vector2(3, 4), SnakeHeadColor, SnakeBodyColor, SnakeSpeed, _grid);
            _builder = new ConsoleFrameBuilder(screenSize, BackgroundColor, _grid);
            _collisionSystem = new CollisionSystem();
        }

        public void StartGame()
        {
            PhysicsMovement snakeMovement = new(_snake);
            KeyboardInput input = new(snakeMovement);

            _collisionSystem.Add(_snake);
            _collisionSystem.Add(_food);

            _grid.Add(_snake);
            _grid.Add(_food);

            _builder.Add(_food);
            _builder.Add(_snake);
            //_builder.Add(_grid);

            _grid.Update();

            while (!_snake.IsDied)
            {
                if (_builder.DeltaTime.TotalMilliseconds >= FrameDelay)
                {
                    input.Update();
                    _builder.Update();
                }
                _collisionSystem.Update();
            }

            Text gameOver = new("Game Over",
                ConsoleColor.DarkRed,
                _grid.Center with { X = 0 },
                _grid.Center with { X = _grid.Size.Width - 1 });

            _grid.Remove(_snake);
            _grid.Remove(_food);
            _builder.Remove(_snake);
            _builder.Remove(_food);

            _builder.Add(gameOver);
            _builder.Update();

            Console.ReadKey();
            Console.ReadKey();
        }
    }
}

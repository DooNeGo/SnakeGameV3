using SnakeGameV3.Model;
using SnakeGameV3.Rendering;
using System.Drawing;
using System.Numerics;
using static SnakeGameV3.Config;

namespace SnakeGameV3.Controllers
{
    internal class Game
    {
        private readonly Grid _grid;
        private readonly Snake _snake;
        private readonly FoodController _foodController;
        private readonly ConsoleFrameBuilder _builder;
        private readonly CollisionHandler _collisionHandler;
        private readonly Scene _mainScene;

        public Game()
        {
            Size screenSize = new(ScreenWidth, ScreenHeight);

            _grid = new Grid(screenSize, new Size(GridCellWidth, GridCellHeight));
            _snake = new Snake(new Vector2(3, 4), SnakeHeadColor, SnakeBodyColor, SnakeSpeed, _grid);
            _foodController = new FoodController(_grid);
            _mainScene = new Scene(_grid, _foodController, _snake);
            _builder = new ConsoleFrameBuilder(screenSize, BackgroundColor, _grid, _mainScene);
            _collisionHandler = new CollisionHandler(_mainScene);
        }

        public void Start()
        {
            PhysicsMovement snakeMovement = new(_snake);
            KeyboardInput input = new(snakeMovement);

            _grid.Add(_snake);
            _grid.Add(_foodController);

            while (!_snake.IsDied)
            {
                if (_builder.DeltaTime.TotalMilliseconds >= FrameDelay)
                    _builder.Update();

                input.Update();
                _grid.Update();
                _foodController.Update();
                _collisionHandler.Update();
            }

            Text gameOver = new("Game Over",
                ConsoleColor.DarkRed,
                _grid.Center with { X = 0 },
                _grid.Center with { X = _grid.Size.Width - 1 });

            _grid.Remove(_snake);
            _grid.Remove(_foodController);

            Scene gameOverScene = new(_grid, gameOver);
            ChangeActiveScene(gameOverScene);
            _builder.Update();

            Console.ReadKey();

            Text score = new($"Score: {_snake.Score}",
                ConsoleColor.White,
                _grid.Center with { X = 0 },
                _grid.Center with { X = _grid.Size.Width - 1 });

            Scene scoreScene = new(_grid, score);
            ChangeActiveScene(scoreScene);
            _builder.Update();

            Console.ReadKey();
            Console.ReadKey();
        }

        private void ChangeActiveScene(Scene scene)
        {
            _builder.ActiveScene = scene;
            _collisionHandler.ActiveScene = scene;
        }
    }
}

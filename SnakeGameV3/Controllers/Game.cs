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

        private float _timeRatio = 1;

        public Game()
        {
            Size screenSize = new(ScreenWidth, ScreenHeight);

            _grid = new Grid(screenSize, new Size(GridCellWidth, GridCellHeight));
            _snake = new Snake(new Vector2(3, 4), SnakeHeadColor, SnakeBodyColor, SnakeSpeed, _grid);
            _foodController = new FoodController(_grid);
            _mainScene = new Scene(_foodController, _snake);
            _grid.ActiveScene = _mainScene;
            _builder = new ConsoleFrameBuilder(screenSize, BackgroundColor, _grid, _mainScene);
            _collisionHandler = new CollisionHandler(_mainScene);
        }

        public void Start()
        {
            PhysicsMovement snakeMovement = new(_snake, SnakeSlewingTime);
            KeyboardInput input = new();
            input.KeyDown += OnKeyDown;
            TimeSpan timeSpan;

            while (!_snake.IsDied)
            {
                timeSpan = _builder.DeltaTime;

                input.Update();
                snakeMovement.Move(input.ReadMovement(), timeSpan * _timeRatio);
                _mainScene.Update();
                _foodController.Update(timeSpan);
                _collisionHandler.Update();
                _builder.Update();

                timeSpan = _builder.DeltaTime;

                if (FrameDelay > timeSpan.TotalMilliseconds)
                    Thread.Sleep((int)(FrameDelay - timeSpan.TotalMilliseconds));
            }

            Text gameOverText = new("Game Over",
                ConsoleColor.DarkRed,
                _grid.Center with { X = 0 },
                _grid.Center with { X = _grid.Size.Width - 1 });

            Scene gameOverScene = new(gameOverText);
            ChangeActiveScene(gameOverScene);
            _builder.Update();

            Console.ReadKey();

            Text scoreText = new($"Score: {_snake.Score}",
                ConsoleColor.White,
                _grid.Center with { X = 0 },
                _grid.Center with { X = _grid.Size.Width - 1 });

            Scene scoreScene = new(scoreText);
            ChangeActiveScene(scoreScene);
            _builder.Update();

            Console.ReadKey();
        }

        private void OnKeyDown(ConsoleKey key)
        {
            if (key == ConsoleKey.Spacebar && _timeRatio == 1)
                _timeRatio = 0;
            else if (key == ConsoleKey.Spacebar && _timeRatio == 0)
                _timeRatio = 1;
        }

        private void ChangeActiveScene(Scene scene)
        {
            _builder.ActiveScene = scene;
            _collisionHandler.ActiveScene = scene;
            _grid.ActiveScene = scene;
        }
    }
}

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
        private readonly Food _food;
        private readonly Snake _snake;
        private readonly ConsoleFrameBuilder _builder;
        private readonly CollisionHandler _collisionHandler;
        private readonly Indexer _indexer;
        private readonly Scene _mainScene;

        public Game()
        {
            Size screenSize = new(ScreenWidth, ScreenHeight);

            _indexer = new Indexer();
            _grid = new Grid(screenSize, new Size(GridCellWidth, GridCellHeight), _indexer);
            _mainScene = new Scene(_grid);
            _food = new Food(_grid, 0.5f, FoodColor, _indexer);
            _snake = new Snake(new Vector2(3, 4), SnakeHeadColor, SnakeBodyColor, SnakeSpeed, _grid, _indexer);
            _builder = new ConsoleFrameBuilder(screenSize, BackgroundColor, _grid, _mainScene);
            _collisionHandler = new CollisionHandler(_mainScene);
        }

        public void Start()
        {
            PhysicsMovement snakeMovement = new(_snake);
            KeyboardInput input = new(snakeMovement);

            _mainScene.Add(_food);
            _mainScene.Add(_snake);

            _grid.Add(_snake);
            _grid.Add(_food);

            _grid.Update();

            while (!_snake.IsDied)
            {
                if (_builder.DeltaTime.TotalMilliseconds >= FrameDelay)
                {
                    input.Update();
                    _builder.Update();
                }
                _collisionHandler.Update();
            }

            Text gameOver = new("Game Over",
                ConsoleColor.DarkRed,
                _grid.Center with { X = 0 },
                _grid.Center with { X = _grid.Size.Width - 1 },
                _indexer);

            _grid.Remove(_snake);
            _grid.Remove(_food);

            _mainScene.Remove(_food);
            _mainScene.Remove(_snake);
            _mainScene.Add(gameOver);

            _builder.Update();

            Console.ReadKey();

            Text score = new($"Score: {_snake.Score}",
                ConsoleColor.White,
                _grid.Center with { X = 0 },
                _grid.Center with { X = _grid.Size.Width - 1 },
                _indexer);

            _mainScene.Remove(gameOver);
            _mainScene.Add(score);

            _builder.Update();

            Console.ReadKey();
        }
    }
}

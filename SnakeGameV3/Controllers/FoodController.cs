using SnakeGameV3.Interfaces;
using SnakeGameV3.Model;
using System.Numerics;

namespace SnakeGameV3.Controllers
{
    internal class FoodController : ICompositeObject
    {
        private const float foodScale = 0.5f;

        private readonly Food[] _foods;
        private readonly Random _random = new();
        private readonly Grid _grid;

        private Index _activeFoodIndex = 0;
        private DateTime _activeFoodTime = DateTime.UtcNow;

        public FoodController(Grid grid)
        {
            _grid = grid;

            _foods = new Food[]
            {
                new Food(_grid.Center, foodScale, ConsoleColor.DarkRed, new Effect(1, EffectType.Length), 15),
                new Food(_grid.Center, foodScale, ConsoleColor.Blue, new Effect(0.5f, EffectType.Speed), 10),
                new Food(_grid.Center, foodScale, ConsoleColor.Blue, new Effect(-0.5f, EffectType.Speed), 10),
                new Food(_grid.Center, foodScale, ConsoleColor.DarkGreen, new Effect(0.1f, EffectType.Scale), 10),
                new Food(_grid.Center, foodScale, ConsoleColor.DarkGreen, new Effect(-0.1f, EffectType.Scale), 10),
                new Food(_grid.Center, foodScale, ConsoleColor.Magenta, new Effect(-1, EffectType.Length), 5),
            };

            for (int i = 0; i < _foods.Length; i++)
            {
                _foods[i].Collect += OnCollect;
            }
        }

        public float Scale => foodScale;

        private Food ActiveFood => _foods[_activeFoodIndex];

        public void Update()
        {
            if ((DateTime.UtcNow - _activeFoodTime).TotalSeconds >= ActiveFood.LifeTime)
            {
                RandFood();
            }
        }

        public IEnumerator<IReadOnlyGameObject> GetGameObjectsWithComponent<T>() where T : Component
        {
            if (ActiveFood.GetComponent<T>() is not null)
            {
                yield return ActiveFood;
            }
        }

        public void OnCollect(IReadOnlyGameObject gameObject)
        {
            RandFood();
        }

        private void RandFood()
        {
            int number = _random.Next(0, 11);

            if (number > 4)
            {
                _activeFoodIndex = 0;
            }
            else if (number > 0 && number <= 4)
            {
                _activeFoodIndex = _random.Next(1, _foods.Length - 1);
            }
            else if (number == 0)
            {
                _activeFoodIndex = ^1;
            }

            do
            {
                Vector2 position = new(_random.Next(0, _grid.Size.Width), _random.Next(0, _grid.Size.Height));
                _foods[_activeFoodIndex].Position = position;
            } while (_grid.IsPositionOccupied(_foods[_activeFoodIndex].Position, foodScale));

            _activeFoodTime = DateTime.UtcNow;
        }
    }
}

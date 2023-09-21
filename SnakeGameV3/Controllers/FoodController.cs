using SnakeGameV3.Components;
using SnakeGameV3.Components.Colliders;
using SnakeGameV3.Interfaces;
using SnakeGameV3.Model;
using System.Collections;
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
                new Food(_grid.Center, foodScale, ConsoleColor.DarkRed, 15),
                new Food(_grid.Center, foodScale, ConsoleColor.Blue, 10),
                new Food(_grid.Center, foodScale, ConsoleColor.Blue, 10),
                new Food(_grid.Center, foodScale, ConsoleColor.DarkGreen, 10),
                new Food(_grid.Center, foodScale, ConsoleColor.DarkGreen, 10),
                new Food(_grid.Center, foodScale, ConsoleColor.Magenta, 5),
            };

            Effect effect0 = _foods[0].AddComponent<Effect>();
            Effect effect1 = _foods[1].AddComponent<Effect>();
            Effect effect2 = _foods[2].AddComponent<Effect>();
            Effect effect3 = _foods[3].AddComponent<Effect>();
            Effect effect4 = _foods[4].AddComponent<Effect>();
            Effect effect5 = _foods[5].AddComponent<Effect>();

            effect0.Type = EffectType.Length;
            effect0.Value = 1;
            effect1.Type = EffectType.Speed;
            effect1.Value = 0.5f;
            effect2.Type = EffectType.Speed;
            effect2.Value = 0.5f;
            effect3.Type = EffectType.Scale;
            effect3.Value = 0.1f;
            effect4.Type = EffectType.Scale;
            effect4.Value = -0.1f;
            effect5.Type = EffectType.Length;
            effect5.Value = -1;

            for (int i = 0; i < _foods.Length; i++)
            {
                _foods[i].GetComponent<Collider>().CollisionEntry += OnCollisionEntry;
            }
        }

        private Food ActiveFood => _foods[_activeFoodIndex];

        public void Update()
        {
            if ((DateTime.UtcNow - _activeFoodTime).TotalSeconds >= ActiveFood.LifeTime)
            {
                RandFood();
            }
        }

        public void OnCollisionEntry(GameObject gameObject)
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
                ActiveFood.GetComponent<Transform>().Position = position;
            } while (_grid.IsPositionOccupied(ActiveFood.GetComponent<Transform>().Position, foodScale));

            _activeFoodTime = DateTime.UtcNow;
        }

        public IEnumerator<GameObject> GetGameObjectsWithComponent<T>() where T : Component
        {
            if (ActiveFood.TryGetComponent<T>() is not null)
            {
                yield return ActiveFood;
            }
        }

        public IEnumerator<GameObject> GetEnumerator()
        {
            yield return ActiveFood;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _foods.GetEnumerator();
        }
    }
}

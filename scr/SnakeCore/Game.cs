using System;
using System.Collections.Generic;
using System.Text;

namespace SnakeCore
{
    public class Game : IUpdatable
    {
        public static int TPS = 20;
        const int itemsCount = 2;
        public readonly Snake[] Snakes;
        public readonly Vector MapSize;
        public readonly List<Item> items = new List<Item>();
        public Game(Snake[] snakes, Vector mapSize)
        {
            Snakes = snakes;
            MapSize = mapSize;
            for (var i = 0; i < itemsCount; i++)
                GenerateItem();
        }

        public void Tick()
        {
            foreach(var snake in Snakes)
                snake.Tick();
            foreach(var item in items)
                item.Tick();
            while(items.Count < itemsCount)
                GenerateItem();
            CheckCollisions();
        }

        public void CheckCollisions()
        {

        }

        public void GenerateItem()
        {

        }
    }
}

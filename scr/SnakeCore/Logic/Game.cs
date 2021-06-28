using System;
using System.Collections.Generic;
using SnakeCore.Logic.Items;
using System.Text;
using System.Linq;

namespace SnakeCore.Logic
{
    public class Game : IUpdatable
    {
        public readonly Snake[] Snakes;
        public readonly Vector MapSize;
        public readonly List<Item> Items = new List<Item>();

        Random rnd = new Random();
        
        public static int TPS = 20;
        const int itemsCount = 10;
        public Game(Snake[] snakes, Vector mapSize)
        {
            Snakes = snakes;
            MapSize = mapSize;
            for (var i = 0; i < itemsCount; i++)
                GenerateItem();
        }

        public static Game GenerateGame(Vector mapSize, int playersCount)
        {
            if (mapSize.Y < 5)
                mapSize = new Vector(mapSize.X, 5);
            if (mapSize.X / (playersCount + 1) < 2)
                mapSize = new Vector((playersCount + 1) * 2, mapSize.Y);
            var snakes = new Snake[playersCount];
            var headY = mapSize.Y / 2;
            var ofset = mapSize.X / (playersCount + 1);
            for (var i = 0; i < playersCount; i++)
            {
                snakes[i] = new Snake(new Vector(1 + ofset * i, headY), Direction.Down, 3, mapSize);
            }
            return new Game(snakes, mapSize);
        }

        public bool Tick()
        {
            var changed = false;
            foreach(var snake in Snakes)
                changed = changed || snake.Tick();
            foreach(var item in Items)
                changed = changed || item.Tick();
            Items.RemoveAll(i => i.TicksToLive == 0);
            while(Items.Count < itemsCount)
            {
                GenerateItem();
                changed = true;
            }
            CheckAllCollisions();
            return changed;
        }

        public void CheckAllCollisions()
        {
            foreach(var snake in Snakes)
            {
                foreach (var snake2 in Snakes)
                    if (snake2.CheckCollision(snake))
                    {
                        snake.Kill();
                        continue;
                    }
                if (!snake.Alive)
                    continue;
                var item = Items.Find(i => i.Position == snake.Head);
                if (item != null)
                {
                    Items.Remove(item);
                    if (item.OnEnemy)
                    {
                        var othre =  GetOther(snake);
                       if (othre != null) othre.Consume(item);
                    }
                    else
                        snake.Consume(item);
                }
            }

        }

        public Snake GetOther(Snake snake)
        {
            return Snakes.First(s => !(s.Equals(snake)));
        }

        public void GenerateItem()
        {
            Item item = null;
            var rn = rnd.Next(5);
            var pos = GetFreeSpot();
            switch(rn)
            {
                case 0:
                    item = new Apple(pos);
                    break;
                case 1:
                    item = new Poison(pos);
                    break;
                case 2:
                    item = new Boots(pos);
                    break;
                case 3:
                    item = new Apple(pos);
                    break;
                case 4:
                    item = new Boots(pos);
                    break;
            }
            Items.Add(item);
        }

        public Vector GetFreeSpot()
        {
            while(true)
            {
                var x = rnd.Next(MapSize.X);
                var y = rnd.Next(MapSize.Y);
                var point = new Vector(x, y);
                if (IsFree(point))
                    return point;
            }
        }

        public bool IsFree(Vector point)
        {
            foreach (var snake in Snakes)
                if (snake.CheckCollision(point))
                    return false;
            foreach (var item in Items)
                if (item.Position == point)
                    return false;
            return true;
        }
    }
}

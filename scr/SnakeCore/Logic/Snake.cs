using SnakeCore.Logic.Items;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SnakeCore.Logic
{
    public class Snake : IUpdatable
    {
        public bool Alive {get; private set;} = true;
        public Vector Head => Body.First.Value;
        public Direction Direction { get; private set; } = Direction.Up;
        public LinkedList<Vector> Body {get; private set;}
        public int TicksPassed {get; private set; } = 0;
        public double Speed { get; private set; } = 2.5;
        public int Points => Body.Count;
        public readonly Vector MapSize;
        public List<Item> ActiveItems = new List<Item>();

        public Snake(Vector head, Direction tailDirection, int length, Vector mapSize)
        {
            MapSize = mapSize;
            Body = new LinkedList<Vector>();
            Body.AddLast(head);
            for (var i = 0; i < length - 1; i++)
            {
                Body.AddLast(Body.Last.Value.AddOnRing(Vector.GetVector(tailDirection), mapSize));
            }
        }

        public void ChangeDirection(Direction direction)
        {
            var neck = Body.First.Next.Value;
            if (Head.AddOnRing(Vector.GetVector(direction), MapSize) == neck)
                return;
            Direction = direction;
        }

        public void Move()
        {
            Body.AddFirst(Head.AddOnRing(Vector.GetVector(Direction), MapSize));
            Body.RemoveLast();
        }

        public void Consume(Item item)
        {
            Speed *= item.SpeedFactor;
            ChangeSize(item.DeltaPoints + Points);
            if (item.Duration != 0)
            {
                item.TicksToLive = item.Duration;
                ActiveItems.Add(item);
            }
        }

        public bool CheckCollision(Snake snake)
        {
            foreach(var p in Body.Skip(1))
                if (p == snake.Head)
                    return true;
            if (snake != this)
                return snake.Head == Head;
            return false;
        }

        public bool CheckCollision(Vector pos)
        {
            foreach(var v in Body)
                if (v == pos)
                    return true;
            return false;
        }

        public void Kill()
        {
            Alive = false;
            Speed = 0;
        }

        public bool Tick()
        {
            TicksPassed++;
            foreach(var item in ActiveItems)
            {
                item.Tick();
                if (item.TicksToLive == 0)
                    DeactivateItem(item);
            }
            ActiveItems.RemoveAll(i => i.TicksToLive <= 0);
            if (TicksPassed * Speed > Game.TPS)
            {
                TicksPassed = 0;
                Move();
                return true;
            }
            return false;
        }

        public void DeactivateItem(Item item)
        {
            Speed /= item.SpeedFactor;
            ChangeSize(Points - item.DeltaPoints);
        }

        public void ChangeSize(int newSize)
        {
            if (newSize < 3)
                newSize = 3;
            while(newSize < Points)
                Body.RemoveLast();
            while(newSize > Points)
                Body.AddLast(Body.Last.Value);
        }
    }
}

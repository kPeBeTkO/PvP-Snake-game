using System;
using System.Collections.Generic;

namespace SnakeCore
{
    public class Snake : IUpdatable
    {
        public bool Alive {get; private set;}
        public Vector Head => Body.First.Value;
        public Direction Direction {get; private set;}
        public LinkedList<Vector> Body {get; private set;}
        public int Speed { get; private set; }
        public int Points => Body.Count + 1;
        public readonly Vector MapSize;
        int ticksPassed = 0;

        public Snake(Vector head, Vector tailDirection, int length, Vector mapSize)
        {
            MapSize = mapSize;
            Body = new LinkedList<Vector>();
            Body.AddLast(head);
            for (var i = 0; i < length - 1; i++)
            {
                Body.AddLast(Body.Last.Value.AddOnRing(tailDirection, mapSize));
            }
        }

        public void Move()
        {
            Body.AddFirst(Head.AddOnRing(Vector.GetVector(Direction), MapSize));
            Body.RemoveLast();
        }

        public void Consume(Item item)
        {

        }

        public bool CheckCollision(Vector pos)
        {
            foreach(var v in Body)
                if (v == pos)
                    return true;
            return false;
        }

        public void Tick()
        {
            ticksPassed++;
            if (ticksPassed * Speed > Game.TPS)
            {
                ticksPassed = 0;
                Move();
            }
        }
    }
}

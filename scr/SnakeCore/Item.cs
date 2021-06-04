using System;
using System.Collections.Generic;
using System.Text;

namespace SnakeCore
{
    public abstract class Item : IUpdatable
    {
        public abstract SnakeChange Change { get; }
        public int TicksToLive;
        public Vector Position;
        public bool OnEnemy;

        public abstract void Tick();
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace SnakeCore.Logic.Items
{
    public abstract class Item : IUpdatable
    {
        public double SpeedFactor { get; protected set; } = 1;
        public int DeltaPoints { get; protected set; } = 0;
        public int TicksToLive = 200;
        public Vector Position;
        public bool OnEnemy = false;
        public int Duration = 0;

        public Item(Vector pos)
        {
            Position = pos;
        }

        public virtual bool Tick()
        {
           TicksToLive--;
            return TicksToLive == 0;
        }
    }
}

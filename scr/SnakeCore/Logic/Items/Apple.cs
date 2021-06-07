using System;
using System.Collections.Generic;
using System.Text;

namespace SnakeCore.Logic.Items
{
    public class Apple : Item
    {
        public Apple(Vector pos) : base(pos)
        {
            DeltaPoints = 1;
        }

        public override void Tick()
        {
            TicksToLive--;
        }
    }
}

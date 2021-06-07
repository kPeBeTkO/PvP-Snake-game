using System;
using System.Collections.Generic;
using System.Text;

namespace SnakeCore.Logic.Items
{
    public class Poison : Item
    {
        public Poison(Vector pos) : base(pos)
        {
            OnEnemy = true;
            DeltaPoints = -2;
        }
    }
}

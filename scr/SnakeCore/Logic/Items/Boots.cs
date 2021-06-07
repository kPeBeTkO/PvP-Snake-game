using System;
using System.Collections.Generic;
using System.Text;

namespace SnakeCore.Logic.Items
{
    public class Boots : Item
    {
        public Boots(Vector pos) : base(pos)
        {
            SpeedFactor = 2;
            Duration = 100;
        }
    }
}

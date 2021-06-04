using System;
using System.Collections.Generic;
using System.Text;

namespace SnakeCore
{
    public class SnakeChange
    {
        public readonly double SpeedFactor;
        public readonly int DeltaPoints;

        public SnakeChange(double speedFactor, int deltaPoints)
        {
            SpeedFactor = speedFactor;
            DeltaPoints = deltaPoints;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace SnakeCore.Logic
{
    public enum GameState
    {
        Unknown,
        WaitingPlayers,
        Running,
        Ended,
        Victory,
        Lose
    }
}

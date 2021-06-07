using System;
using System.Collections.Generic;
using System.Text;
using SnakeCore.Logic;

namespace SnakeCore.Network.Dto
{
    public class GameStateDto
    {
        public Vector[] Player;
        public Vector[] Enemy;
        public ItemDto[] Items;
        public Vector MapSize;
    }
}

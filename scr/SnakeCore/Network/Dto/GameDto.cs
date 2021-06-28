using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SnakeCore.Logic;

namespace SnakeCore.Network.Dto
{
    public class GameDto
    {
        public SnakeDto[] Snakes;
        public ItemDto[] Items;
        public int PlayerId;
        //public GameState State;

        public static GameDto Convert(Game game, int playerId = -1)
        {
            return new GameDto()
            {
                Snakes = game.Snakes.Select(i => SnakeDto.Convert(i)).ToArray(),
                Items = game.Items.Select(i => ItemDto.Convert(i)).ToArray(),
                PlayerId = playerId
            };
        }
    }
}

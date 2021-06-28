using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SnakeCore.Logic;

namespace SnakeCore.Network.Dto
{
    public class GameStateDto
    {
        public SnakeDto[] Snakes;
        public ItemDto[] Items;
        public int PlayerId;

        public static GameStateDto Convert(Game game, int playerId = -1)
        {
            return new GameStateDto()
            {
                Snakes = game.Snakes.Select(i => SnakeDto.Convert(i)).ToArray(),
                Items = game.Items.Select(i => ItemDto.Convert(i)).ToArray(),
                PlayerId = playerId
            };
        }
    }
}

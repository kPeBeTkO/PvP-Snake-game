using SnakeCore.Logic;
using SnakeCore.Network.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThreadWorker;

namespace SnakeCore.Network
{
    public class PlayerHandler : ThreadedTask
    {
        Messaging messaging;
        Game game;
        Snake player; 
        Snake enemy;
        Queue<GameChangeEvent> queue;
        public volatile bool GameUpdated = true;
        public volatile bool Active = true;
        public PlayerHandler(Messaging messaging, Queue<GameChangeEvent> queue, Game game, Snake player, Snake enemy)
        {
            this.messaging = messaging;
            this.queue = queue;
            this.game = game;
            this.player = player;
            this.enemy = enemy;
        }

        public override string GetName()
        {
            return "PlayerHandler";
        }

        public override void Run()
        {
            while(Active)
            {
                var res = messaging.GetPlayerDirection();
                if (res.Success)
                {
                    player.ChangeDirection(res.Value);
                }
                if (GameUpdated)
                {
                    GameUpdated = false;
                    messaging.SendGameState(GetDto());
                }
            }
        }

        public void Stop()
        {
            Active = false;
            messaging.Close();
        }

        
        private GameStateDto GetDto()
        {
            return new GameStateDto()
            {
                Player = player.Body.ToArray(),
                Enemy = enemy.Body.ToArray(),
                Items = game.Items.Select(i => ItemDto.Convert(i)).ToArray(),
                PlayerDirection = player.Direction
            };
        }
    }
}

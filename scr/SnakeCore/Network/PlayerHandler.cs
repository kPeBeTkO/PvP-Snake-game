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
        int playerId;
        public volatile bool GameUpdated = true;
        public volatile bool Active = true;

        public PlayerHandler(Messaging messaging, Game game, int playerId)
        {
            this.messaging = messaging;
            this.game = game;
            this.playerId = playerId;
        }

        public override string GetName()
        {
            return "PlayerHandler";
        }

        public override void Run()
        {
            while(Active)
            {
                if ( messaging.ReciveAll())
                {
                    ProccessInput();
                }
                if (Active && GameUpdated)
                {
                    GameUpdated = false;
                    Active = messaging.Send(GameDto.Convert(game, playerId));
                }
            }
            messaging.Close();
        }

        private void ProccessInput()
        {
            while (messaging.Messages.Count > 0)
            {
                var mes = messaging.Messages.Dequeue();
                if (mes == "Disconnect")
                {
                    Stop();
                    return;
                }
            }
            while (messaging.Data.Count > 0)
            {
                var data = messaging.Data.Dequeue();
                if (data is Direction dir)
                {
                    game.Snakes[playerId].ChangeDirection(dir);
                }
            }
        }

        public void Stop()
        {
            Active = false;
            messaging.Close();
        }
    }
}

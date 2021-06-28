using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using SnakeCore.Logic;
using SnakeCore.Network.Serializers;
using SnakeCore.Network.Dto;
using ThreadWorker;
using Serialize;
using System.Diagnostics;

namespace SnakeCore.Network
{
    public class GameClient : ThreadedTask
    {
        public Direction direction;
        private Messaging messaging;
        public GameStateDto GameState;
        public GameClient(IPEndPoint address)
        {
            messaging = Messaging.Connect(address);
        }

        public override string GetName()
        {
            return "GameClient";
        }

        public override void Run()
        {
            /*var watch = new Stopwatch();
            watch.Start();
            long lasttime = 0;
            while(true)
            {
                var curtime = watch.ElapsedMilliseconds;
                if (curtime - lasttime >= 1000 / Game.TPS)
                {
                    lasttime = curtime;
                    GameState = messaging.GetGameState(direction);
                }
            }*/
        }
    }
}

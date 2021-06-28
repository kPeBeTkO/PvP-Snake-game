using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using ThreadWorker;
using SnakeCore.Logic;
using SnakeCore.Network.Dto;
using System.Diagnostics;
using System.Linq;

namespace SnakeCore.Network
{
    class GameServer : ThreadedTask
    {
        readonly PlayerHandler[] handlers;
        readonly Game game;
        public volatile bool Active = true;

        public GameServer(Messaging[] players, Game game)
        {
            this.game = game;
            if (players.Length > game.Snakes.Length)
                throw new Exception("too much players");
            handlers = new PlayerHandler[players.Length];
            for (var i = 0; i < players.Length; i++)
            {
                players[i].Send(game.MapSize);
                handlers[i] =  new PlayerHandler(players[i],  game, i);
            }
            var disp = ThreadDispatcher.GetInstance();
            foreach(var h in handlers)
                disp.AddInQueue(h);
        }

        public override string GetName()
        {
            return "GameServer";
        }

        public override void Run()
        {
            var watch = new Stopwatch();
            watch.Start();
            long lasttime = 0;
            while(Active)
            {
                var curtime = watch.ElapsedMilliseconds;
                if (curtime - lasttime >= 1000 / Game.TPS)
                {
                    lasttime = curtime;
                    var changed = game.Tick();
                    if (changed)
                        foreach(var handler in handlers)
                            if (handler.Active)
                                handler.GameUpdated = true;
                            else
                                Active = false;
                }
            }
            foreach(var handler in handlers)
                handler.Stop();
        }

    }
}

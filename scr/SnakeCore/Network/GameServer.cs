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
        readonly Queue<GameChangeEvent> eventQueue = new Queue<GameChangeEvent>();
        public volatile bool Active = true;

        public GameServer(Messaging player1, Messaging player2)
        {
            var mapSize = new Vector(20, 20);
            var snake1 = new Snake(new Vector(4, 5), new Vector(0, -1), 3, mapSize);
            var snake2 = new Snake(new Vector(9, 5), new Vector(0, -1), 3, mapSize);
            game = new Game(new Snake[]{ snake1, snake2 }, mapSize);
            handlers = new PlayerHandler[]
            {
                new PlayerHandler(player1, eventQueue, game, snake1, snake2),
                new PlayerHandler(player2, eventQueue, game, snake2, snake1)
            };
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

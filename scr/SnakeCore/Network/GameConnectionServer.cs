using System;
using System.Collections.Generic;
using System.Text;
using SnakeCore.Network.Serializers;
using System.Net.Sockets;
using ThreadWorker;
using Serialize;
using System.Net;
using SnakeCore.Logic;
using SnakeCore.Network.Dto;

namespace SnakeCore.Network
{
    public class GameConnectionServer : ThreadedTask
    {
        public readonly Socket Server;
        public readonly ThreadDispatcher Dispatcher;
        private volatile bool active = true;
        private LocalConnectionFinder localFinder;
        private int playersCount;
        private bool oneGame;
        private Vector mapSize;

        public GameConnectionServer(string hostname, Vector mapSize, int playersCount, IPAddress ip, int port, bool oneGame = true)
        {
            var invite = new InviteDto(hostname, port);

            if (ip == null)
                ip = IPAddress.Any;

            this.mapSize = mapSize;
            this.playersCount = playersCount;
            this.oneGame = oneGame;

            Server = new Socket(SocketType.Stream, ProtocolType.Tcp);
            Server.Bind(new IPEndPoint(ip, port));
            Server.Listen(playersCount);
            
            Dispatcher = ThreadDispatcher.GetInstance();
            localFinder = new LocalConnectionFinder(invite);
        }

        public override string GetName()
        {
            return "GameConnectionServer" + players.ToString();
        }

        int players = 0;

        public override void Run()
        {
            Dispatcher.AddInQueue(localFinder);
            var connectedPlayers = new Queue<Messaging>();
            while (active)
            {
                Socket handler = null;
                try
                {
                    handler = Server.Accept();
                }
                catch{}
                if (handler != null)
                {
                    var player = new Messaging(handler);
                    if(player.IsConnected())
                    {
                        player.Send(mapSize);
                        connectedPlayers.Enqueue(player);
                        players++;
                    }
                    else
                        player.Close();
                }
                if (connectedPlayers.Count >= playersCount)
                {
                    var game = Game.GenerateGame(mapSize, playersCount);
                    var players = new Messaging[playersCount];
                    for (var i = 0; i < playersCount; i++)
                    {
                        players[i] = connectedPlayers.Dequeue();
                    }
                    Dispatcher.AddInQueue(new GameServer(players, game));
                    if (oneGame)
                        active = false;
                }
            }
            localFinder.Stop();
        }

        public void Stop()
        {
            active = false;
            Server.Close();
            localFinder.Stop();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using SnakeCore.Network.Serializers;
using System.Net.Sockets;
using ThreadWorker;
using Serialize;
using System.Net;
using SnakeCore.Logic;

namespace SnakeCore.Network
{
    public class GameConnectionServer : ThreadedTask
    {
        public readonly Socket Server;
        public readonly ThreadDispatcher Dispatcher;
        private volatile bool active = true;
        private LocalConnectionFinder localFinder;

        public GameConnectionServer(IPEndPoint addres)
        {
            Dispatcher = ThreadDispatcher.GetInstance();
            Server = new Socket(SocketType.Stream, ProtocolType.Tcp);
            Server.Bind(addres);
            Server.Listen(10);
            
            localFinder = new LocalConnectionFinder(new Dto.InviteDto("Hi!", addres.Port));
            Dispatcher.AddInQueue(localFinder);
            Dispatcher.AddInQueue(this);
        }

        public override string GetName()
        {
            return "GameConnectionServer" + Server.LocalEndPoint.ToString();
        }

        public override void Run()
        {
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
                        connectedPlayers.Enqueue(player);
                    else
                        handler.Close();
                    if (connectedPlayers.Count > 1)
                    {
                        var game = Game.GenerateGame(new Vector(20, 15), 2);
                        Dispatcher.AddInQueue(new GameServer( new Messaging[]{connectedPlayers.Dequeue(), connectedPlayers.Dequeue() }, game));
                    }
                }
            }
        }

        public void Stop()
        {
            active = false;
            Server.Close();
        }
    }
}

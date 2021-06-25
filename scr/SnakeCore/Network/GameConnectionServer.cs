using System;
using System.Collections.Generic;
using System.Text;
using SnakeCore.Network.Serializers;
using System.Net.Sockets;
using ThreadWorker;
using Serialize;
using System.Net;

namespace SnakeCore.Network
{
    public class GameConnectionServer : ThreadedTask
    {
        public readonly Socket Server;
        public readonly ThreadDispatcher Dispatcher;
        private volatile bool active = true;
        private Serializer serializer;
        private LocalConnectionFinder localFinder;

        public GameConnectionServer(IPEndPoint addres)
        {
            Dispatcher = ThreadDispatcher.GetInstance();
            Server = new Socket(SocketType.Stream, ProtocolType.Tcp);
            Server.Bind(addres);
            Server.Listen(10);
            
            serializer = new Serializer();
            serializer.AddCustom(new VectorSerializer());
            serializer.AddCustom(new DirectionSerializer());
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
                try{handler = Server.Accept();}
                catch{}
                if (handler != null)
                {
                    var player = new Messaging(handler, serializer);
                    if(player.IsConnected())
                        connectedPlayers.Enqueue(player);
                    if (connectedPlayers.Count > 1)
                    {
                        Dispatcher.AddInQueue(new GameServer(connectedPlayers.Dequeue(), connectedPlayers.Dequeue()));
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

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

        public GameConnectionServer(ThreadDispatcher dispatcher, IPEndPoint addres)
        {
            Dispatcher = dispatcher;
            Server = new Socket(SocketType.Stream, ProtocolType.Tcp);
            Server.Bind(addres);
            Server.Listen(10);
            
            serializer = new Serializer();
            serializer.AddCustom(new VectorSerializer());
        }

        public override string GetName()
        {
            return "GameConnectionServer";
        }

        public override void Run()
        {
            var connectedPlayers = new Queue<PlayerMessaging>();
            while (active)
            {
                Socket handler = null;
                try{handler = Server.Accept();}
                catch{}
                if (handler != null)
                {
                    var player = new PlayerMessaging(handler, serializer);
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

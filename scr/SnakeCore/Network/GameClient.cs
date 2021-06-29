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
using System.Net.NetworkInformation;
using System.Linq;

namespace SnakeCore.Network
{
    public class GameClient : ThreadedTask
    {
        public Direction SnakeDirection;
        private Direction oldDirection;
        public GameDto GameState;
        private Messaging server;
        public bool Active {get; private set;} = true;
        public readonly Vector MapSize;
        public readonly int PlayersCount;

        public static event Action Updated;

        private GameClient(Messaging server)
        {
            this.server = server;
            while (true)
            {
                server.ReciveAll();
                if (server.Data.Count > 1)
                {
                    MapSize = (Vector)server.Data.Dequeue();
                    PlayersCount = (int)server.Data.Dequeue();
                    break;
                }
            }
        }

        public static GameClient Connect(IPEndPoint address)
        {
            var server = Messaging.Connect(address);
            if (server != null)
            {
                var client = new GameClient(server);
                var dispatcher = ThreadDispatcher.GetInstance();
                dispatcher.AddInQueue(client);
                return client;
            }
            return null;
        }

        public static GameClient Host(string hostname, Vector mapSize, int playersCount)
        {
            //var port = NextFreePort();
            var port = 9000;
            var serverConnection = new GameConnectionServer(hostname, mapSize, playersCount, IPAddress.Any, port);
            var dispatcher = ThreadDispatcher.GetInstance();
            dispatcher.AddInQueue(serverConnection);
            var server = Messaging.Connect(new IPEndPoint(IPAddress.Loopback, port));
            var client = new GameClient(server);
            dispatcher.AddInQueue(client);
            return client;
        }

        public override string GetName()
        {
            return "GameClient";
        }

        public override void Run()
        {
            while(Active)
            {
                var updated = server.ReciveAll();
                if (updated)
                {
                    while(server.Data.Count > 0)
                    {
                        var obj = server.Data.Dequeue();
                        if (obj is GameDto game)
                            GameState = game;
                        Updated.Invoke();
                    }
                    while(server.Messages.Count > 0)
                    {
                        var mes = server.Messages.Dequeue();
                        if (mes == "Disconnect")
                        {
                            server.Close();
                            Active = false;
                            break;
                        }

                    }
                    oldDirection = GameState.Snakes[GameState.PlayerId].Direction;
                }
                if (SnakeDirection != oldDirection)
                {
                    oldDirection = SnakeDirection;
                    server.Send(oldDirection);
                }
            }
        }

        public GameState IsVictory()
        {
            if (GameState == null || GameState.State != Logic.GameState.Ended)
                return Logic.GameState.Unknown;
            if (GameState.Snakes.Length > 1)
            {
                var maxPoints = GameState.Snakes.Max(s => s.Body.Length);
                if (GameState.Snakes[GameState.PlayerId].Body.Length == maxPoints)
                    return Logic.GameState.Victory;
            }
            else if (GameState.Snakes[GameState.PlayerId].Body.Length == MapSize.X * MapSize.Y)
                    return Logic.GameState.Victory;
            return Logic.GameState.Lose;
        }

        static bool IsFree(int port)
        {
            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] listeners = properties.GetActiveTcpListeners();
            int[] openPorts = listeners.Select(item => item.Port).ToArray<int>();
            return openPorts.All(openPort => openPort != port);
        }

        static int NextFreePort(int port = 0) 
        {
            port = (port > 0) ? port : new Random().Next(1, 65535);
            while (!IsFree(port)) {
                port += 1;
            }
            return port;
        }
    }
}

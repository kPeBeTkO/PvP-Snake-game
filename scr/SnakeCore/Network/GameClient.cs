﻿using System;
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
        public GameStateDto GameState;
        private Messaging server;
        public readonly Vector MapSize;

        private GameClient(Messaging server)
        {
            this.server = server;
            server.ReciveAll();
            MapSize = (Vector)server.Data.Dequeue();
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
            var port = NextFreePort();
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
            while(true)
            {
                var updated = server.ReciveAll();
                if (updated)
                {
                    GameState = (GameStateDto)server.Data.Dequeue();
                }
                if (SnakeDirection != oldDirection)
                {
                    oldDirection = SnakeDirection;
                    server.Send(oldDirection);
                }
            }
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

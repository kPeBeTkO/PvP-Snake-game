using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using SnakeCore.Network.Dto;
using SnakeCore.Network.Serializers;
using SnakeCore.Logic;
using Serialize;

namespace SnakeCore.Network
{
    public class Messaging
    {
        DataTransferHandler messaging;
        public Messaging(Socket player, Serializer serializer)
        {
            messaging = new DataTransferHandler(player, serializer);
        }
        
        public static Messaging Connect(IPEndPoint addres, Serializer serializer)
        {
            var socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(addres);
            if (!socket.Connected)
                return null;
            var mes = new Messaging(socket, serializer);
            mes.ConfirmConnection();
            return mes;
        }

        public bool IsConnected()
        {
            messaging.Send("Hello");
            var ans = messaging.Recieve<string>();
            return ans == "Hello";
        }

        private bool ConfirmConnection()
        {
            var req = messaging.Recieve<string>();
            if (req == "Hello")
            {
                messaging.Send("Hello");
                return true;
            }
            return false;
        }

        public bool SendGameState(GameStateDto gameState)
        {
            messaging.Send("GameState");
            messaging.Send(gameState);
            var ans = messaging.Recieve<string>();
            return ans == "Ok";
        }

        public GameStateDto GetGameState(Direction currentDir)
        {
            messaging.Send("Direction");
            messaging.Send(currentDir);
            var ans = messaging.Recieve<string>();
            if (ans == "GameState")
            {
                var gameState = messaging.Recieve<GameStateDto>();
                messaging.Send("Ok");
                return gameState;
            }
            return null;
        }

        public Direction GetPlayerDirection()
        {
            var ans = messaging.Recieve<string>();
            if (ans == "Direction")
            {
                return  messaging.Recieve<Direction>();
            }
            return Direction.Up;
        }
    }
}

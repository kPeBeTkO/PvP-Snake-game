using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using SnakeCore.Network.Dto;
using SnakeCore.Network.Serializers;
using SnakeCore.Logic;
using System.Threading;
using Serialize;

namespace SnakeCore.Network
{
    public class Messaging
    {
        DataTransferHandler messaging;
        public Queue<string> Messages = new Queue<string>();
        public Queue<object> Data = new Queue<object>(); 
        public Messaging(Socket player)
        {
            messaging = new DataTransferHandler(player, new MessageSerializer());
        }
        
        public static Messaging Connect(IPEndPoint addres)
        {
            var socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(addres);
            if (!socket.Connected)
                return null;
            var mes = new Messaging(socket);
            if (mes.ConfirmConnection())
                return mes;
            else 
            {
                return null;
            }
        }

        public void Close()
        {
            messaging.TrySend("Disconnect");
            messaging.Close();
        }

        public bool IsConnected()
        {
            var res = messaging.TrySend("Hello");
            if (!res)
                return false;
            for (var i  =0; i < 10; i++)
            {
                var ans = messaging.TryRecieve<string>();
                if (ans.Success)
                    return ans.Value == "Hello";
                Thread.Sleep(10);
            }
            return false;
        }

        private bool ConfirmConnection()
        {
            for (var i = 0; i < 10; i++)
            {
                var req = messaging.TryRecieve<string>();
                if (req.Value == "Hello")
                {
                    messaging.Send("Hello");
                    return true;
                }
                Thread.Sleep(10);
            }
            return false;
        }

        public bool ReciveAll()
        {
            var recieved = false;
            while(true)
            {
                var result = messaging.TryRecieve();
                recieved |= result.Success;
                if (!result.Success)
                    return recieved;
                if (result.Value is string s)
                    Messages.Enqueue(s);
                else
                    Data.Enqueue(result.Value);
            }
        }

        public bool Send(object obj)
        {
            return messaging.TrySend(obj);
        }

        public bool SendGameState(GameDto gameState)
        {
            messaging.Send("GameState");
            messaging.Send(gameState);
            var ans = messaging.Recieve<string>();
            if (ans == "Ok")
                return true;
            return false;
        }

        public Result<GameDto> GetGameState()
        {
            var ans = messaging.TryRecieve<string>();
            if (ans.Success && ans.Value == "GameState")
            {
                var gameState = messaging.Recieve<GameDto>();
                messaging.Send("Ok");
                return Result.Ok(gameState);
            }
            return Result.Fail<GameDto>();
        }
        
        public bool SendDirection(Direction currentDir)
        {
            messaging.Send("Direction");
            messaging.Send(currentDir);
            var ans = messaging.Recieve<string>();
            return ans == "Ok";
        }

        public Result<Direction> GetPlayerDirection()
        {
            var ans = messaging.TryRecieve<string>();
            if (ans.Success && ans.Value == "Direction")
            {
                var dir =  messaging.Recieve<Direction>();
                messaging.Send("Ok");
                return Result.Ok(dir);
            }
            return Result.Fail<Direction>();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using SnakeCore.Network.Serializers;
using Serialize;

namespace SnakeCore.Network
{
    public class PlayerMessaging
    {
        DataTransferHandler messaging;
        public PlayerMessaging(Socket player, Serializer serializer)
        {
            messaging = new DataTransferHandler(player, serializer);
        }

        public bool IsConnected()
        {
            return true;
        }
    }
}

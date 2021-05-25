using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace Serialize
{
    public class Server
    {
        Socket server;
        public Server(IPEndPoint ip)
        {
            server = new Socket(SocketType.Stream, ProtocolType.Tcp);
            server.Bind(ip);
            server.Listen(0);

        }

        public DataTransferHandler AcceptConnection(Serializer serializer)
        {
            var handler = server.Accept();
            return new DataTransferHandler(handler, serializer);
        }
    }
}

using System;
using System.Net;
using SnakeCore.Network;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var address = new IPEndPoint(IPAddress.Loopback, 9000);
            var server = new GameConnectionServer(address);
        }
    }
}

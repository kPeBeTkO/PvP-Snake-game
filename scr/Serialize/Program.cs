using System;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net;

namespace Serialize
{
    class Program
    {
        static void Main(string[] args)
        {
            var ip = new IPEndPoint(IPAddress.Loopback, 8081);
            var server = new Server(ip);

            var serializer = new Serializer();
            serializer.AddCustom(new ThreeByteSerializer());

            var connection = server.AcceptConnection(serializer);
            var pac = new Package();
            pac.a = 1;
            pac.b = 2.1;
            pac.str = "aboba";
            pac.arr = new string[] { "abab", "asdsad", "qwerty", "lol", "", null };
            pac.sub.three = new ThreeByte(1,2,3);
            Console.WriteLine(pac);
            connection.Send(pac);
            Console.WriteLine(connection.Recieve<string>());
        }
    }
}

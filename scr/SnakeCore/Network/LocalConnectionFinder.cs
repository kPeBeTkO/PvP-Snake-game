using System;
using System.Collections.Generic;
using System.Text;
using Serialize;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using SnakeCore.Network.Dto;
using System.Net.NetworkInformation;
using System.Linq;
using ThreadWorker;

namespace SnakeCore.Network
{
    public class LocalConnectionFinder : ThreadedTask
    {
        static string HelloMessage = "Is anybody here?";
        static readonly Serializer serializer = new Serializer();
        readonly UdpClient server;
        static int serverPort = 9001;
        private bool active = true;
        private readonly InviteDto invite; 
        public LocalConnectionFinder(InviteDto invite)
        {
            server = new UdpClient(serverPort);
            server.EnableBroadcast = true;
            server.DontFragment = true;
            this.invite = invite;
        }

        public static InviteDto[] TryGetInvites()
        {
            var client = new UdpClient();
            var result = new HashSet<InviteDto>();
            var serverAddr = new IPEndPoint(IPAddress.Any, 0);
            foreach(var broadcast in GetBroadcasts())
            {
                var data = serializer.Serialize(HelloMessage);
                client.Send(data, data.Length, new IPEndPoint(broadcast, serverPort));
                Thread.Sleep(100);
                while(client.Available > 0)
                {
                    data = client.Receive(ref serverAddr);
                    InviteDto invite = null;
                    try 
                    {
                        invite = serializer.Deserialize<InviteDto>(data);
                    }
                    catch { continue; }
                    if (invite.Address == null)
                        invite.Address = serverAddr.Address.ToString();
                    result.Add(invite);
                }
            }
            return result.ToArray();
        }

        public static IPAddress[] GetBroadcasts()
        {
            var localAdresses = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
            var result = new List<IPAddress>();
            foreach(var address in localAdresses)
            {
                if (address.AddressFamily == AddressFamily.InterNetwork)
                {
                    var subAddr = GetSubnetMask(address);
                    var sub = BitConverter.ToUInt32(subAddr.GetAddressBytes(), 0);
                    var cur = BitConverter.ToUInt32(address.GetAddressBytes(), 0);
                    var broad = (sub & cur) | ~(uint.MaxValue & sub);
                    result.Add(new IPAddress(broad));
                }
            }
            return result.ToArray();
        }

        public static IPAddress GetSubnetMask(IPAddress address)
        {
            foreach (NetworkInterface adapter in NetworkInterface.GetAllNetworkInterfaces())
            {
                foreach (UnicastIPAddressInformation unicastIPAddressInformation in adapter.GetIPProperties().UnicastAddresses)
                {
                    if (unicastIPAddressInformation.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        if (address.Equals(unicastIPAddressInformation.Address))
                        {
                            return unicastIPAddressInformation.IPv4Mask;
                        }
                    }
                }
            }
            throw new ArgumentException(string.Format("Can't find subnetmask for IP address '{0}'", address));
        }

        public override string GetName()
        {
            return "LocalConnectionFinder";
        }

        public override void Run()
        {
            var clientAddr = new IPEndPoint(IPAddress.Any, 0);
            byte[] data = new byte[4];
            while(active)
            {
                try
                {
                    data = server.Receive(ref clientAddr);
                }
                catch 
                { 
                    break; 
                }
                var message = serializer.Deserialize<string>(data);
                if (message != HelloMessage)
                    continue;
                data = serializer.Serialize(invite);
                server.Send(data, data.Length, clientAddr);
            }
        }

        public void Stop()
        {
            active = false;
            server.Close();
        }
    }
}

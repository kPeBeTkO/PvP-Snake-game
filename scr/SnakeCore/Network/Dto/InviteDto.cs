using System;
using System.Collections.Generic;
using System.Text;

namespace SnakeCore.Network.Dto
{
    public class InviteDto
    {
        public string HostName;
        public int Port;
        public string Address;

        public InviteDto(){}
        
        public InviteDto(string name, int port)
        {
            HostName = name;
            Port = port;
        }

        public override int GetHashCode()
        {
            return Address.GetHashCode() ^ Port;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is InviteDto))
                return false;
            var inv = (InviteDto)obj;
            return inv.Equals(Address) && inv.Equals(Port) && inv.Equals(HostName);
        }
    }
}

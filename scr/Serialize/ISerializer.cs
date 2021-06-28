using System;
using System.Collections.Generic;
using System.Text;

namespace Serialize
{
    public interface ISerializer
    {
        byte[] Serialize(object obj);
        T Deserialize<T>(byte[] data);
        object Deserialize(byte[] data);
    }
}

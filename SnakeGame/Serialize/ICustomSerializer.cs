using System;
using System.Collections.Generic;
using System.Text;

namespace Serialize
{
    public interface ICustomSerializer
    {
        byte[] Serialize(object obj);
        object Deserialize(byte[] data, int start, int size);
        Type GetSerializedType();
    }
}

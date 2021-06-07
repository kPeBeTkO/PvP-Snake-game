using System;
using System.Collections.Generic;
using System.Text;
using SnakeCore.Logic;
using Serialize;

namespace SnakeCore.Network.Serializers
{
    public class DirectionSerializer : ICustomSerializer
    {
        public object Deserialize(byte[] data, int start, int size)
        {
            return (Direction)data[start];
        }

        public Type GetSerializedType()
        {
            return typeof(Direction);
        }

        public byte[] Serialize(object obj)
        {
            var dir = (Direction)obj;
            return new byte[] { (byte)dir };
        }
    }
}

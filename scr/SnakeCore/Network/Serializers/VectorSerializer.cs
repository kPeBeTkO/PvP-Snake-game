using System;
using System.Collections.Generic;
using System.Text;
using Serialize;
using SnakeCore.Logic;
using System.Linq;

namespace SnakeCore.Network.Serializers
{
    class VectorSerializer : ICustomSerializer
    {
        public object Deserialize(byte[] data, int start, int size)
        {
            var x = BitConverter.ToInt32(data, start);
            var y = BitConverter.ToInt32(data, start + 4);
            return new Vector(x, y);
        }

        public Type GetSerializedType()
        {
            return typeof(Vector);
        }

        public byte[] Serialize(object obj)
        {
            var vector = (Vector)obj;
            return BitConverter.GetBytes(vector.X).Concat(BitConverter.GetBytes(vector.Y)).ToArray();
        }
    }
}

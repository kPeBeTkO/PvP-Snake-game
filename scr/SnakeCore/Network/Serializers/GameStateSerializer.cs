using Serialize;
using SnakeCore.Logic;
using System;
using System.Collections.Generic;
using System.Text;

namespace SnakeCore.Network.Serializers
{
    public class GameStateSerializer : ICustomSerializer
    {
        public object Deserialize(byte[] data, int start, int size)
        {
            return (GameState)data[start];
        }

        public Type GetSerializedType()
        {
            return typeof(GameState);
        }

        public byte[] Serialize(object obj)
        {
            var dir = (GameState)obj;
            return new byte[] { (byte)dir };
        }
    }
}

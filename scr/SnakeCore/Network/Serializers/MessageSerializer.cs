using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Serialize;
using SnakeCore.Network.Dto;
using SnakeCore.Logic;

namespace SnakeCore.Network.Serializers
{
    public class MessageSerializer : ISerializer
    {
        Serializer serializer;
        public MessageSerializer()
        {
            serializer = new Serializer();
            serializer.AddCustom(new VectorSerializer());
            serializer.AddCustom(new DirectionSerializer());
            serializer.AddCustom(new GameStateSerializer());
        }

        public object Deserialize(byte[] data)
        {
            if (data.Length < 2)
                throw new Exception("Small data");
            var typeEnum = (SerializedType)data[0];
            Type type = null;
            switch(typeEnum)
            {
                case SerializedType.String:
                    type = typeof(string);
                    break;
                case SerializedType.Int:
                    type = typeof(int);
                    break;
                case SerializedType.GameDto:
                    type = typeof(GameDto);
                    break;
                case SerializedType.Direction:
                    type = typeof(Direction);
                    break;
                case SerializedType.Vector:
                    type = typeof(Vector);
                    break;
            }
            if (type == null)
                throw new Exception("Unknown type");
            var size = BitConverter.ToInt32(data, 1);
            var obj = serializer.Deserialize(type, data, 5, size);
            return obj;
        }

        public T Deserialize<T>(byte[] data)
        {
            return (T)Deserialize(data);
        }

        public byte[] Serialize(object obj)
        {
            SerializedType type = SerializedType.String;
            if (obj is string)
                type = SerializedType.String;
            else if (obj is GameDto)
                type = SerializedType.GameDto;
            else if (obj is Direction)
                type = SerializedType.Direction;
            else if (obj is Vector)
                type = SerializedType.Vector;
            else if (obj is int)
                type = SerializedType.Int;
            var typeD = new byte[]{ (byte)type };
            var data = serializer.Serialize(obj);
            return typeD.Concat(data).ToArray();
        }
    }
}

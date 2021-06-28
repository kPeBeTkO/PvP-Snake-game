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
        }

        public object Deserialize(byte[] data)
        {
            if (data.Length < 4)
                throw new Exception("Small data");
            var typeLen = BitConverter.ToInt32(data, 0);
            var typeStr = Encoding.UTF8.GetString(data, 4, typeLen);
            if (data.Length == typeLen + 4)
                return typeStr;
            Type type = null;
            switch(typeStr)
            {
                case "GameStateDto":
                    type = typeof(GameStateDto);
                    break;
                case "Direction":
                    type = typeof(Direction);
                    break;
                case "Vector":
                    type = typeof(Vector);
                    break;
            }
            if (type == null)
                throw new Exception("Unknown type");
            var size = BitConverter.ToInt32(data, typeLen + 4);
            var obj = serializer.Deserialize(type, data, typeLen + 8, size);
            return obj;
        }

        public T Deserialize<T>(byte[] data)
        {
            return (T)Deserialize(data);
        }

        public byte[] Serialize(object obj)
        {
            if (obj is string)
                return serializer.Serialize(obj);
            var type = serializer.Serialize(obj.GetType().Name);
            var data = serializer.Serialize(obj);
            return type.Concat(data).ToArray();
        }
    }
}

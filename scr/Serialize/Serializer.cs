using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Linq;

namespace Serialize
{
    public class Serializer : ISerializer
    {
        Dictionary<Type, ICustomSerializer> serializers = new Dictionary<Type, ICustomSerializer>();

        public byte[] Serialize(object obj)
        {
            if (obj == null)
                return new byte[] { 0, 0, 0, 0 };
            var type = obj.GetType();
            byte[] data;
            if (serializers.ContainsKey(type))
            {
                data = serializers[type].Serialize(obj);
                return BitConverter.GetBytes(data.Length).Concat(data).ToArray();
            }
            if (!type.IsClass || type == typeof(string))
            {
                data = SerializePrimitive(obj);
            }
            else if (type.IsArray)
            {
                data = SerializeArray((Array)obj);
            }
            else
                data = SerializeClass(obj);
            if (data == null)
                    throw new Exception("Serialization Error");
            return data;
        }

        byte[] SerializeClass(object obj)
        {
            var type = obj.GetType();
            var result = new List<byte>(){};
            foreach(var field in type.GetFields())
            {
                result.AddRange(field.Name.Select(c => (byte)c));
                result.Add((byte)':');
                var value = field.GetValue(obj);
                result.AddRange(Serialize(value));
            }
            return BitConverter.GetBytes(result.Count).Concat(result).ToArray();
        }

        byte[] SerializeArray(Array array)
        {
            var result = new List<byte>(){};
            result.AddRange(BitConverter.GetBytes(array.Length));
            foreach (var item in array)
                result.AddRange(Serialize(item));
            return BitConverter.GetBytes(result.Count).Concat(result).ToArray();
        }

        byte[] SerializePrimitive(object obj)
        {
            var data = SerializePrimitiveValue(obj);
            return BitConverter.GetBytes(data.Length).Concat(data).ToArray();
        }

        byte[] SerializePrimitiveValue(object obj)
        {
            if (obj == null)
                return new byte[0];

            var type = obj.GetType();

            if (type == typeof(byte))
                return new byte[]{ (byte)obj };

            if (type == typeof(sbyte))
                return new byte[]{ (byte)obj };

            if (type == typeof(string))
                return Encoding.UTF8.GetBytes((string)obj);

            if (type == typeof(DateTime))
                obj = ((DateTime)obj).ToBinary();

            if (type == typeof(TimeSpan))
                obj = ((TimeSpan)obj).Ticks;

            if (type == typeof(DateTimeOffset))
                obj = ((DateTimeOffset)obj).ToFileTime();

            if (type == typeof(Guid))
                return ((Guid)obj).ToByteArray();

            var bitConverter = typeof(BitConverter).GetMethod("GetBytes", new Type[]{ obj.GetType() });
            if (bitConverter != null)
            {
                return (byte[])bitConverter.Invoke(null, new object[]{ obj });
            }
            return null;
        }

        public T Deserialize<T>(byte[] data)
        {
            var size = BitConverter.ToInt32(data, 0);
            return (T)Deserialize(typeof(T), data, 4, size);
        }

        public object Deserialize(Type type, byte[] data, int start, int size)
        {
            object obj;
            if (size == 0)
                return null;
            if (serializers.ContainsKey(type))
            {
                obj = serializers[type].Deserialize(data, start, size);
            }
            else if (!type.IsClass || type == typeof(string))
            {
                obj = DeserializePrimitive(type, data, start, size);
            }
            else if (type.IsArray)
                obj = DeserializeArray(type, data, start, size);
            else
            {
                obj = DeserializeClass(type, data, start, size);
            }
            if (obj == null)
                    throw new Exception();
                return obj;
        }

        object DeserializeArray(Type type, byte[] data, int startIndex, int size )
        {
            var arraySize = BitConverter.ToInt32(data, startIndex);
            var constructor = type.GetConstructor(new Type[] { typeof(int)});
            Array result = (Array)constructor.Invoke(new object[] { arraySize });
            var elementType = type.GetElementType();
            var index = startIndex + 4;
            for(var i = 0; i < arraySize; i++)
            {
                var elementSize = BitConverter.ToInt32(data, index);
                index += 4;
                var value = Deserialize(elementType, data, index, elementSize);
                result.SetValue(value, i);
                index += elementSize;
            }
            return result;
        }

        object DeserializePrimitive(Type type, byte[] data, int startIndex, int size)
        {
             if (type == typeof(byte))
                return data[startIndex];

            if (type == typeof(sbyte))
                return data[startIndex];

            if (type == typeof(string))
            {
                var buff = new byte[size];
                for (var i = 0; i < size; i++)
                    buff[i] = data[startIndex + i];
                return Encoding.UTF8.GetString(buff);
            }

            if (type == typeof(DateTime))
            {
                var value = BitConverter.ToInt64(data, startIndex);
                return DateTime.FromBinary(value);
            }

            if (type == typeof(DateTimeOffset))
            {
                 var value = BitConverter.ToInt64(data, startIndex);
                return DateTimeOffset.FromFileTime(value);
            }

            if (type == typeof(TimeSpan))
            {
                 var value = BitConverter.ToInt64(data, startIndex);
                return TimeSpan.FromTicks(value);
            }

            if (type == typeof(Guid))
            {
                throw new NotImplementedException();
            }

            var bitConverter = typeof(BitConverter).GetMethod("To" + type.Name, new Type[]{ typeof(byte[]), typeof(int) });
            if (bitConverter != null)
            {
                return bitConverter.Invoke(null, new object[]{ data, startIndex});
            }
            return null;
        }

        object DeserializeClass(Type type, byte[] data, int start, int size)
        {
            var end = start + size;
            var constructor = type.GetConstructor(new Type[0]);
            if (constructor == null)
                throw new ArgumentException("Type " + type.FullName + " doesn't have plane constructor");
            object result = constructor.Invoke(new object[0]);

            var index = start;
            while(index < end)
            {
                var fieldName = "";
                while (data[index] != (byte)':')
                {
                    fieldName += (char)data[index];
                    index++;
                }
                index++;
                var fieldSize = BitConverter.ToInt32(data, index);
                index += 4;
                var field = type.GetField(fieldName);
                var value = Deserialize(field.FieldType, data, index, fieldSize);
                if (field != null)
                    field.SetValue(result, value);
                index += fieldSize;
            }
            return result;
        }
        
        public void AddCustom(ICustomSerializer serializer)
        {
            var type = serializer.GetSerializedType();
            serializers[type] = serializer;
        }

        public object Deserialize(byte[] data)
        {
            throw new NotImplementedException();
        }
    }
}

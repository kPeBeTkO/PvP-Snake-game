using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Net;
using System.Linq;

namespace Serialize
{
    public class DataTransferHandler
    {
        public Serializer serializer;
        readonly MD5 md5 = MD5.Create();
        readonly Socket handler;
        readonly byte[] preamble = new byte[] { 1, 1, 0, 1, 0, 1, 0, 0 };

        public DataTransferHandler(Socket handler, Serializer serializer)
        {
            this.handler = handler;
            this.serializer = serializer;
        }

        public static DataTransferHandler Connect(IPEndPoint addres, Serializer serializer)
        {
            var socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(addres);
            return new DataTransferHandler(socket, serializer);
        }

        public void Close()
        {
            handler.Close();
        }

        public void Send(object obj)
        {
            handler.Send(Encode(obj));
        }

        public bool TrySend(object obj)
        {
            try
            {
                Send(obj);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public Result<T> TryRecieve<T>()
        {
            if (handler.Available > 0)
                return Result.Ok(Recieve<T>());
            return Result.Fail<T>();
        }

        public T Recieve<T>()
        {
            var received = new byte[preamble.Length];
            handler.Receive(received);
            if (!received.SequenceEqual(preamble))
                 throw new Exception("Transmission error");

            received = new byte[4];
            handler.Receive(received);
            var size = BitConverter.ToInt32(received, 0);

            var zipData = new byte[size];
            var count = handler.Receive(zipData);
            if (count != size)
                throw new Exception("Transmission error");

            received = new byte[16];
            handler.Receive(received);
            var hash = md5.ComputeHash(zipData);
            if (!received.SequenceEqual(hash))
                throw new Exception("Transmission error");

            var data = Zip.Decompress(zipData);
            return serializer.Deserialize<T>(data);
        }

        public byte[] Encode(object obj)
        {
            var data = serializer.Serialize(obj);
            var zipData = Zip.Compress(data);
            var result = new List<byte>();
            result.AddRange(preamble);
            result.AddRange(BitConverter.GetBytes(zipData.Length));
            result.AddRange(zipData);
            result.AddRange(md5.ComputeHash(zipData));
            return result.ToArray();
        }

        public T Decode<T>(byte[] data)
        {
            var index = 0;
            for (; index < preamble.Length; index++)
                if (data[index] != preamble[index])
                    throw new Exception("Transmission error");
            var size = BitConverter.ToInt32(data, index);
            index += 4;
            var zipData = new byte[size];
            for (var i = 0; i < size; i++)
            {
                zipData[i] = data[index];
                index++;
            }
            var recievedHash = new byte[16];
            for (var i = 0; i < 16; i++)
            {
                recievedHash[i] = data[index];
                index++;
            }
            var hash = md5.ComputeHash(zipData);
            for (var i = 0; i < 16; i++)
                if (recievedHash[i] != hash[i])
                    throw new Exception("Transmission error");
            var serializedData = Zip.Decompress(zipData);
            return serializer.Deserialize<T>(serializedData);
        }
    }
}

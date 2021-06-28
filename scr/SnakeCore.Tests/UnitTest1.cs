using NUnit.Framework;
using SnakeCore.Network;
using System.Net;
using System.Threading;
using SnakeCore.Network.Dto;
using ThreadWorker;
using SnakeCore.Network.Serializers;
using SnakeCore.Logic;

namespace SnakeCore.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            /*var invite = new InviteDto("Sasha", 25565);
            var connector = new LocalConnectionFinder(invite);
            var disp = ThreadDispatcher.GetInstance();
            disp.AddInQueue(connector);
            var recieved = LocalConnectionFinder.TryGetInvites();
            Assert.IsTrue(recieved.Length > 0);
            Assert.AreEqual(invite.Port, recieved[0].Port);*/
        }

        [Test]
        public void MessageSerializerTest()
        {
            var ser = new MessageSerializer();
            var obj = new Vector(2, 5);
            var data = ser.Serialize(obj);
            var res = ser.Deserialize(data);
            Assert.AreEqual(obj, res);
        }
    }
}
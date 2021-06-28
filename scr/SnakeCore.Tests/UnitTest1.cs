using NUnit.Framework;
using SnakeCore.Network;
using System.Net;
using System.Threading;
using SnakeCore.Network.Dto;
using ThreadWorker;

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
            var invite = new InviteDto("Sasha", 25565);
            var connector = new LocalConnectionFinder(invite);
            var disp = ThreadDispatcher.GetInstance();
            disp.AddInQueue(connector);
            var recieved = LocalConnectionFinder.TryGetInvites();
            Assert.IsTrue(recieved.Length > 0);
            Assert.AreEqual(invite.Port, recieved[0].Port);
        }
    }
}
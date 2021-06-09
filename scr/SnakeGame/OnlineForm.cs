using Serialize;
using SnakeCore.Logic;
using SnakeCore.Network;
using SnakeCore.Network.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace SnakeGame
{
    class OnlineForm : StartForm
    {
        Messaging server;
        Direction direction = Direction.Up;
        Direction oldDirection = Direction.Right;
        public OnlineForm(int h, int w) : base(h, w)
        {
            var serializer = new Serializer();
            serializer.AddCustom(new VectorSerializer());
            serializer.AddCustom(new DirectionSerializer());
            //var address = new IPEndPoint(IPAddress.Parse("192.168.0.102"), 9000);
            var address = new IPEndPoint(IPAddress.Loopback, 9000);
            server = Messaging.Connect(address, serializer);
            KeyDown += ChangeDirection;
            (new Thread(Update){ IsBackground = true }).Start();
        }

        void Update()
        {
            while(true)
            {
                var res = server.GetGameState();
                if (res.Success)
                    state = res.Value;
                if (direction != oldDirection)
                {
                    oldDirection = direction;
                    server.SendDirection(oldDirection);
                }
            }
        }

        void ChangeDirection(object sender, KeyEventArgs args)
        {
            switch(args.KeyCode)
            {
                case Keys.W:
                            direction = Direction.Down;
                            break;
                case Keys.S:
                    direction = Direction.Up;
                    break;
                case Keys.A:
                    direction = Direction.Left;
                    break;
                case Keys.D:
                    direction = Direction.Right;
                    break;
            }
        }
    }
}

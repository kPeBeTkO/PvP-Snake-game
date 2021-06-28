//using Serialize;
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
using SnakeCore.Network.Dto;

namespace SnakeGame
{
    class OnlineForm : StartForm
    {
        /*GameClient client;
        public OnlineForm(int h, int w) : base(h, w)
        {
            
        }

        protected override void StartGame(int fieldHeight, int fieldWidth)
        {
            CreateAllTextures();
            CreateField(fieldHeight, fieldWidth);
            var host = true;
            if (host)
            {
                client = new GameClient("Server", new Vector(20, 15), 1);
            }
            else
            {
                var invites = new InviteDto[0];
                while (invites.Length < 1)
                {
                    invites = LocalConnectionFinder.TryGetInvites();
                }
                //var address = new IPEndPoint(IPAddress.Parse("192.168.0.102"), 9000);
                //var address = new IPEndPoint(IPAddress.Loopback, 9000);
                var address = new IPEndPoint(IPAddress.Parse(invites[0].Address), invites[0].Port);
                client = new GameClient(address);
            }
            KeyDown += ChangeDirection;
        }


        void ChangeDirection(object sender, KeyEventArgs args)
        {
            switch(args.KeyCode)
            {
                case Keys.W:
                    client.SnakeDirection = Direction.Down;
                    break;
                case Keys.S:
                    client.SnakeDirection = Direction.Up;
                    break;
                case Keys.A:
                    client.SnakeDirection = Direction.Left;
                    break;
                case Keys.D:
                    client.SnakeDirection = Direction.Right;
                    break;
            }
        }*/
    }
}

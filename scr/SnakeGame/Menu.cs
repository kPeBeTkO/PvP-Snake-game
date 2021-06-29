using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SnakeCore.Network.Dto;
using SnakeCore.Logic;
using static System.Windows.Forms.Control;
using SnakeCore.Network;
using System.Net;

namespace SnakeGame
{
    class Menu
    {
        public WMPLib.WindowsMediaPlayer WMP = new WMPLib.WindowsMediaPlayer();
        private GameClient client;
        private ControlCollection controls;
        bool connectMenu = false;
        bool hostMenu = false;
        private Button back = new Button();
        private TextBox box = new TextBox();
        private Button[] invitesButt;
        private int prevHeightMain;
        private int prevWidthMain;
        private int prevHeightConnect;
        private int prevWidthConnect;
        private int prevHeightHost;
        private int prevWidthHost;
        private Label hostNameLabel = new Label();
        private Label playerCountLabel = new Label();
        private Label fieldSizeLabel = new Label();
        private Label fieldX = new Label();
        private Label fieldY = new Label();
        private TextBox hostNameBox = new TextBox();
        private TextBox playerCountBox = new TextBox();
        private TextBox fieldSizeBox = new TextBox();
        private TextBox YBox = new TextBox();
        private TextBox XBox = new TextBox();
        private Button play = new Button();

        public Menu(Button host, Button connect, Label text, ControlCollection control, GameClient client)
        {
            this.client = client;
            controls = control;
            fieldX.Image = MultiplyTexture("Textures\\X.png", 10);
            fieldX.BackColor = Color.FromArgb(0, 100, 100, 100);

            fieldY.Image = MultiplyTexture("Textures\\Y.png", 10);
            fieldY.BackColor = Color.FromArgb(0, 100, 100, 100);

            YBox.Font = new Font("impact", 40);
            XBox.Font = new Font("impact", 40);
            hostNameBox.Font = new Font("impact", 40);
            playerCountBox.Font = new Font("impact", 40);
            fieldSizeBox.Font = new Font("impact", 40);

            hostNameLabel.Image = MultiplyTexture("Textures\\name.png", 10);
            hostNameLabel.BackColor = Color.FromArgb(0, 100, 100, 100);

            playerCountLabel.Image = MultiplyTexture("Textures\\snakes.png", 10);
            playerCountLabel.BackColor = Color.FromArgb(0, 100, 100, 100);

            fieldSizeLabel.Image = MultiplyTexture("Textures\\field.png", 10);
            fieldSizeLabel.BackColor = Color.FromArgb(0, 100, 100, 100);

            play.Image = MultiplyTexture("Textures\\play.png", 10);
            play.FlatStyle = FlatStyle.Flat;
            play.BackColor = Color.FromArgb(0, 100, 100, 100);
            play.FlatAppearance.BorderSize = 0;
            play.Location = new Point(30, 30);
            play.Click += (s, a) =>
            {
                var count = 0;
                var fieldX = 0;
                var fieldY = 0;
                if (hostNameBox.Text != null && hostNameBox.Text != "" && int.TryParse(playerCountBox.Text, out count) &&
                count > 0 && count < 4 && XBox.Text != null && int.TryParse(XBox.Text, out fieldX) &&
                XBox.Text != null && int.TryParse(XBox.Text, out fieldX) && YBox.Text != null && int.TryParse(YBox.Text, out fieldY)
                && fieldX < 100 && fieldY < 100 && fieldX > 9 && fieldY > 9)
                {
                    WMP.URL = "Sounds\\AlIkAbIr_-_Square.wav";
                    WMP.controls.play();
                    StartForm.client = GameClient.Host("Server", new Vector(fieldX, fieldY), count);
                    
                    //=================================================================================
                    StartForm.game.PrepareGame(count);
                    StartForm.game.isStart = true;
                }
            };

            box.Font = new Font("impact", 40);
            box.KeyDown += (s, a) =>
            {
                if (a is KeyEventArgs key && key.KeyCode == Keys.Enter)
                {
                    var ip = box.Text.Split(':')[0];
                    var port = int.Parse(box.Text.Split(':')[1]);
                    var address = new IPEndPoint(IPAddress.Parse(ip), port);
                    StartForm.game.PrepareGame(2);
                    StartForm.game.isStart = true;
                    StartForm.client = GameClient.Connect(address);
                }
            };

            var backText = MultiplyTexture("Textures\\back.png", 10);
            back.Image = backText;
            back.Size = new Size(backText.Width, backText.Height);
            back.FlatStyle = FlatStyle.Flat;
            back.BackColor = Color.FromArgb(0, 100, 100, 100);
            back.FlatAppearance.BorderSize = 0;
            back.Location = new Point(30, 30);
            back.Click += (s, a) =>
            {
                connectMenu = false;
                hostMenu = false;
                controls.Clear();
                prevWidthMain = 0;
                prevHeightMain = 0;
                prevWidthHost = 0;
                prevHeightHost = 0;
            };

            var textText = MultiplyTexture("Textures\\title.png", 10);
            text.Image = textText;
            text.BackColor = Color.FromArgb(0, 0, 0, 0);

            var hostText = MultiplyTexture("Textures\\host.png", 10);
            host.Image = hostText;
            host.FlatStyle = FlatStyle.Flat;
            host.BackColor = Color.FromArgb(0, 100, 100, 100);
            host.FlatAppearance.BorderSize = 0;
            host.Click += (s, a) =>
            {
                hostMenu = true;
                controls.Clear();
                prevWidthConnect = 0;
                prevHeightConnect = 0;
            };

                var connectText = MultiplyTexture("Textures\\connect.png", 10);
            connect.Image = connectText;
            connect.FlatStyle = FlatStyle.Flat;
            connect.BackColor = Color.FromArgb(0, 100, 100, 100);
            connect.FlatAppearance.BorderSize = 0;
            connect.Click += (s, a) =>
            {
                connectMenu = true;
                controls.Clear();
                prevWidthConnect = 0;
                prevHeightConnect = 0;
            };
        }

        public void DrawMenu(Graphics g, Button host, Button connect, Label text, int Height, int Width)
        {
            DrawBack(g, Height, Width);
            if (!connectMenu && !hostMenu)
            {
                if (prevHeightMain != Height || prevWidthMain != Width)
                {
                    SetMainControls(host, connect, text, Height, Width);
                    prevWidthMain = Width;
                    prevHeightMain = Height;
                }
            }
            else if (connectMenu)
            {
                if (prevHeightConnect != Height || prevWidthConnect != Width)
                {
                    SetConnectControls(Height, Width);
                    //SetConnectList(Height, Width);
                    prevWidthConnect = Width;
                    prevHeightConnect = Height;
                }
            }
            else
            {
                if (prevHeightHost != Height || prevWidthHost != Width)
                {
                    SetHostSettings(Height, Width);
                    prevWidthHost = Width;
                    prevHeightHost = Height;
                }
            }
        }

        private Bitmap MultiplyTexture(String str, int times)
        {
            var texture = new Bitmap(str);
            var bigTexture = new Bitmap(texture.Width * times, texture.Height * times);
            var g = Graphics.FromImage(bigTexture);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            g.DrawImage(texture, new Rectangle(0, 0, texture.Width * times, texture.Height * times));
            return bigTexture;
        }

        private void SetConnectList(int Height, int Width)
        {
            var invites = LocalConnectionFinder.TryGetInvites();
            if (invites.Length == 0)
                return;
            invitesButt = new Button[invites.Length];
/*            var i1 = new InviteDto();
            i1.HostName = "Play1";
            var i2 = new InviteDto();
            i2.HostName = "Play2";
            var i3 = new InviteDto();
            i3.HostName = "Play3";
            var i4 = new InviteDto();
            i4.HostName = "Play4";
            var i5 = new InviteDto();
            i5.HostName = "Play5";
            var invites = new InviteDto[5];
            invites[0] = i1;
            invites[1] = i2;
            invites[2] = i3;
            invites[3] = i4;
            invites[4] = i5;
            invitesButt = new Button[invites.Length];*/
            var size = (4 * Height) / (8 * invites.Length);
            for (int i = 0; i < invites.Length; i++)
            {
                var ip = new Button();
                ip.Text = invites[i].Address;
                ip.Font = new Font("impact", 40);
                ip.Size = new Size(Width / 2, 8 * (Width / 2) / 58);
                ip.FlatStyle = FlatStyle.Flat;
                ip.BackColor = Color.FromArgb(0, 100, 100, 100);
                ip.FlatAppearance.BorderSize = 0;
                ip.Location = new Point(30, 30);
                ip.Location = new Point(Width / 4, Height / 8 + ((i + 1) * size));
                ip.ForeColor = Color.White;
                ip.Click += (s, a) =>
                {
                    WMP.URL = "Sounds\\AlIkAbIr_-_Square.wav";
                    WMP.controls.play();
                    var address = new IPEndPoint(IPAddress.Parse(invites[0].Address), invites[0].Port);
                    StartForm.client = GameClient.Connect(address);
                };
                invitesButt[i] = ip;
            }
            for (int i = 0; i < invitesButt.Length; i++)
            {
                controls.Add(invitesButt[i]);
                //StartForm.game.isStart = true;
            }
        }

        private void SetHostSettings(int Height, int Width)
        {
            //back
            controls.Add(back);
            //name
            hostNameLabel.Size = new Size(Width / 2, 8 * (Width / 2) / 58);
            hostNameLabel.Location = new Point(0, Height / 8);
            controls.Add(hostNameLabel);

            hostNameBox.Size = new Size(Width / 4, 8 * (Width / 2) / 56);
            hostNameBox.Location = new Point(Width / 2, (Height / 8) + 15);
            controls.Add(hostNameBox);
            //player count
            playerCountLabel.Size = new Size(Width / 2, 8 * (Width / 2) / 58);
            playerCountLabel.Location = new Point(0, 5 * Height / 16);
            controls.Add(playerCountLabel);

            playerCountBox.Size = new Size(Width / 4, 8 * (Width / 2) / 56);
            playerCountBox.Location = new Point(Width / 2, (5 * Height / 16) + 15);
            controls.Add(playerCountBox);
            //field 
            fieldSizeLabel.Size = new Size(Width / 2, 8 * (Width / 2) / 58);
            fieldSizeLabel.Location = new Point(0, Height / 2);
            controls.Add(fieldSizeLabel);

            fieldX.Size = new Size(Width / 20, 8 * (Width / 2) / 58);
            fieldX.Location = new Point(Width / 2, Height / 2);
            fieldY.Size = new Size(Width / 20, 8 * (Width / 2) / 58);
            fieldY.Location = new Point(3 * Width / 4, Height / 2);
            controls.Add(fieldY);
            controls.Add(fieldX);

            XBox.Size = new Size(Width / 10, 8 * (Width / 2) / 58);
            XBox.Location = new Point(Width / 2 + fieldX.Width, Height / 2 + 15);
            YBox.Size = new Size(Width / 10, 8 * (Width / 2) / 58);
            YBox.Location = new Point(3 * Width / 4 + fieldY.Width, Height / 2 + 15);
            controls.Add(XBox);
            controls.Add(YBox);
            //play
            play.Size = new Size(Width / 2, 8 * (Width / 2) / 58);
            play.Location = new Point(Width / 4, 6 * Height / 8);
            controls.Add(play);
        }

        private void DrawBack(Graphics g, int Height, int Width)
        {
            var y = 0;
            var color = true;
            var color2 = false;
            while (y < Height)
            {
                for (int i = 0; i < 12; i++)
                {
                    if (color)
                        g.FillRectangle(StartForm.brush1, new Rectangle(Width / 12 * i, y, Width / 12, Width / 12));
                    else
                        g.FillRectangle(StartForm.brush2, new Rectangle(Width / 12 * i, y, Width / 12, Width / 12));
                    color = !color;
                }
                color = color2;
                color2 = !color;
                y += Width / 12;
            }
        }

        private void SetMainControls(Button host, Button connect, Label text, int Height, int Width)
        {
            text.Size = new Size(Width / 2, 8 * (Width / 2) / 58);
            host.Size = new Size(Width / 2, 8 * (Width / 2) / 55);
            connect.Size = new Size(Width / 2, 8 * (Width / 2) / 55);
            text.Location = new Point(Width / 4, Height / 8);
            host.Location = new Point(Width / 4, 5 * Height / 16);
            connect.Location = new Point(Width / 4, 2 * Height / 4);
            controls.Add(host);
            controls.Add(connect);
            controls.Add(text);
        }

        private void SetConnectControls(int Height, int Width)
        {
            box.Size = new Size(Width / 2, 8 * (Width / 2) / 58);
            box.Location = new Point(Width / 4, Height / 8);
            controls.Add(back);
            controls.Add(box);
        }
    }


}

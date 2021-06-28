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

namespace SnakeGame
{
    class Menu
    {
        private ControlCollection controls;
        bool secondMenu = false;
        private Button back = new Button();
        private TextBox box = new TextBox();
        private Button[] invitesButt;
        private int PrevHeight1 = 0;
        private int PrevWidth1 = 0;
        private int PrevHeight2 = 0;
        private int PrevWidth2 = 0;
        private Bitmap MultiplyTexture(String str, int times)
        {
            var texture = new Bitmap(str);
            var bigTexture = new Bitmap(texture.Width * times, texture.Height * times);
            var g = Graphics.FromImage(bigTexture);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            g.DrawImage(texture, new Rectangle(0, 0, texture.Width * times, texture.Height * times));
            return bigTexture;
        }
        public Menu(Button host, Button connect, Label text, int Height, int Width, ControlCollection control)
        {
            controls = control;

            box.Font = new Font("impact", 40);

            var backText = MultiplyTexture("Textures\\back.png", 10);
            back.Image = backText;
            back.Size = new Size(backText.Width, backText.Height);
            back.FlatStyle = FlatStyle.Flat;
            back.BackColor = Color.FromArgb(0, 100, 100, 100);
            back.FlatAppearance.BorderSize = 0;
            back.Location = new Point(30, 30);
            back.Click += (s, a) =>
            {
                secondMenu = false;
                controls.Clear();
                PrevWidth1 = 0;
                PrevHeight1 = 0;
            };

            var textText = MultiplyTexture("Textures\\title.png", 10);
            text.Image = textText;
            text.BackColor = Color.FromArgb(0, 0, 0, 0);

            var hostText = MultiplyTexture("Textures\\host.png", 10);
            host.Image = hostText;
            host.Size = new Size(Width / 4, Height / 6);
            host.FlatStyle = FlatStyle.Flat;
            host.BackColor = Color.FromArgb(0, 100, 100, 100);
            host.FlatAppearance.BorderSize = 0;
            host.Click += (s, a) =>
            {
                StartForm.game.isStart = true;
            };


            var connectText = MultiplyTexture("Textures\\connect.png", 10);
            connect.Image = connectText;
            connect.FlatStyle = FlatStyle.Flat;
            connect.BackColor = Color.FromArgb(0, 100, 100, 100);
            connect.FlatAppearance.BorderSize = 0;
            connect.Click += (s, a) =>
            {
                secondMenu = true;
                controls.Clear();
                PrevWidth2 = 0;
                PrevHeight2 = 0;
            };
        }

        public void DrawMenu(Graphics g, Button host, Button connect, Label text, int Height, int Width)
        {
            DrawBack(g, Height, Width);
            if (!secondMenu)
            {
                if (PrevHeight1 != Height || PrevWidth1 != Width)
                {
                    SetControlsFirst(host, connect, text, Height, Width);
                    PrevWidth1 = Width;
                    PrevHeight1 = Height;
                }
            }
            else
            {
                if (PrevHeight2 != Height || PrevWidth2 != Width)
                {
                    SetControlsSecond(Height, Width);
                    SetConnectList(Height, Width);
                    PrevWidth2 = Width;
                    PrevHeight2 = Height;
                }
            }

        }

        private void SetConnectList(int Height, int Width)
        {
            var invites = LocalConnectionFinder.TryGetInvites();
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
                ip.Text = invites[i].HostName;
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
                    
                };
                invitesButt[i] = ip;
            }
            for (int i = 0; i < invitesButt.Length; i++)
            {
                controls.Add(invitesButt[i]);
            }
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

        private void SetControlsFirst(Button host, Button connect, Label text, int Height, int Width)
        {
            text.Size = new Size(Width / 2, 8 * (Width / 2) / 58);
            host.Size = new Size(Width / 2, 8 * (Width / 2) / 32);
            connect.Size = new Size(Width / 2, 8 * (Width / 2) / 55);
            text.Location = new Point(Width / 4, Height / 8);
            host.Location = new Point(Width / 4, Height / 4);
            connect.Location = new Point(Width / 4, 2 * Height / 4);
            controls.Add(host);
            controls.Add(connect);
            controls.Add(text);
        }

        private void SetControlsSecond(int Height, int Width)
        {
            box.Size = new Size(Width / 2, 8 * (Width / 2) / 58);
            box.Location = new Point(Width / 4, Height / 8);
            controls.Add(back);
            controls.Add(box);
        }
    }


}

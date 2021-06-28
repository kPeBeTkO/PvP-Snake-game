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

namespace SnakeGame
{
    class Menu
    {
        private ControlCollection controls;
        bool secondMenu = false;
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
            };
        }

        public void DrawMenu(Graphics g, Button host, Button connect, Label text, int Height, int Width)
        {
            DrawBack(g, Height, Width);
            if (!secondMenu)
            {
                SetControls(host, connect, text, Height, Width);
                controls.Add(host);
                controls.Add(connect);
                controls.Add(text);
            }
            else
            {
                var box = new TextBox();
                controls.Add(box);
                controls.Clear();
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

        private void SetControls(Button host, Button connect, Label text, int Height, int Width)
        {
            text.Size = new Size(Width - Width / 2, 8 * (Width - Width / 2) / 58);
            host.Size = new Size(Width - Width / 2, 8 * (Width - Width / 2) / 32);
            connect.Size = new Size(Width - Width / 2, 8 * (Width - Width / 2) / 55);
            text.Location = new Point(Width / 4, Height / 8);
            host.Location = new Point(Width / 4, Height / 4);
            connect.Location = new Point(Width / 4, 2 * Height / 4);
        }
    }


}

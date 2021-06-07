using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeGame
{
    public partial class StartForm : Form
    {
        private Button start;
        private Label text;
        public StartForm(int fieldHeight, int fieldWidth)
        {
            Icon = new Icon("Textures\\icon.ico");
            //BackColor = Color.Green;

            start = new Button()
            {
                Text = "PLAY",
                Location = new Point(Width / 2, Height / 2),
                Size = new Size(Width / 4, Height / 6),
                Font = new Font("impact", 50),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(0, 100, 100, 100),
                
            };

            text = new Label()
            {
                //BackColor = Color.Green,
                Location = new Point(Width / 2, Height / 4),
                Text = "PvP Snake",
                Font = new Font("impact", 70),
                Size = new Size(Width / 5, Height / 8),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(0, 0, 0, 0)
        };

            start.FlatAppearance.BorderSize = 0;
            start.Click += (sender, args) =>
            {
                Application.Run(new GameForm(20, 20));
            };
            Controls.Add(start);
            Controls.Add(text);
            Brush green = new SolidBrush(Color.FromArgb(41, 135, 72));
            Brush lime = new SolidBrush(Color.FromArgb(92, 188, 90));
            Paint += (s, a) => 
            {
                var g = a.Graphics;
                var y = 0;
                var color = true;
                var color2 = false;
                while (y < Height)
                {
                    for (int i = 0; i < 12; i++)
                    {
                        if (color)
                            g.FillRectangle(lime, new Rectangle(Width / 12 * i, y, Width / 12, Width / 12));
                        else
                            g.FillRectangle(green, new Rectangle(Width / 12 * i, y, Width / 12, Width / 12));
                        color = !color;
                    }
                    color = color2;
                    color2 = !color;
                    y = y + Width / 12;
                }
                start.Size = new Size(Width / 4 , Height / 6);
                start.Location = new Point(Width / 2 - start.Size.Width/2, Height / 2 - start.Size.Height / 2);
                text.Size = new Size(Width / 2, Height / 6);
                text.Location = new Point(Width / 2 - start.Size.Width / 2, Height / 4 - start.Size.Height / 2);
                Controls.Add(start);
                Controls.Add(text);
            };
            var timer = new Timer();
            timer.Interval = 100;
            timer.Enabled = true;
            timer.Tick += (s, a) => Invalidate();
            timer.Start();
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Icon = new Icon("Textures/icon.ico");
            Text = "PvPSnake";
        }
    }
}

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

namespace SnakeGame
{
    public partial class StartForm : Form
    {
        private GameStateDto state = new GameStateDto();
        private Button start;
        private Label text;
        private Point[,] coordinates;
        private Bitmap field;
        private int rectangleSize;
        private Dictionary<Texture, Bitmap> textures = new Dictionary<Texture, Bitmap>();

        public void DrawMenu(Graphics g, Brush brush1, Brush brush2)
        {
            var y = 0;
            var color = true;
            var color2 = false;
            while (y < Height)
            {
                for (int i = 0; i < 12; i++)
                {
                    if (color)
                        g.FillRectangle(brush1, new Rectangle(Width / 12 * i, y, Width / 12, Width / 12));
                    else
                        g.FillRectangle(brush2, new Rectangle(Width / 12 * i, y, Width / 12, Width / 12));
                    color = !color;
                }
                color = color2;
                color2 = !color;
                y = y + Width / 12;
            }
        }

        private void CreateAllTextures()
        {
            var tailUp = new Bitmap("Textures\\tail.png");
            var tailDown = new Bitmap("Textures\\tail.png");
            tailDown.RotateFlip(RotateFlipType.Rotate180FlipNone);
            var tailRight = new Bitmap("Textures\\tail.png");
            tailRight.RotateFlip(RotateFlipType.Rotate90FlipNone);
            var tailLeft = new Bitmap("Textures\\tail.png");
            tailLeft.RotateFlip(RotateFlipType.Rotate270FlipNone);
            var headUp = new Bitmap("Textures\\head.png");
            var headDown = new Bitmap("Textures\\head.png");
            headDown.RotateFlip(RotateFlipType.Rotate180FlipNone);
            var headRight = new Bitmap("Textures\\head.png");
            headRight.RotateFlip(RotateFlipType.Rotate90FlipNone);
            var headLeft = new Bitmap("Textures\\head.png");
            headLeft.RotateFlip(RotateFlipType.Rotate270FlipNone);
            var bodyVertical = new Bitmap("Textures\\body.png");
            var bodyHorizintal = new Bitmap("Textures\\body.png");
            bodyHorizintal.RotateFlip(RotateFlipType.Rotate90FlipNone);
            var angelDownRight = new Bitmap("Textures\\right.png");
            var angelRightUp = new Bitmap("Textures\\right.png");
            angelRightUp.RotateFlip(RotateFlipType.Rotate270FlipNone);
            var angelUpLeft = new Bitmap("Textures\\right.png");
            angelUpLeft.RotateFlip(RotateFlipType.Rotate180FlipNone);
            var angelLeftDown = new Bitmap("Textures\\right.png");
            angelLeftDown.RotateFlip(RotateFlipType.Rotate90FlipNone);
            var apple = new Bitmap("Textures\\apple.png");
            var boot = new Bitmap("Textures\\boot.png");
            var poison = new Bitmap("Textures\\poison.png");
            textures.Add(Texture.TailUp, tailUp);
            textures.Add(Texture.TailDown, tailDown);
            textures.Add(Texture.TailRight, tailRight);
            textures.Add(Texture.TailLeft, tailLeft);
            textures.Add(Texture.HeadUp, headUp);
            textures.Add(Texture.HeadDown, headDown);
            textures.Add(Texture.HeadRight, headRight);
            textures.Add(Texture.HeadLeft, headLeft);
            textures.Add(Texture.BodyHorizintal, bodyHorizintal);
            textures.Add(Texture.BodyVertical, bodyVertical);
            textures.Add(Texture.AngelDownRight, angelDownRight);
            textures.Add(Texture.AngelRightUp, angelRightUp);
            textures.Add(Texture.AngelUpLeft, angelUpLeft);
            textures.Add(Texture.AngelLeftDown, angelLeftDown);
            textures.Add(Texture.Apple, apple);
            textures.Add(Texture.Boot, boot);
            textures.Add(Texture.Poison, poison);
        }

        private void StartGame(int fieldHeight, int fieldWidth)
        {
            CreateAllTextures();
            CreateField(fieldHeight, fieldWidth);
        }

        private void CreateField(int fieldHeight, int fieldWidth)
        {
            coordinates = new Point[fieldHeight,fieldWidth];
            var field = new Bitmap(Width, Height);
            var g = Graphics.FromImage(field);
            Brush brush1 = new SolidBrush(Color.FromArgb(41, 135, 72));
            Brush brush2 = new SolidBrush(Color.FromArgb(48, 155, 82));
            Brush brush3 = new SolidBrush(Color.FromArgb(92, 188, 90));
            g.FillRectangle(brush1, 0, 0, Width, Height);
            rectangleSize = Math.Min((Height - Height / 10 - Height / 10) / fieldHeight, Width / fieldWidth);
            var x = (Width - rectangleSize * fieldWidth) / 2;
            var y = Height/10;
            var color = true;
            var color2 = false;
            for (int j = 0; j < fieldHeight; j++)
            {
                for (int i = 0; i < fieldWidth; i++)
                {
                    coordinates[j, i] = new Point(y + rectangleSize * j, x + rectangleSize * i);
                    if (color)
                        g.FillRectangle(brush2, new Rectangle(x + rectangleSize * i, y + rectangleSize * j, rectangleSize, rectangleSize));
                    else
                        g.FillRectangle(brush3, new Rectangle(x + rectangleSize * i, y + rectangleSize * j, rectangleSize, rectangleSize));
                    color = !color;
                }
                color = color2;
                color2 = !color;
            }
            this.field = field;
        }

        private Bitmap DrawFrame(Dictionary<Texture, Bitmap> textures, Bitmap field, 
            GameStateDto state, int fieldHeight, int fieldWidth)
        {
            var point = new Point();
            var frame = new Bitmap(field);
            var g = Graphics.FromImage(field);
            point = coordinates[state.Player[0].Y, state.Player[0].X];
            g.DrawImage(textures[Texture.HeadDown], new Rectangle(point.Y, point.X, rectangleSize, rectangleSize));
            for (int i = 1; i < state.Player.Length - 1; i++)
            {
                point = coordinates[state.Player[i].Y, state.Player[i].X];
                g.DrawImage(textures[Texture.BodyHorizintal], new Rectangle(point.Y, point.X, rectangleSize, rectangleSize));
            }
            point = coordinates[state.Player[state.Player.Length - 1].Y, state.Player[state.Player.Length - 1].X];
            g.DrawImage(textures[Texture.TailUp] , new Rectangle(point.Y, point.X, rectangleSize, rectangleSize));
            return frame;
        }

        public StartForm(int fieldHeight, int fieldWidth)
        {
            DoubleBuffered = true;
            Icon = new Icon("Textures\\icon.ico");

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

            start.FlatAppearance.BorderSize = 0;
            state.Player = new Vector[] { new Vector(0,0), new Vector(1, 0), new Vector(2, 0), new Vector(2, 1), new Vector(2, 2) };
            start.Click += (sender, args) =>
            {
                StartGame(fieldHeight, fieldWidth);
            };

            text = new Label()
            {
                Location = new Point(Width / 2, Height / 4),
                Text = "PvP Snake",
                Font = new Font("impact", 70),
                Size = new Size(Width / 5, Height / 8),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(0, 0, 0, 0)
            };

            Brush green = new SolidBrush(Color.FromArgb(48, 155, 82));
            Brush lime = new SolidBrush(Color.FromArgb(92, 188, 90));
            Bitmap frame;
            Paint += (s, a) =>
            {
                if (field != null)
                {
                    frame = DrawFrame(textures, field, state, fieldHeight, fieldWidth);
                    a.Graphics.DrawImage(frame, new PointF(0,0));
                    Controls.Clear();
                }
                else
                {
                    DrawMenu(a.Graphics, green, lime);
                    start.Size = new Size(Width / 4, Height / 6);
                    start.Location = new Point(Width / 2 - start.Size.Width / 2, Height / 2 - start.Size.Height / 2);
                    text.Size = new Size(Width / 2, Height / 6);
                    text.Location = new Point(Width / 2 - start.Size.Width / 2, Height / 4 - start.Size.Height / 2);
                    Controls.Add(start);
                    Controls.Add(text);
                }
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

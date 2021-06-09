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
        protected GameStateDto state = new GameStateDto();
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
            textures.Add(Texture.BodyHorizontal, bodyHorizintal);
            textures.Add(Texture.BodyVertical, bodyVertical);
            textures.Add(Texture.AngelDownRight, angelDownRight);
            textures.Add(Texture.AngelUpRight, angelRightUp);
            textures.Add(Texture.AngelUpLeft, angelUpLeft);
            textures.Add(Texture.AngelDownLeft, angelLeftDown);
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

        private Bitmap DrawFrame(Dictionary<Texture, Bitmap> textures, int fieldHeight, int fieldWidth)
        {
            var point = new Point();
            var prev = new Point();
            var next = new Point();
            var frame = new Bitmap(field);
            var g = Graphics.FromImage(frame);


            point = new Point(state.Player[0].X, state.Player[0].Y);
            prev = new Point (state.Player[0].X, state.Player[0].Y);
            next = new Point(state.Player[1].X, state.Player[1].Y);
            var texture = Texture.Apple;
            if (prev.Y == next.Y && prev.X - 1 == next.X)
                texture = Texture.HeadRight;
            else if (prev.Y == next.Y && prev.X + 1 == next.X)
                texture = Texture.HeadLeft;
            else if (prev.Y - 1 == next.Y && prev.X == next.X)
                texture = Texture.HeadDown;
            else if (prev.Y + 1 == next.Y && prev.X == next.X)
                texture = Texture.HeadUp;
            g.DrawImage(textures[texture], new Rectangle(coordinates[point.Y, point.X].Y, coordinates[point.Y, point.X].X, rectangleSize, rectangleSize));
            for (int i = 1; i < state.Player.Length - 1; i++)
            {
                point = next;
                next = new Point(state.Player[i + 1].X, state.Player[i + 1].Y);
                if (prev.Y == next.Y)
                    texture = Texture.BodyHorizontal;
                else if (prev.X == next.X)
                    texture = Texture.BodyVertical;
                else if ((prev.X == next.X + 1 && prev.Y == next.Y - 1 && point.X == prev.X) ||
                    (prev.X == next.X - 1 && prev.Y == next.Y + 1 && point.Y == prev.Y))
                    texture = Texture.AngelUpLeft;
                else if ((prev.X == next.X + 1 && prev.Y == next.Y + 1 && point.Y == prev.Y) ||
                        (prev.X == next.X - 1 && prev.Y == next.Y - 1 && point.X == prev.X))
                    texture = Texture.AngelUpRight;
                else if ((prev.X == next.X - 1 && prev.Y == next.Y - 1 && point.Y == prev.Y) ||
                    (prev.X == next.X + 1 && prev.Y == next.Y + 1 && point.X == prev.X))
                    texture = Texture.AngelDownLeft;
                else
                    texture = Texture.AngelDownRight;
                g.DrawImage(textures[texture], new Rectangle(coordinates[point.Y, point.X].Y, coordinates[point.Y, point.X].X, rectangleSize, rectangleSize));
                prev = point;
            }
            if (prev.Y == next.Y && prev.X - 1 == next.X)
                texture = Texture.TailRight;
            else if (prev.Y == next.Y && prev.X + 1 == next.X)
                texture = Texture.TailLeft;
            else if (prev.Y - 1 == next.Y && prev.X == next.X)
                texture = Texture.TailDown;
            else if (prev.Y + 1 == next.Y && prev.X == next.X)
                texture = Texture.TailUp;

            g.DrawImage(textures[texture] , new Rectangle(coordinates[next.Y, next.X].Y, coordinates[next.Y, next.X].X, rectangleSize, rectangleSize));



            point = new Point(state.Enemy[0].X, state.Enemy[0].Y);
            prev = new Point (state.Enemy[0].X, state.Enemy[0].Y);
            next = new Point(state.Enemy[1].X, state.Enemy[1].Y);
            texture = Texture.Apple;
            if (prev.Y == next.Y && prev.X - 1 == next.X)
                texture = Texture.HeadRight;
            else if (prev.Y == next.Y && prev.X + 1 == next.X)
                texture = Texture.HeadLeft;
            else if (prev.Y - 1 == next.Y && prev.X == next.X)
                texture = Texture.HeadDown;
            else if (prev.Y + 1 == next.Y && prev.X == next.X)
                texture = Texture.HeadUp;
            g.DrawImage(textures[texture], new Rectangle(coordinates[point.Y, point.X].Y, coordinates[point.Y, point.X].X, rectangleSize, rectangleSize));
            for (int i = 1; i < state.Enemy.Length - 1; i++)
            {
                point = next;
                next = new Point(state.Enemy[i + 1].X, state.Enemy[i + 1].Y);
                if (prev.Y == next.Y)
                    texture = Texture.BodyHorizontal;
                else if (prev.X == next.X)
                    texture = Texture.BodyVertical;
                else if ((prev.X == next.X + 1 && prev.Y == next.Y - 1 && point.X == prev.X) ||
                    (prev.X == next.X - 1 && prev.Y == next.Y + 1 && point.Y == prev.Y))
                    texture = Texture.AngelUpLeft;
                else if ((prev.X == next.X + 1 && prev.Y == next.Y + 1 && point.Y == prev.Y) ||
                        (prev.X == next.X - 1 && prev.Y == next.Y - 1 && point.X == prev.X))
                    texture = Texture.AngelUpRight;
                else if ((prev.X == next.X - 1 && prev.Y == next.Y - 1 && point.Y == prev.Y) ||
                    (prev.X == next.X + 1 && prev.Y == next.Y + 1 && point.X == prev.X))
                    texture = Texture.AngelDownLeft;
                else
                    texture = Texture.AngelDownRight;
                g.DrawImage(textures[texture], new Rectangle(coordinates[point.Y, point.X].Y, coordinates[point.Y, point.X].X, rectangleSize, rectangleSize));
                prev = point;
            }
            if (prev.Y == next.Y && prev.X - 1 == next.X)
                texture = Texture.TailRight;
            else if (prev.Y == next.Y && prev.X + 1 == next.X)
                texture = Texture.TailLeft;
            else if (prev.Y - 1 == next.Y && prev.X == next.X)
                texture = Texture.TailDown;
            else if (prev.Y + 1 == next.Y && prev.X == next.X)
                texture = Texture.TailUp;

            g.DrawImage(textures[texture] , new Rectangle(coordinates[next.Y, next.X].Y, coordinates[next.Y, next.X].X, rectangleSize, rectangleSize));


            foreach (var item in state.Items)
            {
                if (item.Type == "Apple")
                    texture = Texture.Apple;
                else if (item.Type == "Boots")
                    texture = Texture.Boot;
                else if (item.Type == "Poison")
                    texture = Texture.Poison;
                g.DrawImage(textures[texture], new Rectangle(coordinates[item.Location.Y, item.Location.X].Y,
                        coordinates[item.Location.Y, item.Location.X].X, rectangleSize, rectangleSize));
            }
            return frame;
        }

        private GameStateDto GetSnake()
        {
            var state = new GameStateDto();
            var snake = new List<Vector>();
            var r = new Random();
            var v = r.Next(3, 10);
            snake.Add(new Vector(r.Next(20), r.Next(15)));
            for (int i = 0; i < v; i++)
            {
                var dir = (Direction)r.Next(4); 
                var newP = snake.Last().AddOnRing(Vector.GetVector(dir), new Vector(20, 15));
                var j = 0;
                for(j = 0; snake.Contains(newP) && j < 3; j++)
                {
                    dir = (Direction)(((int)dir + 1) % 4);
                    newP = snake.Last().AddOnRing(Vector.GetVector(dir), new Vector(20, 15));
                }
                if (j == 3) break;
                snake.Add(newP);
            }

            state.Items = new ItemDto[2];
            for (var i = 0; i < 2; i ++)
            {
                var type = "";
                var n = r.Next(3);
                switch(n)
                {
                    case 1:
                        type = "Apple";
                        break;
                    case 2:
                        type = "Boots";
                        break;
                    case 0:
                        type = "Poison";
                        break;
                }
                var newP = new Vector(r.Next(20), r.Next(15));
                while (snake.Contains(newP))
                    newP = new Vector(r.Next(20), r.Next(15));
                state.Items[i] = new ItemDto() { Location = newP, Type = type };
            }
            state.Player = snake.ToArray();
            return state;
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
                BackColor = Color.FromArgb(0, 100, 100, 100)
            };

            start.FlatAppearance.BorderSize = 0;
            start.Click += (s, a) =>
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
            Paint += (s, a) =>
            {
                if (field != null)
                {

                    //state = GetSnake();

                    var frame = DrawFrame(textures, fieldHeight, fieldWidth);
                    a.Graphics.DrawImage(frame, new PointF(0,0));
                    Controls.Clear();
                }
                else
                {
                    DrawMenu(a.Graphics, green, lime);
                    start.Size = new Size(Width / 4, 3 * Height / 14);
                    start.Font = new Font("impact", Math.Min(Height / 10, Width / 12));
                    start.Location = new Point(Width / 2 - start.Size.Width / 2, Height / 2 - start.Size.Height / 2);
                    text.Size = new Size(Width / 2, Height / 4);
                    text.Font = new Font("impact", Math.Min(Height / 10, Width / 15));
                    text.Location = new Point(Width / 2 - start.Size.Width / 2, Height / 4 - start.Size.Height / 2);
                    Controls.Add(start);
                    Controls.Add(text);
                }
            };
            var timer = new Timer();
            timer.Interval = 50;
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

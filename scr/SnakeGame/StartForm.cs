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
using SnakeCore.Network;

namespace SnakeGame
{
    public partial class StartForm : Form
    {
        public static GameClient client;
        public WMPLib.WindowsMediaPlayer WMP = new WMPLib.WindowsMediaPlayer();
        private GameDto state;
        private Button host = new Button();
        private Button connect = new Button();
        private Label text = new Label();
        static public Brush brush1 = new SolidBrush(Color.FromArgb(48, 165, 82));
        static public Brush brush2 = new SolidBrush(Color.FromArgb(92, 188, 90));
        static public GameDraw game = new GameDraw();

        private GameDto GetSnake()
        {
            var state = new GameDto();
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
            state.Snakes = new SnakeDto[1];
            state.Snakes[0] = new SnakeDto();
            state.Snakes[0].Body = snake.ToArray();
            return state;
        }

        public StartForm()
        {
            DoubleBuffered = true;
            GameClient.Updated += () => Invalidate();
            var menu = new Menu(host, connect, text, Controls);
            KeyDown += ChangeDirection;
            var timer = new Timer();
            /*timer.Interval = 50;
            timer.Enabled = true;
            timer.Tick += (send, args) => Invalidate();
            timer.Start();*/
            host.Click += (s, a) =>
            {
                /*WMP.URL = "Sounds\\AlIkAbIr_-_Square.wav";
                //WMP.settings.volume = 100;
                WMP.controls.play();
                var timer = new Timer();/*
                timer.Interval = 50;
                timer.Enabled = true;
                timer.Tick += (send, args) => Invalidate();
                timer.Start();
                client = GameClient.Host("Server", new Vector(20, 15), 2);
                StartForm.game.isStart = true;*/
            };

            /*connect.Click += (s, a) =>
            {
                var timer = new Timer();
                timer.Interval = 50;
                timer.Enabled = true;
                timer.Tick += (send, args) => Invalidate();
                timer.Start();
            };*/
            Paint += (s, a) =>
            {
                if (game.isStart)
                {
                    Controls.Clear();
                    if (client != null)
                        state = client.GameState;
                    var frame = game.GetFrame(state, Height, Width);
                    a.Graphics.DrawImage(frame, new PointF(0,0));
                }
                else
                {
                    menu.DrawMenu(a.Graphics, host, connect, text, Height, Width);
                }
            };
            SizeChanged += (s, a) => Invalidate();
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Icon = new Icon("Textures/icon.ico");
            Text = "PvPSnake";
        }

        void ChangeDirection(object sender, KeyEventArgs args)
        {
            if (client == null)
                return;
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
        }
    }
}

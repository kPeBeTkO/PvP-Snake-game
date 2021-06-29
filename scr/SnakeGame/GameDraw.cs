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
    public class GameDraw
    {
        private Bitmap field;
        private Dictionary<Texture, Bitmap>[] snakeTextures;
        private Dictionary<Texture, Bitmap> itemsTexture = new Dictionary<Texture, Bitmap>();
        private Bitmap[] scoreTexture;
        private Color[] snakeColors;
        private int fieldHeight;
        private int fieldWidth;
        public bool isStart = false;

        public void PrepareGame(int snakes, int height, int width)
        {
            this.fieldHeight = height;
            this.fieldWidth = width;
            snakeTextures = new Dictionary<Texture, Bitmap>[snakes];
            scoreTexture = new Bitmap[snakes];
            snakeColors = new Color[3];
            snakeColors[0] = Color.FromArgb(255, 167, 127);
            snakeColors[1] = Color.FromArgb(255, 0, 220);
            snakeColors[2] = Color.FromArgb(89, 94, 237);
            var textureFab = new Resources();
            for (int i = 0; i < snakes; i++)
            {
                scoreTexture[i] = textureFab.CreateScoreTexture(snakeColors[i]);
                snakeTextures[i] = textureFab.CreateSnakeTextures(snakeColors[i]);
            }
            itemsTexture = textureFab.CreateItemsTextures();
            field = GetField();
        }

        private Bitmap DrawSnake(Bitmap frame, SnakeDto snake, Dictionary<Texture, Bitmap> textures)
        {
            var g = Graphics.FromImage(frame);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            var point = new Point(snake.Body[0].X, snake.Body[0].Y);
            var prev = new Point(snake.Body[0].X, snake.Body[0].Y);
            var next = new Point(snake.Body[1].X, snake.Body[1].Y);
            var texture = Texture.Apple;
            if (prev.Y == next.Y && (prev.X - 1 + fieldWidth) % fieldWidth == next.X)
                texture = Texture.HeadRight;
            else if (prev.Y == next.Y && (prev.X + 1) %fieldWidth == next.X)
                texture = Texture.HeadLeft;
            else if ((prev.Y - 1 + fieldHeight) % fieldHeight == next.Y && prev.X == next.X)
                texture = Texture.HeadDown;
            else if ((prev.Y + 1) % fieldHeight == next.Y && prev.X == next.X)
                texture = Texture.HeadUp;
            g.DrawImage(textures[texture], new Rectangle(point.X * 16, point.Y * 16, 16, 16));
            for (int i = 1; i < snake.Body.Length - 1; i++)
            {
                point = next;
                next = new Point(snake.Body[i + 1].X, snake.Body[i + 1].Y);
                if (prev.Y == next.Y)
                    texture = Texture.BodyHorizontal;
                else if (prev.X == next.X)
                    texture = Texture.BodyVertical;
                else if ((prev.X == (next.X + 1) % fieldWidth && prev.Y == (next.Y - 1 + fieldHeight) % fieldHeight && point.X == prev.X) ||
                    (prev.X == (next.X - 1 + fieldWidth) % fieldWidth && prev.Y == (next.Y + 1) % fieldHeight && point.Y == prev.Y))
                    texture = Texture.AngelUpLeft;
                else if ((prev.X == (next.X + 1) % fieldWidth && prev.Y == (next.Y + 1) % fieldHeight && point.Y == prev.Y) ||
                        (prev.X == (next.X - 1 + fieldWidth) % fieldWidth && prev.Y == (next.Y - 1 + fieldHeight) % fieldHeight && point.X == prev.X))
                    texture = Texture.AngelUpRight;
                else if ((prev.X == (next.X - 1 + fieldWidth) % fieldWidth && prev.Y == (next.Y - 1 + fieldHeight) % fieldHeight && point.Y == prev.Y) ||
                    (prev.X == (next.X + 1) % fieldWidth && prev.Y == (next.Y + 1) % fieldHeight && point.X == prev.X))
                    texture = Texture.AngelDownLeft;
                else
                    texture = Texture.AngelDownRight;
                g.DrawImage(textures[texture], new Rectangle(point.X * 16, point.Y * 16, 16, 16));
                prev = point;
            }
            if (prev.Y == next.Y && (prev.X - 1 + fieldWidth) % fieldWidth == next.X)
                texture = Texture.TailRight;
            else if (prev.Y == next.Y && (prev.X + 1) % fieldWidth == next.X)
                texture = Texture.TailLeft;
            else if ((prev.Y - 1 + fieldHeight) % fieldHeight == next.Y && prev.X == next.X)
                texture = Texture.TailDown;
            else if ((prev.Y + 1) % fieldHeight == next.Y && prev.X == next.X)
                texture = Texture.TailUp;
            g.DrawImage(textures[texture], new Rectangle(next.X * 16, next.Y * 16, 16, 16));
            return frame;
        }

        private Bitmap DrawItems(Bitmap frame, ItemDto[] items, Dictionary<Texture, Bitmap> textures)
        {
            var g = Graphics.FromImage(frame);
            var texture = Texture.Apple;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            foreach (var item in items)
            {
                if (item.Type == "Apple")
                    texture = Texture.Apple;
                else if (item.Type == "Boots")
                    texture = Texture.Boot;
                else if (item.Type == "Poison")
                    texture = Texture.Poison;
                g.DrawImage(textures[texture], new Rectangle(item.Location.X * 16, item.Location.Y * 16, 16, 16));
            }
            return frame;
        }

        public Bitmap GetFrame(GameDto state, int height, int width)
        {
            var frame = new Bitmap(field);
            if (state != null)
            {
                if (state.Snakes != null)
                    for(var i = 0; i < state.Snakes.Length; i++)
                        frame = DrawSnake(frame, state.Snakes[i], snakeTextures[i]);
                frame = DrawItems(frame, state.Items, itemsTexture);
            }
            return GetBorders(frame, height, width, state);
        }

        private Bitmap GetField()
        {
            var field = new Bitmap(fieldWidth * 16, fieldHeight * 16);
            var g = Graphics.FromImage(field);
            var color = true;
            var color2 = false;
            for (int j = 0; j < fieldHeight; j++)
            {
                for (int i = 0; i < fieldWidth; i++)
                {
                    if (color)
                        g.FillRectangle(StartForm.brush1, new Rectangle(16 * i, 16 * j, 16, 16));
                    else
                        g.FillRectangle(StartForm.brush2, new Rectangle(16 * i, 16 * j, 16, 16));
                    color = !color;
                }
                color = color2;
                color2 = !color;
            }
            return field;
        }

        private Bitmap GetBorders(Bitmap pic, int Height, int Width, GameDto state)
        {
            var frame = new Bitmap(Width, Height);
            var g = Graphics.FromImage(frame);
            Brush brush1 = new SolidBrush(Color.FromArgb(41, 135, 72));
            g.FillRectangle(brush1, 0, 0, Width, Height);
            var rectangleSize = Math.Min((Height - Height / 9 - Height / 9) / fieldHeight, Width / fieldWidth);
            var x = (Width - rectangleSize * fieldWidth) / 2;
            var y = Height / 9;
            if (state == null)
            {
                g.DrawImage(scoreTexture[0], new Point(x, 10));
                g.DrawString("0", new Font("impact", 40), new SolidBrush(snakeColors[0]), x + scoreTexture[0].Width, 5);
            }
            else
            {
                for (int i = 0; i < state.Snakes.Length; i++)
                { 
                    g.DrawImage(scoreTexture[i], new Point(x +  i * Width / state.Snakes.Length, 10));
                    g.DrawString((state.Snakes[i].Body.Length - 3).ToString(), new Font("impact", 40), new SolidBrush(snakeColors[i]),
                        x + scoreTexture[i].Width + i * Width / state.Snakes.Length, 5);
                }
            }
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            g.DrawImage(pic, new Rectangle(x, y, rectangleSize * fieldWidth, rectangleSize * fieldHeight));
            return frame;
        }

    }
}

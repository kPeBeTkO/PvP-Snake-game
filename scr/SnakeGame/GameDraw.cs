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
        private Dictionary<Texture, Bitmap>[] snakeTextures = new Dictionary<Texture, Bitmap>[2];
        private Dictionary<Texture, Bitmap> itemsTexture = new Dictionary<Texture, Bitmap>();
        public bool isStart = false;

        public GameDraw()
        {
            var textureFab = new Resources();
            Color color1 = Color.FromArgb(255, 167, 127);
            Color color2 = Color.FromArgb(255, 0, 220);
            snakeTextures[0] = textureFab.CreateSnakeTextures(color1);
            snakeTextures[1] = textureFab.CreateSnakeTextures(color2);
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
            if (prev.Y == next.Y && (prev.X - 1 + Program.fieldWidth) % Program.fieldWidth == next.X)
                texture = Texture.HeadRight;
            else if (prev.Y == next.Y && (prev.X + 1) % Program.fieldWidth == next.X)
                texture = Texture.HeadLeft;
            else if ((prev.Y - 1 + Program.fieldHeight) % Program.fieldHeight == next.Y && prev.X == next.X)
                texture = Texture.HeadDown;
            else if ((prev.Y + 1) % Program.fieldHeight == next.Y && prev.X == next.X)
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
                else if ((prev.X == (next.X + 1) % Program.fieldWidth && 
                    prev.Y == (next.Y - 1 + Program.fieldHeight) % Program.fieldHeight && point.X == prev.X) ||
                    (prev.X == (next.X - 1 + Program.fieldWidth) % Program.fieldWidth && 
                    prev.Y == (next.Y + 1) % Program.fieldHeight && point.Y == prev.Y))
                    texture = Texture.AngelUpLeft;
                else if ((prev.X == (next.X + 1) % Program.fieldWidth && 
                    prev.Y == (next.Y + 1) % Program.fieldHeight && point.Y == prev.Y) ||
                        (prev.X == (next.X - 1 + Program.fieldWidth) % Program.fieldWidth && 
                        prev.Y == (next.Y - 1 + Program.fieldHeight) % Program.fieldHeight && point.X == prev.X))
                    texture = Texture.AngelUpRight;
                else if ((prev.X == (next.X - 1 + Program.fieldWidth) % Program.fieldWidth && 
                    prev.Y == (next.Y - 1 + Program.fieldHeight) % Program.fieldHeight && point.Y == prev.Y) ||
                    (prev.X == (next.X + 1) % Program.fieldWidth && 
                    prev.Y == (next.Y + 1) % Program.fieldHeight && point.X == prev.X))
                    texture = Texture.AngelDownLeft;
                else
                    texture = Texture.AngelDownRight;
                g.DrawImage(textures[texture], new Rectangle(point.X * 16, point.Y * 16, 16, 16));
                prev = point;
            }
            if (prev.Y == next.Y && (prev.X - 1 + Program.fieldWidth) % Program.fieldWidth == next.X)
                texture = Texture.TailRight;
            else if (prev.Y == next.Y && (prev.X + 1) % Program.fieldWidth == next.X)
                texture = Texture.TailLeft;
            else if ((prev.Y - 1 + Program.fieldHeight) % Program.fieldHeight == next.Y && prev.X == next.X)
                texture = Texture.TailDown;
            else if ((prev.Y + 1) % Program.fieldHeight == next.Y && prev.X == next.X)
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
                    foreach(var snake in state.Snakes)
                        frame = DrawSnake(frame, snake, snakeTextures[0]);
                frame = DrawItems(frame, state.Items, itemsTexture);
            }
            return GetBorders(frame, height, width);
        }

        private Bitmap GetField()
        {
            var field = new Bitmap(Program.fieldWidth * 16, Program.fieldHeight * 16);
            var g = Graphics.FromImage(field);
            var color = true;
            var color2 = false;
            for (int j = 0; j < Program.fieldHeight; j++)
            {
                for (int i = 0; i < Program.fieldWidth; i++)
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

        private Bitmap GetBorders(Bitmap pic, int Height, int Width)
        {
            var frame = new Bitmap(Width, Height);
            var g = Graphics.FromImage(frame);
            Brush brush1 = new SolidBrush(Color.FromArgb(41, 135, 72));
            g.FillRectangle(brush1, 0, 0, Width, Height);
            var rectangleSize = Math.Min((Height - Height / 10 - Height / 10) / Program.fieldHeight, Width / Program.fieldWidth);
            var x = (Width - rectangleSize * Program.fieldWidth) / 2;
            var y = Height / 10;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            g.DrawImage(pic, new Rectangle(x, y, rectangleSize * Program.fieldWidth, rectangleSize * Program.fieldHeight));
            return frame;
        }

    }
}

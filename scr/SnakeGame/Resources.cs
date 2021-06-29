using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    class Resources
    {
        private Bitmap Paint(Bitmap pic, Color color)
        {
            for (int y = 0; y < pic.Height; y++)
            {
                for (int x = 0; x < pic.Width; x++)
                {
                    if (pic.GetPixel(x, y) == Color.FromArgb(151, 128, 188))
                        pic.SetPixel(x, y, color);
                    if (pic.GetPixel(x, y) == Color.FromArgb(177, 151, 219))
                        pic.SetPixel(x, y, Color.FromArgb(Math.Min(color.R + 15, 255), Math.Min(color.G + 15, 255), Math.Min(color.B + 15, 255)));
                    if (pic.GetPixel(x, y) == Color.FromArgb(134, 114, 165))
                        pic.SetPixel(x, y, Color.FromArgb(Math.Max(color.R - 15, 0), Math.Max(color.G - 15, 0), Math.Max(color.B - 15, 0)));
                }
            }
            return pic;
        }
        public Dictionary<Texture, Bitmap> CreateSnakeTextures(Color color)
        {
            var apple = new Bitmap("Textures\\apple.png");
            var tail = Paint(new Bitmap("Textures\\tail.png"), color);
            var head = Paint(new Bitmap("Textures\\head.png"), color);
            var angle = Paint(new Bitmap("Textures\\right.png"), color);
            var body = Paint(new Bitmap("Textures\\body.png"), color);
            var tailUp = new Bitmap(tail);
            var tailDown = new Bitmap(tail);
            tailDown.RotateFlip(RotateFlipType.Rotate180FlipNone);
            var tailRight = new Bitmap(tail);
            tailRight.RotateFlip(RotateFlipType.Rotate90FlipNone);
            var tailLeft = new Bitmap(tail);
            tailLeft.RotateFlip(RotateFlipType.Rotate270FlipNone);
            var headUp = new Bitmap(head);
            var headDown = new Bitmap(head);
            headDown.RotateFlip(RotateFlipType.Rotate180FlipNone);
            var headRight = new Bitmap(head);
            headRight.RotateFlip(RotateFlipType.Rotate90FlipNone);
            var headLeft = new Bitmap(head);
            headLeft.RotateFlip(RotateFlipType.Rotate270FlipNone);
            var bodyVertical = new Bitmap(body);
            var bodyHorizintal = new Bitmap(body);
            bodyHorizintal.RotateFlip(RotateFlipType.Rotate90FlipNone);
            var angelDownRight = new Bitmap(angle);
            var angelRightUp = new Bitmap(angle);
            angelRightUp.RotateFlip(RotateFlipType.Rotate270FlipNone);
            var angelUpLeft = new Bitmap(angle);
            angelUpLeft.RotateFlip(RotateFlipType.Rotate180FlipNone);
            var angelLeftDown = new Bitmap(angle);
            angelLeftDown.RotateFlip(RotateFlipType.Rotate90FlipNone);
            var textures = new Dictionary<Texture, Bitmap>();
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
            return textures;
        }

        public Bitmap CreateScoreTexture(Color color, int multiply)
        {
            var score = Paint(new Bitmap("Textures\\score.png"), color);
            var bigScore = new Bitmap(score.Width * multiply, score.Height * multiply);
            var g = Graphics.FromImage(bigScore);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            g.DrawImage(score, new Rectangle(0, 0, score.Width * multiply, score.Height * multiply));
            return bigScore;
        }

        public Dictionary<Texture, Bitmap> CreateItemsTextures()
        {
            var apple = new Bitmap("Textures\\apple.png");
            var boot = new Bitmap("Textures\\boot.png");
            var poison = new Bitmap("Textures\\poison.png");
            var textures = new Dictionary<Texture, Bitmap>();
            textures.Add(Texture.Apple, apple);
            textures.Add(Texture.Boot, boot);
            textures.Add(Texture.Poison, poison);
            return textures;
        }
    }
}

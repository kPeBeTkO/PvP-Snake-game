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
        public void CreateAllTextures(Dictionary<Texture, Bitmap> textures)
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
    }
}

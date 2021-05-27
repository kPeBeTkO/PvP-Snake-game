using System;
using System.Collections.Generic;
using System.Text;

namespace SnakeCore
{
    public struct Vector
    {
        public readonly int X;
        public readonly int Y;
        public double Length { get { return Math.Sqrt(X * X + Y * Y); } }
        public Vector(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static Vector operator +(Vector a, Vector b)
            => new Vector(a.X + b.X, a.Y + b.Y);
    }
}

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

        public static Vector GetVector(Direction direction)
        {
            switch(direction)
            {
                case Direction.Up:
                    return new Vector(0, 1);
                case Direction.Right:
                    return new Vector(1, 0);
                case Direction.Down:
                    return new Vector(0, -1);
                case Direction.Left:
                    return new Vector(-1, 0);
            }
            return new Vector(0, 0);
        }

        public Vector AddOnRing(Vector vector, Vector ringSize)
            => new Vector((X + vector.X) % ringSize.X, (Y + vector.Y) % ringSize.Y);

        public static Vector AddOnRing(Vector a, Vector b, Vector ringSize)
            => new Vector((a.X + b.X) % ringSize.X, (a.Y + b.Y) % ringSize.Y);

        public static Vector operator +(Vector a, Vector b)
            => new Vector(a.X + b.X, a.Y + b.Y);

        public static bool operator ==(Vector a, Vector b)
            => a.X == b.X && a.Y == b.Y;

        public static bool operator !=(Vector a, Vector b)
            => !(a==b);

        public static Vector operator *(Vector a, int b)
            => new Vector(a.X * b, a.Y * b);

        public static Vector operator %(Vector vector, Vector ringSize) 
            => new Vector(vector.X % ringSize.X, vector.Y % ringSize.Y);

        public override bool Equals(object obj)
        {
            if (!(obj is Vector)) return false;
            var v = (Vector)obj;
            return X == v.X && Y == v.Y;
        }

        public override int GetHashCode()
        {
            return X * 100000 + Y;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;

namespace SnakeCore
{
    public class Snake
    {
        public bool Alive {get; private set;}
        public Point Head {get; private set;}
        public Point Direction {get; private set;}
        public LinkedList<Point> Body {get; private set;}
        public int Speed { get; private set; }
        public int Points 
        {
            get{ return Body.Count + 1; }
        }

        public Snake(Point head, Point tailDirection, int length)
        {
            Head = head;
            Body = new LinkedList<Point>();
            Body.AddLast(head);
            for (var i = 0;i < length; i++)
                var newPoint = Point.
        }

        public void Move()
        {
            Body.AddFirst(Head);
            Body.RemoveLast();
            Head.Offset(Direction);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SnakeCore.Logic;

namespace SnakeCore.Network.Dto
{
    public class SnakeDto
    {
        public Vector[] Body;
        public double Speed;
        public Direction Direction;
        public bool Alive;
        public static SnakeDto Convert(Snake snake)
        {
            return new SnakeDto()
            {
                Body = snake.Body.ToArray(),
                Speed = snake.Speed,
                Direction = snake.Direction,
                Alive = snake.Alive
            };
        }
    }
}

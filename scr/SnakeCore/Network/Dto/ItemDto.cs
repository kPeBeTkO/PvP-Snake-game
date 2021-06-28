using System;
using System.Collections.Generic;
using System.Text;
using SnakeCore.Logic;
using SnakeCore.Logic.Items;

namespace SnakeCore.Network.Dto
{
    public class ItemDto
    {
        public string Type;
        public Vector Location;
        public static ItemDto Convert(Item item)
        {
            return new ItemDto()
            {
                Type = item.GetType().Name,
                Location = item.Position
            };
        }
    }
}

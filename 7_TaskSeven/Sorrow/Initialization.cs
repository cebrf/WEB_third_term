using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sorrow.Data;
using Sorrow.Models;

namespace Sorrow
{
    public class Initialization
    {
        public static void Init(ItemContext context)
        {
            if (!context.Item.Any())
            {
                context.Item.AddRange(
                new Item
                {
                    Name = "21",
                    Field = "34"
                },
                new Item
                {
                    Name = "44",
                    Field = "34"
                }
                );
                context.SaveChanges();
            }
        }
    }
}

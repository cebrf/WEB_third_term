using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _8_mvcApp.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public string User { get; set; }
        public string Address { get; set; }
        public string ContactPhone { get; set; }


        //внешний ключ на связанную модель Phone = 
        public int PhoneId { get; set; } // ссылка на связанную модель Phone
        public Phone Phone { get; set; }
    }
}

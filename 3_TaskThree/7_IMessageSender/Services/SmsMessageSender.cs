using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _7_IMessageSender.Services
{
    public class SmsMessageSender : IMessageSender
    {
        public string Send(HttpContext context)
        {
            if (context.Request.Cookies.ContainsKey("someInfo"))
            {
                return context.Request.Cookies["someInfo"];
            }
            else
            {
                context.Response.Cookies.Append("someInfo", "very important info from cookie");
                return "info was added (cookie)";
            }
        }
    }
}

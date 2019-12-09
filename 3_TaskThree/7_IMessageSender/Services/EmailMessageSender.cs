using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _7_IMessageSender.Services
{
    public class EmailMessageSender : IMessageSender
    {
        public string Send(HttpContext context)
        {
            if (context.Session.Keys.Contains("someInfo"))
            {
                return context.Session.GetString("someInfo");
            }
            else
            {
                context.Session.SetString("someInfo", "very important info from session");
                return "info was added (session)";
            }
        }
    }
}

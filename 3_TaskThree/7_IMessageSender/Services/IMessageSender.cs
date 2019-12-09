using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _7_IMessageSender.Services
{
    public interface IMessageSender
    {
        string Send(HttpContext context);
    }

    public class MessageService
    {
        IMessageSender sender;
        public MessageService(IMessageSender sender)
        {
            this.sender = sender;
        }
        public string Send(HttpContext context)
        {
            return sender.Send(context);
        }
    }
}

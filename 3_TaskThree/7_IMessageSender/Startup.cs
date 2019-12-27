using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using _7_IMessageSender.Services;

namespace _7_IMessageSender
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSession();
            services.AddMvc();

            services.AddTransient<IMessageSender>(provider =>
            {
                Random random = new Random();
                int rund = random.Next();
                /*if (rund % 2 == 0)
                    return new EmailMessageSender();
                else*/ 
                    return new SmsMessageSender();
            });
            services.AddTransient<MessageService>();
        }

        public void Configure(IApplicationBuilder app, MessageService messageService)
        {
            app.UseSession();

            app.Run(async (context) =>
            {
                for (int i = 0; i < 3; i++)
                {
                    await context.Response.WriteAsync(i + ")  " + messageService.Send(context) + '\n');
                }
            });
        }
    }
}

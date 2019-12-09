using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _5_threeMiddleware
{
    public class RoutingMiddleware
    {
        private readonly RequestDelegate next;
        public RoutingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string path = context.Request.Path.Value;
            if (path == "/defaultPage")
            {
                await context.Response.WriteAsync("that is default page. Nowadays it's empty");
            }
            else if (path == "/info")
            {
                await context.Response.WriteAsync("nothing interesting here :|");
            }
            else
            {
                context.Response.StatusCode = 404;
            }
        }
    }
}

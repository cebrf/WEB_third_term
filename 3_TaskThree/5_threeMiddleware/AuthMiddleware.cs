using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _5_threeMiddleware
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate next;
        public AuthMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var password = context.Request.Query["password"];
            if (password == "123")
            {
                await next.Invoke(context);
            }
            else
            {
                context.Response.StatusCode = 423;
            }
        }
    }
}

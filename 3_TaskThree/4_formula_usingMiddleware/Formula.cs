using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _4_formula_usingMiddleware
{
    public class Formula
    {
        private readonly RequestDelegate next;

        public Formula(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var req = context.Request.Query["f"];
            if (req == "1")
            {
                await next.Invoke(context);
            }
            else
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync("write '/?f=1' to see result of rormula");
            }
        }
    }
}

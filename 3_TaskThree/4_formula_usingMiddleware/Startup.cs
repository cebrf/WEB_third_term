using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace _4_formula_usingMiddleware
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<Formula>();

            app.Run(async (context) =>
            {
                double x = 13, y = 0.748, z = 74.6;
                double res = Math.Log(y, x) * Math.Pow(z, -5) * Math.Max(Math.Sin(x), Math.Cos(y)) - Math.Pow(Math.E, (y / Math.PI));

                await context.Response.WriteAsync($"res = {res}");
            });
        }
    }
}

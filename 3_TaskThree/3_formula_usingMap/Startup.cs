using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace _3_formula_usingMap
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
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.Map("/fir", Fir);
            app.Map("/las", Las);

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("go to /fir for first formula\nor to /las for second formula");
            });
        }

        public static void Fir(IApplicationBuilder app)
        {
            int n = 23;
            double res = 0;
            double factorial = 1;
            //formula: sum (from i = 1 to i = n) of i^(i+1) / (i! * e^i)
            for (int i = 1; i < n; i++)
            {
                factorial *= i;
                res += Math.Pow(i, i + 1) / (factorial * Math.Pow(Math.E, (double)i));
            }

            app.Run(async context =>
            {
                await context.Response.WriteAsync($"res = {res}");
            });
        }
        public static void Las(IApplicationBuilder app)
        {
            double x = 13, y = 0.748, z = 74.6;
            double res = Math.Log(y, x) * Math.Pow(z, -5) * Math.Max(Math.Sin(x), Math.Cos(y)) - Math.Pow(Math.E, (y / Math.PI));

            app.Run(async context =>
            {
                await context.Response.WriteAsync($"res = {res}");
            });
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace _3_formula_methodUse
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

            app.UseEndpoints(endpoints =>
            {
                int n = 23;
                double res = 0;
                double factorial = 1;
                app.Use(async (context, next) => 
                {
                    //formula: sum (from i = 1 to i = n) of i^(i+1) / (i! * e^i)
                    for (int i = 1; i < n; i++)
                    {
                        factorial *= i;
                        res += Math.Pow(i, i + 1) / (factorial * Math.Pow(Math.E, (double)i));
                    }

                    await next.Invoke();
                    await context.Response.WriteAsync($"for n == 23\n");
                });

                app.Run(async (context) =>
                {
                    await context.Response.WriteAsync($"res = {res}    ");
                });
            });
        }
    }
}

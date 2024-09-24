using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Threading.Tasks;
using AndresBalladares_Proyecto.Controllers;
using AndresBalladares_Proyecto.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Gestion.UI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Se registran los servicios:
            services.AddScoped<IServicioClientes, ServicioClientes>();
            services.AddScoped<IServicioInventario, ServicioInventario>();
            services.AddScoped<IServicioDirecciones, ServicioDirecciones>();
            services.AddScoped<IServicioPedidos, ServicioPedidos>();
            services.AddScoped<IServicioReportes, ServicioReportes>();
            services.AddMemoryCache();
            services.AddControllersWithViews();
            //Registrar los controladores como servicios para permitir que unos accedan a otros:
            services.AddTransient<ClientesController>();
            services.AddTransient<InventarioController>();
            services.AddTransient<PedidosController>();
            services.AddTransient<DireccionesController>();
            services.AddTransient<ReportesController>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseDefaultFiles();
            app.UseStaticFiles();


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Gestion.UI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}

/*
 * Para que este proyecto se ejecute correctamente se necesita:
 *      - Tener la base de datos 0118520229 en el servidor. El script est� en el archivo Proyecto 3.
 *      - Asegurarse que el usuario y la contrase�a escritos en API.appsettings.json y en Context
 *        coinciden con los del servidor al iniciar sesi�n en SQL Server.
 *      - Asegurarse que baseurl en Services coincide con la direcci�n del API, la cual est� en 
 *        API.LaunchSettings.json.
 *        
 *       .NET utilizado: 7.0.
 */
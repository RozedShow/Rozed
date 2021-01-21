using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Modelos;
using Servicios;

namespace WebApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // CreateHostBuilder(args).Build().Run();
            var host = CreateHostBuilder(args).Build();

            //Creo la carpeta almacenamiento si no existe
            Directory.CreateDirectory("Almacenamiento");
            // //Agrego Un administrador si no existe
            using (var scope = host.Services.CreateScope())
            {
                var logger = scope.ServiceProvider.GetService<ILogger<Program>>();
                var ctx = scope.ServiceProvider.GetService<RChanContext>();
                var aspenv = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                var opt = scope.ServiceProvider.GetService<IConfiguration>();
                var wh = scope.ServiceProvider.GetService<IWebHostEnvironment>();
                var comentarioService = scope.ServiceProvider.GetService<IComentarioService>();
                var hash = scope.ServiceProvider.GetService<HashService>();

                var migs = await ctx.Database.GetPendingMigrationsAsync();

                if(migs.Count() != 0) 
                {
                    var migrations = ctx.Database.GetPendingMigrations();
                    logger.LogInformation("Applicando migraciones pendientes");
                    try
                    {
                        await ctx.Database.MigrateAsync();
                    }
                    catch (Exception e)
                    {
                        logger.LogError("Error al applicar las migraciones");
                        Console.WriteLine(e);
                    }
                }

                var um = scope.ServiceProvider.GetService<SignInManager<UsuarioModel>>().UserManager;
                
                var admin = (await um.GetUsersForClaimAsync(new Claim("Role", "admin"))).FirstOrDefault();

                if(admin is null)
                {
                    var pepe = um.Users.FirstOrDefault(u =>u.UserName == "pepe");
                    if(pepe is null) {
                        var r = await um.CreateAsync(new UsuarioModel {UserName = "pepe"}, "contraseña");
                    }
                    pepe = um.Users.FirstOrDefault(u =>u.UserName == "pepe");
                    await um.AddClaimAsync(pepe, new Claim("Role", "admin"));
                }

                // Inicializar estadisticas
                var estadisticasService = scope.ServiceProvider.GetService<EstadisticasService>();
                
                await estadisticasService.Inicializar(new Estadisticas 
                    {
                        ComentariosCreados = ctx.Comentarios.Count(),
                        HilosCreados = ctx.Hilos.Count(),
                    }
                );
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                    .ConfigureAppConfiguration(cb => {
                        cb.AddJsonFile("appsettings.json",false, true);
                        cb.AddJsonFile("categoriassettings.json",false, true);
                        cb.AddJsonFile("generalsettings.json",false, true);
                        cb.AddCommandLine(args);
                        cb.AddEnvironmentVariables();
                    })
                    .UseStartup<Startup>()
                        .UseUrls("http://0.0.0.0:5000/");
                });
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Ganss.XSS;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Modelos;
using WebApp;

namespace Servicios
{
    public class EstadisticasService
    {
        private static Estadisticas estadisticas;
        private readonly string ubicacionDeArchivo;
        private readonly IHubContext<RChanHub> rchanHub;

        public EstadisticasService(string ubicacionDeArchivo, IHubContext<RChanHub> rchanHub)
        {
            this.ubicacionDeArchivo = ubicacionDeArchivo;
            this.rchanHub = rchanHub;

            Task.Run(async () => {
                var r = new Random();
                while (true)
                {
                    await rchanHub.Clients.Group("rozed").SendAsync("estadisticasActualizadas", estadisticas);
                    await Task.Delay(r.Next(7) * 1000);
                }
            });
        }

        public async Task Guardar()
        {
            await File.WriteAllTextAsync(ubicacionDeArchivo, JsonSerializer.Serialize(estadisticas,
                new JsonSerializerOptions
                {
                    WriteIndented = true,
                })
            );
        }

        public async Task<Estadisticas> GetEstadisticasAsync()
        {
            estadisticas = JsonSerializer.Deserialize<Estadisticas>(await File.ReadAllTextAsync(ubicacionDeArchivo));
            estadisticas.ComputadorasConectadas = RChanHub.NumeroDeUsuariosConectados;
            return estadisticas;
        }

        public async Task RegistrarNuevoHilo()
        {
            estadisticas.HilosCreados++;
            await Guardar();
        }

        public async Task RegistrarNuevoComentario()
        {
            estadisticas.ComentariosCreados++;
            await Guardar();
        }

        public async Task Inicializar(Estadisticas estadisticas)
        {
            if (!File.Exists(ubicacionDeArchivo))
            {
                await File.WriteAllTextAsync(ubicacionDeArchivo, JsonSerializer.Serialize(estadisticas,
                    new JsonSerializerOptions { WriteIndented = true })
                );
            }
        }
    }
}
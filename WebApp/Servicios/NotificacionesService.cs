using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Data;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Modelos;
using WebApp;

namespace Servicios
{

    public class NotificacioensService
    {
        private readonly RChanContext context;
        private readonly HashService hashService;
        private readonly IHubContext<RChanHub> rchanHub;

        public NotificacioensService(
            RChanContext context,
            HashService hashService,
            IHubContext<RChanHub> rchanHub
        )
        {
            this.hashService = hashService;
            this.rchanHub = rchanHub;
            this.context = context;
        }

        public async Task Notificar(HiloModel hilo, ComentarioModel comentario) 
        {
            var comentariosRespondidos = Regex.Matches(comentario.Contenido, @"&gt;&gt;([A-Z0-9]{8})")
                .Select( m => m.Groups[1].Value)
                .Distinct() 
                .ToList();

            var notis = await context.Comentarios
                .Where(c => comentariosRespondidos.Contains(c.Id))
                .Where(c => c.UsuarioId != comentario.UsuarioId)
                .Select(c => new UserNoti(context.Notificaciones.FirstOrDefault(n => n.UsuarioId == c.UsuarioId && n.HiloId == comentario.HiloId && n.Tipo == NotificacionType.Respuesta), c.UsuarioId))
                .ToListAsync();
                

            var ids = notis.Select(u => u.UsuarioId).ToList();
            await EnviarNotificacionTiempoReal(hilo, comentario, ids, NotificacionType.Respuesta);
            ActualizarNotificaciones(notis, comentario, NotificacionType.Respuesta);

            notis = await context.HiloAcciones
                .SeguidoresDeHilo(hilo.Id, context)
                .Where(u => !context.Comentarios.Any(c => comentariosRespondidos.Contains(c.Id) && c.UsuarioId == u.Id))
                .Where(u => u.Id != comentario.UsuarioId)
                .Select(u => new UserNoti(context.Notificaciones.FirstOrDefault(n => n.UsuarioId == u.Id && n.HiloId == comentario.HiloId && n.Tipo == NotificacionType.Comentario), u.Id))
                .ToListAsync();

            ActualizarNotificaciones(notis, comentario, NotificacionType.Comentario);

            await context.SaveChangesAsync();

            ids = notis.Select(u => u.UsuarioId).ToList();
            await EnviarNotificacionTiempoReal(hilo, comentario, ids, NotificacionType.Comentario);

            await context.SaveChangesAsync();
        }

        private async Task EnviarNotificacionTiempoReal(HiloModel hilo, ComentarioModel comentario, List<string> ids, NotificacionType tipo)
        {
            await rchanHub.Clients.Users(ids)
                        .SendAsync("NuevaNotificacion",
                            new
                            {
                                HiloId = hilo.Id,
                                ComentarioId = comentario.Id,
                                Tipo = tipo,
                                Contenido = comentario.Contenido,
                                HiloTitulo = hilo.Titulo,
                                HiloImagen = hilo.Media.VistaPreviaCuadrado
                            });
        }

        private void ActualizarNotificaciones(List<UserNoti> userNoti, ComentarioModel comentario, NotificacionType tipo)
        {
              userNoti.ForEach(un =>
            {
                if (un.Notificacion is null)
                {
                    var nuevaNoti = new NotificacionModel(hashService.Random(), un.UsuarioId, comentario, tipo);
                    context.Notificaciones.Add(nuevaNoti);
                }
                else
                {
                    un.Notificacion.Conteo++;
                    un.Notificacion.Actualizacion = DateTimeOffset.Now;
                }
            });
        }

        private class UserNoti
        {
            public NotificacionModel Notificacion { get; set; }

            public UserNoti(NotificacionModel notificacion, string usuarioId)
            {
                Notificacion = notificacion;
                UsuarioId = usuarioId;
            }

            public string UsuarioId { get; set; }
        }
    }

    public static class AccionesExtensions
    {
        public static IQueryable<UsuarioModel> SeguidoresDeHilo(this IQueryable<HiloAccionModel> acciones,
            string hiloId, RChanContext context)
        {
            return acciones.Where(a => a.Seguido && a.HiloId == hiloId)
                    .Select(a => a.Usuario);
        }
    }

}
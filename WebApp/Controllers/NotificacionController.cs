using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore;
using Microsoft.Extensions.Logging;
using Servicios;
using System.Collections.Generic;
using Modelos;
using System.Threading.Tasks;
using Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace WebApp.Controllers
{
    [Authorize]
    public class NotificacionController : Controller
    {
        private readonly RChanContext _context;
        private readonly IHubContext<RChanHub> rchanHub;

        public NotificacionController(
            RChanContext _context,
            IHubContext<RChanHub> rchanHub
        )
        {
            this._context = _context;
            this.rchanHub = rchanHub;
        }

        [HttpGet("Notificacion/{id}")]
        public async Task<IActionResult> Index(string id, string hiloId="", string comentarioId="")
        {
            var noti = await _context.Notificaciones.AsNoTracking()
                .FirstOrDefaultAsync(n => n.Id == id || (hiloId == n.HiloId && comentarioId== n.ComentarioId && n.UsuarioId == User.GetId()));
            if(noti is null) return Redirect($"/Hilo/{hiloId}#{comentarioId}");

            var query =  _context.Notificaciones.AsQueryable();
                if(noti.Tipo == NotificacionType.Comentario)
                {
                    query = query.Where(n => n.UsuarioId == User.GetId() && n.Tipo == noti.Tipo 
                    && noti.HiloId == n.HiloId);
                }
                else 
                {
                    query = query.Where(n => n.UsuarioId == User.GetId() && n.Tipo == noti.Tipo 
                    && noti.HiloId == n.HiloId &&  n.ComentarioId == noti.ComentarioId);
                }
                
            var notisABorrar = await query.Select(n => new NotificacionModel{Id = n.Id})
                .ToListAsync();

            _context.Notificaciones.RemoveRange(notisABorrar);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine(e);
            }
            

            return Redirect($"/Hilo/{noti.HiloId}#{comentarioId}");
        }
    }
}
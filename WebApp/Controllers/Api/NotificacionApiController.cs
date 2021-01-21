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
    [ApiController, Route("api/Notificacion/{action}/")]

    [Authorize]
    public class NotificacionApiController : Controller
    {
        private readonly RChanContext _context;
        private readonly IHubContext<RChanHub> rchanHub;

        public NotificacionApiController(
            RChanContext _context ,
            IHubContext<RChanHub> rchanHub

        )
        {
            this._context = _context;
            this.rchanHub = rchanHub;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> Limpiar()
        {
            var notisABorrar = await _context.Notificaciones
                .Where(n => n.UsuarioId == User.GetId())
                 .Select(n => new NotificacionModel {Id = n.Id})
                .ToListAsync();

            _context.RemoveRange(notisABorrar);
            await _context.SaveChangesAsync();
            await rchanHub.Clients.Users(User.GetId())
                .SendAsync("notificacionesLimpeadas");

            return new ApiResponse("Notificaciones limpiadas");
        }
    }
}
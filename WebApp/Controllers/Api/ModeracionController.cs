using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore;
using Microsoft.Extensions.Logging;
using Servicios;
using System.Collections.Generic;
using Modelos;
using System.Threading.Tasks;
using System.Net;
using System;
using WebApp;
using Microsoft.AspNetCore.Authorization;
using Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.IO;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Hosting;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Controllers
{
    [Authorize("esAuxiliar")]
    [ApiController, Route("api/Moderacion/{action}/{id?}")]
    public class Moderacion : Controller
    {
        private readonly IHiloService hiloService;
        private readonly IComentarioService comentarioService;
        private readonly IMediaService mediaService;
        private readonly HashService hashService;
        private readonly IHubContext<RChanHub> rchanHub;
        private readonly RChanContext context;
        private readonly UserManager<UsuarioModel> userManager;
        private readonly SignInManager<UsuarioModel> signInManager;
        private readonly IOptions<GeneralOptions> config;
        private readonly IOptions<List<Categoria>> categoriasOpt;
        private readonly AccionesDeModeracionService historial;

        public Moderacion(
            IHiloService hiloService,
            IComentarioService comentarioService,
            IMediaService mediaService,
            HashService hashService,
            IHubContext<RChanHub> rchanHub,
            RChanContext context,
            UserManager<UsuarioModel> userManager,
            SignInManager<UsuarioModel> signInManager,
            IOptionsSnapshot<GeneralOptions> config,
            IOptions<List<Categoria>> categoriasOpt,
            AccionesDeModeracionService historial
        )
        {
            this.hiloService = hiloService;
            this.comentarioService = comentarioService;
            this.mediaService = mediaService;
            this.hashService = hashService;
            this.rchanHub = rchanHub;
            this.context = context;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.config = config;
            this.categoriasOpt = categoriasOpt;
            this.historial = historial;
        }

        [Route("/Moderacion")]
        public async Task<ActionResult> Index()
        {
            var hilos = await context.Hilos.OrderByDescending(h => h.Creacion)
                .AsNoTracking()
                .Take(100)
                .AViewModelMod(context)
                .ToListAsync();

            var comentarios = await context.Comentarios.OrderByDescending(c => c.Creacion)
                .AsNoTracking()
                .Take(100)
                .Include(c => c.Media)
                .AViewModelMod()
                .ToListAsync();

            var denuncias = await context.Denuncias
                .AsNoTracking()
                .OrderByDescending(d => d.Creacion)
                .Take(100)
                .Include(d => d.Hilo)
                .Include(d => d.Usuario)
                .Include(d => d.Comentario)
                .Include(d => d.Comentario.Media)
                .Include(d => d.Hilo.Media)
                .Include(d => d.Hilo.Usuario)
                .Include(d => d.Comentario.Usuario)
                .Where(d => d.Creacion > DateTime.Now - TimeSpan.FromDays(1.5))
                .ToListAsync();

            var medias = await context.Comentarios
                .AsNoTracking()
                .OrderByDescending(d => d.Creacion)
                .Where(c => c.MediaId != null)
                .Include(c => c.Media)
                .Where(c => c.Media.Tipo != MediaType.Eliminado)
                .Take(50)
                .Select(c => new ComentarioViewModelMod
                {
                    HiloId = c.HiloId,
                    UsuarioId = c.UsuarioId,
                    Contenido = c.Contenido,
                    Id = c.Id,
                    Creacion = c.Creacion,
                    Media = c.Media
                }).ToListAsync();

            return View(new { hilos, comentarios, denuncias, medias });
        }

        [Route("/Moderacion/ListaDeUsuarios"), Authorize("esMod")]
        public async Task<ActionResult> ListaDeUsuarios()
        {
            var ultimosRegistros = await context.Users
                .OrderByDescending(u => u.Creacion)
                .Take(100)
                .ToListAsync();

            var cantidadDeUsuarios = await context.Users.CountAsync();

            var ultimosBaneos = await context.Bans
                .OrderByDescending(u => u.Creacion)
                .Include(b => b.Usuario)
                .Where(b => b.Expiracion > DateTime.Now)
                .ToListAsync();
            return View(new { ultimosRegistros, ultimosBaneos, cantidadDeUsuarios });
        }


        [HttpGet]
        [Route("/Moderacion/HistorialDeUsuario/{id}"), Authorize("esMod")]
        public async Task<ActionResult> HistorialDeUsuario(string id)
        {
            var usuario = await context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);
            if (usuario is null)
                return NotFound();
            return View(new
            {
                Usuario = await context.Usuarios.Select(u => new
                {
                    u.Id,
                    u.Creacion,
                    u.UserName,
                    Rozs = context.Hilos.DeUsuario(id).Count(),
                    Comentarios = context.Comentarios.DeUsuario(id).Count(),

                }).FirstOrDefaultAsync(u => u.Id == id),

                Hilos = await context.Hilos
                    .AsNoTracking()
                    .DeUsuario(id)
                    .Recientes()
                    .AViewModelMod(context)
                    .ToListAsync(),

                Comentarios = await context.Comentarios
                    .AsNoTracking()
                    .Recientes()
                    .DeUsuario(id)
                    .Take(150)
                    .AViewModelMod()
                    .ToListAsync(),

                Baneos = await context.Bans
                    .AsNoTracking()
                    .Recientes()
                    .Where(b => b.UsuarioId == id)
                    .Take(100)
                    .Include(b => b.Hilo)
                    .Include(b => b.Comentario)
                    .Include(b => b.Hilo.Media)
                    .Include(b => b.Comentario.Media)
                    .Select(b => new {
                        b.Aclaracion,
                        Comentario = b.Comentario != null? new ComentarioViewModelMod(b.Comentario, b.Hilo): null,
                        b.Creacion,
                        b.Duracion,
                        b.Id,
                        b.Motivo,
                        b.Hilo,
                    })
                    .ToListAsync()

            });
        }

        [Route("/Moderacion/EliminadosYDesactivados")]
        public async Task<ActionResult> EliminadosYDesactivados()
        {
            var hilos = await context.Hilos
                    .Where(h => h.Estado == HiloEstado.Eliminado)
                    .Recientes()
                    .Take(100)
                    .AViewModelMod(context)
                    .ToListAsync();

            var comentarios = await context.Comentarios
                    .Recientes()
                    .Where(h => h.Estado == ComentarioEstado.Eliminado)
                    .Take(150)
                    .AViewModelMod()
                    .ToListAsync();

            return View(new { hilos, comentarios });
        }
        [Route("/Moderacion/Historial"), Authorize("esMod")]
        public async Task<ActionResult> Historial()
        {
            var antesDeAyer = DateTimeOffset.Now - TimeSpan.FromDays(2);
            var acciones = await context.AccionesDeModeracion
                .OrderByDescending(a => a.Creacion)
                .Where(a => a.Creacion > antesDeAyer)
                .Include(a => a.Usuario)
                .Include(a => a.Ban)
                .Include(a => a.Hilo)
                .Include(a => a.Comentario)
                .Include(a => a.Comentario.Media)
                .Include(a => a.Hilo.Media)
                .Include(a => a.Denuncia.Comentario.Media)
                .Include(a => a.Denuncia.Hilo.Media)
                .Include(a => a.Denuncia.Hilo.Usuario)
                .Include(a => a.Denuncia.Comentario.Usuario)
                // .Take(100)
                .ToListAsync();
            
            var accionesVM = acciones.Select(a => new {
                a.Creacion,
                a.Id,
                a.Ban,
                a.Usuario,
                a.Tipo,
                a.TipoElemento,
                a.Nota,
                Hilo =  a.Hilo == null ? null : new HiloViewModel(a.Hilo),
                Comentario =  a.Comentario == null ? null : new ComentarioViewModelMod(a.Comentario, a.Hilo),
                a.Denuncia
            });

            return View(new { Acciones = accionesVM});
        }

        [HttpPost]
        public async Task<ActionResult> Banear(BanViewModel model)
        {
            // var baneado = context.Users.FirstOrDefault(u => u.Id )
            var comentario = await context.Comentarios.FirstOrDefaultAsync(c => c.Id == model.ComentarioId);
            var hilo = await context.Hilos.FirstOrDefaultAsync(h => h.Id == model.HiloId);
            var tipo = comentario is null ? TipoElemento.Hilo : TipoElemento.Comentario;

            CreacionUsuario elemento = comentario ?? (CreacionUsuario)hilo;

            var ban = new BaneoModel
            {
                Id = hashService.Random(),
                Aclaracion = model.Aclaracion,
                ComentarioId = model.ComentarioId,
                Creacion = DateTimeOffset.Now,
                Expiracion = DateTimeOffset.Now + TimeSpan.FromMinutes(model.Duracion),
                ModId = User.GetId(),
                Motivo = model.Motivo,
                Tipo = tipo,
                HiloId = model.HiloId,
                Ip = elemento.Ip,
                UsuarioId = tipo == TipoElemento.Comentario ? comentario.UsuarioId : hilo.UsuarioId,
            };

            context.Bans.Add(ban);
            // Si se marco la opcion para eliminar elemento, borro el hilo o el comentario

            var denuncias = new List<DenunciaModel>();
            if (comentario != null && model.EliminarElemento)
            {
                await comentarioService.Eliminar(comentario.Id);
            }
            else if (hilo != null && model.EliminarElemento)
            {
                await hiloService.EliminarHilos(hilo.Id);
            }
            denuncias.ForEach(d => d.Estado = EstadoDenuncia.Aceptada);

            await rchanHub.Clients.Group("moderacion")
                .SendAsync("denunciasAceptadas", denuncias.Select(d => d.Id).ToArray());

            bool mediaEliminado = false;
            if (model.EliminarAdjunto)
            {
                mediaEliminado = await mediaService.Eliminar(elemento.MediaId);
            }

            //Borro todos los hilos y comentarios del usuario
            if (model.Desaparecer)
            {
                var hilos = await context.Hilos
                    .DeUsuario(elemento.UsuarioId).Select(e => e.Id).ToListAsync();
                var comentarios = await context.Comentarios
                    .DeUsuario(elemento.UsuarioId).Select(e => e.Id).ToListAsync();

                await comentarioService.Eliminar(comentarios.ToArray());
                await hiloService.EliminarHilos(hilos.ToArray());
            }
            await context.SaveChangesAsync();
            await historial.RegistrarBan(User.GetId(), ban);
            return Json(new ApiResponse($"Usuario Baneado {(mediaEliminado ? "; imagen/video eliminado" : "")} {(model.Desaparecer ? "; Usuario desaparecido" : "")}"));
        }

        [HttpPost]
        public async Task<ActionResult> RemoverBan(string id)
        {
            var ban = context.Bans.FirstOrDefault(b => b.Id == id);
            if (ban != null)
            {
                ban.Expiracion = DateTime.Now;
                await context.SaveChangesAsync();
            }
            await historial.RegistrarBanRemovido(User.GetId(), ban);
            return Json(new ApiResponse("Usuario Desbaneado"));
        }

        [HttpPost]
        public async Task<ActionResult> RechazarDenuncia(string id)
        {
            var denuncia = await context.Denuncias.FirstOrDefaultAsync(d => d.Id == id);

            if (denuncia is null)
            {
                ModelState.AddModelError("Denuncia", "No se encontro la denuncia");
                return BadRequest(ModelState);
            }
            
            if(denuncia.Estado == EstadoDenuncia.Rechazada)
                return Json(new ApiResponse("Denuncia rechazada"));
            else if(denuncia.Estado == EstadoDenuncia.Aceptada)
                return Json(new ApiResponse("No se puede rechazar una denuncia aceptada"));

            denuncia.Estado = EstadoDenuncia.Rechazada;

            await context.SaveChangesAsync();

            await rchanHub.Clients.Group("moderacion")
                .SendAsync("denunciasRechazadas", new string[]{denuncia.Id});
            
            await historial.RegistrarDenunciaRechazada(User.GetId(), denuncia);
            return Json(new ApiResponse("Denuncia rechazada"));
        }

        [HttpPost]
        public async Task<ActionResult> EliminarComentarios(BorrarCreacionesVm model)
        {
            var comentarios = await context.Comentarios
                .Where(c => model.Ids.Contains(c.Id))
                .Where(c => c.Estado != ComentarioEstado.Eliminado)
                .ToListAsync();

            var his = comentarios.Select(c => historial.RegistrarEliminacion(User.GetId(), c.HiloId, c.Id));
            await Task.WhenAll(his);

            await comentarioService.Eliminar(model.Ids, model.BorrarMedia);
            return Json(new ApiResponse($"comentarios domados!"));
        }

        [HttpPost]
        public async Task<ActionResult> RestaurarHilo(string id)
        {
            var hilo = await context.Hilos.PorId(id);
            if (hilo is null) return Json(new ApiResponse($"No se eoncontro el roz", false));

            hilo.Estado = HiloEstado.Normal;
            await context.SaveChangesAsync();
            await historial.RegistrarRestauracion(User.GetId(), id);
            return Json(new ApiResponse($"Roz restaurado"));
        }

        public async Task<ActionResult> RestaurarComentario(string id)
        {
            var comentario = await context.Comentarios.PorId(id);
            if (comentario is null) return Json(new ApiResponse($"No se eoncontro el comentario", false));

            comentario.Estado = ComentarioEstado.Normal;
            await context.SaveChangesAsync();
            await historial.RegistrarRestauracion(User.GetId(), comentario.HiloId, id);
            return Json(new ApiResponse($"comentario restaurado"));
        }

        [HttpPost, Authorize("esMod")]
        public async Task<ActionResult> AñadirSticky(Sticky sticky)
        {
            var hilo = await context.Hilos.FirstOrDefaultAsync(h => h.Id == sticky.HiloId);
            if (hilo is null) ModelState.AddModelError("Hilo", "El hilo no existe");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            context.Stickies.RemoveRange(context.Stickies.Where(s => s.HiloId == sticky.HiloId));
            await context.SaveChangesAsync();

            if (sticky.Importancia == 0) return Json(new ApiResponse("Sticky removido"));

            context.Add(sticky);
            await context.SaveChangesAsync();
            return Json(new ApiResponse("Hilo stickeado"));
        }

        public class BorrarCreacionesVm
        {
            public string[] Ids { get; set; }
            public bool BorrarMedia { get; set; }
        }
        [HttpPost]
        public async Task<ActionResult> BorrarHilo(BorrarCreacionesVm vm)
        {
            await hiloService.EliminarHilos(vm.Ids, vm.BorrarMedia, User.GetId());
            return Json(new ApiResponse("Hilo borrado"));
        }

        public class CambiarCategoriaVm
        {
            public string HiloId { get; set; }
            public int CategoriaId { get; set; }
        }

        [HttpPost]
        public async Task<ActionResult> CambiarCategoria(CambiarCategoriaVm vm)
        {
            var hilo = await context.Hilos.FirstOrDefaultAsync(h => h.Id == vm.HiloId);
            if (hilo is null) return NotFound();
            var categoriaAntigua = hilo.CategoriaId;

            if (!categoriasOpt.Value.Any(c => c.Id == vm.CategoriaId))
            {
                ModelState.AddModelError("Categoria", "La categoria es invalida");
                return Json(ModelState);
            }

            hilo.CategoriaId = vm.CategoriaId;

            var denunciasPorCategoriaIncorrecta = await context.Denuncias
                .Where(d => d.HiloId == vm.HiloId)
                .Where(d => d.Motivo == MotivoDenuncia.CategoriaIncorrecta)
                .ToListAsync();
            denunciasPorCategoriaIncorrecta.ForEach(d => d.Estado = EstadoDenuncia.Aceptada);

            await rchanHub.Clients.Group("moderacion")
                .SendAsync("denunciasAceptadas", denunciasPorCategoriaIncorrecta.Select(d => d.Id).ToArray());

            await context.SaveChangesAsync();
            await historial.RegistrarCambioDeCategoria(User.GetId(), hilo.Id, categoriaAntigua, hilo.CategoriaId);
            return Json(new ApiResponse("Categoria cambiada!"));
        }

        [Route("/Moderacion/Media")]
        public async Task<ActionResult> Media()
        {
            var medias = await context.Medias
                .AsNoTracking()
                .OrderByDescending(m => m.Creacion)
                .Where(m => m.Tipo != MediaType.Eliminado)
                .Take(100)
                .ToListAsync();

            return View(new
            {
                medias
            });
        }

        public class EliminarMediaVm
        {
            public string MediaId { get; set; }
            public bool EliminarElementos { get; set; }
        }
        [HttpPost]
        public async Task<ActionResult> EliminarMedia(string[] ids)
        {
            var medias = await context.Medias.Where(m => ids.Contains(m.Id)).ToListAsync();

            foreach (var m in medias)
            {
                await mediaService.Eliminar(m.Id);

                var comentarios = await context.Comentarios
                    .Where(c => c.MediaId == m.Id).ToListAsync();
                var hilos = await context.Hilos
                    .Where(c => c.MediaId == m.Id).ToListAsync();

                comentarios.ForEach(c => c.Estado = ComentarioEstado.Eliminado);
                hilos.ForEach(c => c.Estado = HiloEstado.Eliminado);

            }
            await context.SaveChangesAsync();

            return Json(new ApiResponse("Archivos Eliminados"));
        }
    }

    public class ModeracionIndexVm
    {
        public ModeracionIndexVm(List<HiloViewModelMod> hilos, List<ComentarioViewModelMod> comentarios, List<DenunciaModel> denuncias)
        {
            this.Hilos = hilos;
            this.Comentarios = comentarios;
            this.Denuncias = denuncias;
        }

        public List<HiloViewModelMod> Hilos { get; set; }
        public List<ComentarioViewModelMod> Comentarios { get; set; }
        public List<DenunciaModel> Denuncias { get; set; }
    }

    public class BanViewModel
    {
        public string UsuarioId { get; set; }
        public string HiloId { get; set; }
        [Required, Range(0, 10, ErrorMessage = "Motivo Invalido")]
        public MotivoDenuncia Motivo { get; set; }
        public string Aclaracion { get; set; }
        public string ComentarioId { get; set; }
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Tenes que elegir una duracion para el ban")]
        public int Duracion { get; set; }
        public bool EliminarElemento { get; set; }
        public bool EliminarAdjunto { get; set; }
        public bool Desaparecer { get; set; }
    }
}
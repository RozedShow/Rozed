using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Servicios;
using System.Collections.Generic;
using Modelos;
using System.Threading.Tasks;
using Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;

namespace WebApp.Controllers
{
    public class HiloController : Controller
    {
        private readonly ILogger<HiloController> _logger;
        private readonly IHiloService hiloService;
        private readonly RChanContext context;
        private readonly IOptions<List<Categoria>> categoriasOpts;

        public IMediaService MediaService { get; }

        public HiloController(
            ILogger<HiloController> logger,
            IHiloService hiloService,
            RChanContext context,
            IOptions<List<Categoria>> categoriasOpts,
            IMediaService mediaService)
        {
            _logger = logger;
            this.hiloService = hiloService;
            this.context = context;
            this.categoriasOpts = categoriasOpts;
            MediaService = mediaService;
        }

        public async Task<IActionResult> IndexAsync()
        {
            int[] categorias = categoriasOpts.Value.Sfw().Ids().ToArray();

            HttpContext.Request.Cookies.TryGetValue("categoriasActivas", out string categoriasActivas);

            if (categoriasActivas != null) categorias = JsonSerializer.Deserialize<int[]>(categoriasActivas);
            
            var vm = new HiloListViewModel
            {
                Hilos = await hiloService.GetHilosOrdenadosPorBump(new GetHilosOptions
                {
                    UserId = User.GetId(),
                    CategoriasId = categorias,
                    IncluirStickies = true
                }),
                CategoriasActivas = categorias.ToList()
            };
            return View(vm);
        }

        [HttpGet("/Favoritas")]
        public async Task<IActionResult> Favoritas()
        {
            int[] categorias = new int[]{};

            HttpContext.Request.Cookies.TryGetValue("categoriasFavoritas", out string categoriasActivas);

            if (categoriasActivas != null) categorias = JsonSerializer.Deserialize<int[]>(categoriasActivas);
            
            var vm = new HiloListViewModel
            {
                Hilos = await hiloService.GetHilosOrdenadosPorBump(new GetHilosOptions
                {
                    UserId = User.GetId(),
                    CategoriasId = categorias,
                    IncluirStickies = true
                }),
                CategoriasActivas = categorias.ToList()
            };
            return View("Index", vm);
        }

        [HttpGet("Hilo/{id}")]
        public async Task<IActionResult> MostrarAsync(string id)
        {
            // var hilo = await hiloService.GetHiloFull(id, User.GetId());
             IHiloFullView hilo;
             if(User.EsMod())
             {
                 hilo =  await hiloService.GetHiloFullMod(id, User.GetId(), true);
             }
             else {
                 hilo =  await hiloService.GetHiloFull(id, User.GetId());
             }
            if (hilo is null) return NotFound();
            // return Json(new {
            //     hilo.Hilo,
            //     hilo.Comentarios,
            //     hilo.Acciones,
            //     Usuario = GetUserInfo(),
            //     Notificaciones = await GetNotis()
            // });
            return View("MostrarSvelte", hilo);
        }

        [Route("/{categoria}")] // Ej /anm, /art, /ytb
        public async Task<IActionResult> CategoriaAsync(string categoria)
        {
            var cate = categoriasOpts.Value.FirstOrDefault(c => c.NombreCorto.ToLower() == categoria.ToLower());
            if (cate == null)
            {
                return NotFound();
            };

            var vm = new HiloListViewModel
            {
                
                // Hilos = await context.Hilos
                //     .Where(h => h.CategoriaId == cate.Id)
                //     .AsNoTracking()
                //     .OrdenadosPorBump()
                //     .FiltrarOcultosDeUsuario(User.GetId(), context)
                //     .FiltrarNoActivos()
                //     .Take(16)
                //     .AViewModel(context)
                //     .ToListAsync(),
                Hilos = await hiloService.GetCategoria(cate.Id, User.GetId()),
                CategoriasActivas = new int[] { cate.Id }.ToList()
            };
            return View("Index", vm);
        }

        [Authorize]
        [HttpGet("/Mis/{coleccion?}")]
        public async Task<IActionResult> Coleciones(string coleccion)
        {
            if (coleccion == null || !"favoritos ocultos seguidos creados".Split(" ").Contains(coleccion.ToLower()))
            {
                return NotFound();
            }
            coleccion = coleccion.ToLower();
            var query = hiloService
                .OrdenadosPorBump()
                .FiltrarNoActivos();

            query = coleccion switch
            {
                "favoritos" => query.Where(h =>
                        context.HiloAcciones.Any(a => a.HiloId == h.Id && a.UsuarioId == User.GetId() && a.Favorito)),
                "ocultos" => query.Where(h =>
                        context.HiloAcciones.Any(a => a.HiloId == h.Id && a.UsuarioId == User.GetId() && a.Hideado)),
                "seguidos" => query.Where(h =>
                        context.HiloAcciones.Any(a => a.HiloId == h.Id && a.UsuarioId == User.GetId() && a.Seguido)),
                "creados" => query.Where(h => h.UsuarioId == User.GetId()),
                _ => null
            };
            
            var hilos = await query.AViewModel(context).ToListAsync();
            var vm = new HiloListViewModel { Hilos = hilos, CategoriasActivas = new List<int>() };
            return View("Index", vm);
        }

        [HttpGet("/Buscar")]
        public IActionResult Buscar() 
        {
            return View();         
        }

        private UsuarioVm GetUserInfo()
        {
            return new UsuarioVm
            {
                EstaAutenticado = User.Identity.IsAuthenticated,
                UserName = User.Identity.Name,
                EsAdmin = User.EsAdmin(),
                EsMod = User.EsMod(),
                EsAuxiliar= User.EsAuxiliar(),
            };
        }

        private async Task<List<NotificacionViewModel>> GetNotis()
        {
            if(User is null) return null;
            return await context.Notificaciones
                .Where(n => n.UsuarioId == User.GetId())
                .Select(n => new NotificacionViewModel
                {
                    Id = n.Id,
                    HiloId = n.HiloId,
                    HiloTitulo = context.Hilos.First(h => h.Id == n.HiloId).Titulo,
                    HiloImagen = context.Medias.First(m => m.Id == context.Hilos.First(h => h.Id == n.HiloId).MediaId).VistaPreviaCuadrado,
                    ComentarioId = n.ComentarioId,
                    Tipo = n.Tipo,
                    Conteo = n.Conteo,
                })
                .AsNoTracking()
                .ToListAsync();
        }
    }


}

class UsuarioVm
{
    public bool EstaAutenticado { get; set; }
    public string UserName { get; set; }
    public bool EsAdmin { get; set; }
    public bool EsMod { get; set; }
    public bool EsAuxiliar{ get; set; }
}

public class HiloListViewModel
{
    public List<HiloViewModel> Hilos { get; set; }
    public List<int> CategoriasActivas { get; set; }
}
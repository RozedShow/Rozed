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

namespace WebApp.Controllers
{
    [Authorize("esAdmin")]
    [ApiController, Route("api/Administracion/{action}/{id?}")]
    public class Administracion : Controller
    {
        private readonly IHiloService hiloService;
        private readonly IMediaService mediaService;
        private readonly HashService hashService;
        private readonly IHubContext<RChanHub> rchanHub;
        private readonly RChanContext context;
        private readonly UserManager<UsuarioModel> userManager;
        private readonly SignInManager<UsuarioModel> signInManager;
        private readonly IOptions<GeneralOptions> config;
        private readonly IOptions<List<Categoria>> categoriasOpt;

        public Administracion(
            IHiloService hiloService,
            IMediaService mediaService,
            HashService hashService,
            IHubContext<RChanHub> rchanHub,
            RChanContext context,
            UserManager<UsuarioModel> userManager,
            SignInManager<UsuarioModel> signInManager,
            IOptionsSnapshot<GeneralOptions> config,
            IOptions<List<Categoria>> categoriasOpt
        )
        {
            this.hiloService = hiloService;
            this.mediaService = mediaService;
            this.hashService = hashService;
            this.rchanHub = rchanHub;
            this.context = context;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.config = config;
            this.categoriasOpt = categoriasOpt;
        }
        [Route("/Administracion")]
        public async Task<ActionResult> Index()
        {
            var admins = await userManager.GetUsersForClaimAsync(new Claim("Role", "admin"));
            var mods = await userManager.GetUsersForClaimAsync(new Claim("Role", "mod"));
            var auxiliares = await userManager.GetUsersForClaimAsync(new Claim("Role", "auxiliar"));
            var vm = new AdministracionVM
            {
                Admins = admins.Select(u => new UsuarioVM { Id = u.Id, UserName = u.UserName }).ToArray(),
                Mods = mods.Select(u => new UsuarioVM { Id = u.Id, UserName = u.UserName }).ToArray(),
                Auxiliares  = auxiliares.Select(u => new UsuarioVM { Id = u.Id, UserName = u.UserName }).ToArray(),
                Config = config.Value
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<ActionResult> ActualizarConfiguracion(GeneralOptions config, [FromServices] IWebHostEnvironment host) 
        {
            await config.Guardar(Path.Combine(host.ContentRootPath, "generalsettings.json"));

            // Juntar con el layout
            await rchanHub.Clients.Groups("rozed").SendAsync("configuracionActualizada", new {
                config.TiempoEntreComentarios,
                config.TiempoEntreHilos,
                config.LimiteArchivo,
                config.RegistroAbierto,
                config.CaptchaHilo,
                config.CaptchaComentario,
                config.CaptchaRegistro,
                config.Version,
                config.ModoMessi,
                config.ModoSerenito,
                config.Flags});

            return Json(new ApiResponse("Configuracion actualizada"));
        }

        [HttpPost]
        public async Task<ActionResult> GenerarNuevoLinkDeInvitacion([FromServices]  IWebHostEnvironment host)
        {
            string link = hashService.Random(8);
            config.Value.LinkDeInvitacion = link;
            await config.Value.Guardar(Path.Combine(host.ContentRootPath, "generalsettings.json"));
            return Json(new {Link = link});
        }

        public class RolUserVM
        {
            public string Username { get; set; }
            public string Role { get; set; }
        }
        [HttpPost]
        public async Task<ActionResult> AñadirRol(RolUserVM model)
        {
            var role = model.Role;
            var username = model.Username;

            if (!new[] { "admin", "mod", "auxiliar" }.Contains(role))
                ModelState.AddModelError("Rol", "Rol invalido");


            var user = await userManager.Users.FirstOrDefaultAsync(u => u.UserName == username || u.Id == username);
            if (user is null)
                ModelState.AddModelError("userName", "No se encontre al usuario");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bool yaTieneElRol = (await userManager.GetClaimsAsync(user)).FirstOrDefault(c => c.Type == "Role") != null;

            if (yaTieneElRol)
            {
                ModelState.AddModelError("", "El usuario ya tiene ese rol");
                return BadRequest(ModelState);
            }

            var result = await userManager.AddClaimAsync(user, new Claim("Role", role));
            if (result.Succeeded)
            {
                return Json(new ApiResponse($"{user.UserName} ahora es {role}"));
            }
            else
                return BadRequest(result.Errors);
        }
        public async Task<ActionResult> RemoverRol(RolUserVM model)
        {
            var role = model.Role;
            var username = model.Username;

            if (!new[] { "admin", "mod", "auxiliar" }.Contains(role))
                ModelState.AddModelError("Rol", "Rol invalido");


            var user = await userManager.Users.FirstOrDefaultAsync(u => u.UserName == username || u.Id == username);
            if (user is null)
                ModelState.AddModelError("userName", "No se encontre al usuario");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var result = await userManager.RemoveClaimAsync(user, new Claim("Role", role));
            if (result.Succeeded)
            {
                // await signInManager.RefreshSignInAsync(user);
                return Json(new ApiResponse($"{user.UserName} ya no es {role}"));
            }
            else
                return BadRequest(result.Errors);
        }

        [Route("/Administracion/CambiarContraseña")]
        public async Task<ActionResult> CambiarContraseña () {
            return View();
        }

        [Route("/Administracion/CambiarContraseña"), HttpPost]
        public async Task<ActionResult> CambiarContraseña ([FromForm] string contraseñaVieja, [FromForm] string contraseñaNueva) 
        {
            var user = await userManager.GetUserAsync(User);
            var result = await userManager.ChangePasswordAsync(user, contraseñaVieja, contraseñaNueva);

            if(result.Succeeded)
            {
                return Redirect("/");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<ActionResult> LimpiarRozesViejos () 
        {
            var dosDiasAtras = DateTimeOffset.Now - TimeSpan.FromDays(2);
            var hilosALimpiar = await context.Hilos
                .Where(h => h.Estado == HiloEstado.Archivado || h.Estado == HiloEstado.Eliminado)
                .Where(h => h.Creacion < dosDiasAtras)
                .ToListAsync();
            
            foreach (var h in hilosALimpiar)
            {
                await hiloService.LimpiarHilo(h);
            }
            await context.SaveChangesAsync();
            var ArchivosLimpiados = await mediaService.LimpiarMediasHuerfanos();

                return Json(new ApiResponse($"{hilosALimpiar.Count()} hilos limpiados y {ArchivosLimpiados} archivos limpiados"));

        } 
    }
}

public class AdministracionVM
{
    public UsuarioVM[] Admins { get; set; }
    public UsuarioVM[] Mods { get; set; }
    public UsuarioVM[] Auxiliares { get; set; }
    public GeneralOptions Config { get; internal set; }
}

public class UsuarioVM
{
    public string Id { get; set; }
    public string UserName { get; set; }
}


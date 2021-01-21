using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Modelos;
using Data;
using SqlKata.Execution;
using SqlKata.Compilers;
using System.Linq;
using Dapper;
using Microsoft.Extensions.Options;
using WebApp;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Servicios
{
    public interface IHiloService
    {
        Task<List<HiloViewModel>> GetHilosOrdenadosPorBump();
        Task<List<HiloViewModel>> GetHilosOrdenadosPorBump(GetHilosOptions opciones);
        Task<string> GuardarHilo(HiloModel Hilo);
        Task ActualizarHilo(HiloModel Hilo);
        Task<HiloViewModel> GetHilo(string id, bool mostrarOcultos);
        Task<HiloFullViewModel> GetHiloFull(string id, string userId = null, bool mostrarOcultos = false);
        Task<HiloFullViewModelMod> GetHiloFullMod(string id, string userId = null, bool mostrarOcultos = false);
        IQueryable<HiloModel> OrdenadosPorBump();
        Task<List<HiloViewModel>> GetCategoria(int categoria, string usuarioId="", int cantidad = 16);
        Task EliminarHilos(params string[] ids);
        Task EliminarHilos(string[] ids, bool borrarMedias = false, string usuarioId="");
        Task LimpiarHilo(string id);
        Task LimpiarHilo(HiloModel hilo);
        Task LimpiarHilosViejos();
        
    }

    public class HiloService : ContextService, IHiloService
    {
        private readonly IComentarioService comentarioService;
        private readonly IOptionsSnapshot<GeneralOptions> options;
        private readonly FormateadorService formateador;
        private readonly IHubContext<RChanHub> rchanHub;
        private readonly IMediaService mediaService;
        private readonly AccionesDeModeracionService historial;
        private readonly ILogger<HiloService> logger;

        public HiloService(RChanContext context,
            HashService hashService,
            IComentarioService comentarioService,
            IOptionsSnapshot<GeneralOptions> options,
            FormateadorService formateador,
            IHubContext<RChanHub> rchanHub, 
            IMediaService mediaService,
            AccionesDeModeracionService historial,
            ILogger<HiloService> logger
            )
        : base(context, hashService)
        {
            this.comentarioService = comentarioService;
            this.options = options;
            this.formateador = formateador;
            this.rchanHub = rchanHub;
            this.mediaService = mediaService;
            this.historial = historial;
            this.logger = logger;
        }

        public async Task ActualizarHilo(HiloModel Hilo)
        {
            _context.Hilos.Update(Hilo);
            await _context.SaveChangesAsync();
        }

        public async Task<HiloViewModel> GetHilo(string id, bool mostrarOcultos = false)
        {
            var hilo = await _context.Hilos
                .FirstOrDefaultAsync(h =>
                h.Id == id &&
                (h.Estado ==  HiloEstado.Normal || mostrarOcultos));

            if (hilo is null) return null;
            return new HiloViewModel(hilo);

        }
        public async Task<HiloFullViewModel> GetHiloFull(string id, string userId = null, bool mostrarOcultos = false)
        {
            var hiloFullView = new HiloFullViewModel();

            HiloModel hilo;
            var query = _context.Hilos
                .Include(h => h.Media);

            if(!mostrarOcultos) 
                hilo = await query.FiltrarEliminados().PorId(id);
            else 
                hilo = await query.PorId(id);
            

            if (hilo is null) return null;

            hiloFullView.Hilo = new HiloViewModel(hilo);

            if(hilo.Encuesta != null) 
            {
                hiloFullView.Hilo.EncuestaData = new EncuestaViewModel(hilo.Encuesta, userId);
            }

            hiloFullView.Comentarios =  await _context.Comentarios
                .Where(c => c.HiloId == hilo.Id)
                .Where(c => c.Estado == ComentarioEstado.Normal)
                .OrderByDescending(c => c.Creacion)
                .Include(c => c.Media)
                .Select(c => new ComentarioViewModel(c, hilo))
                .ToListAsync();

             if (!string.IsNullOrEmpty(userId))
             {
                 hiloFullView.Acciones = await _context.HiloAcciones
                    .FirstOrDefaultAsync(a => a.UsuarioId == userId && a.HiloId == id);
             }
             
             hiloFullView.Acciones ??= new HiloAccionModel();

            return hiloFullView;

        }
        public async Task<HiloFullViewModelMod> GetHiloFullMod(string id, string userId = null, bool mostrarOcultos = false)
        {
            var hiloFullView = new HiloFullViewModelMod();


            HiloModel hilo;
            var query = _context.Hilos
                .Include(h => h.Media);

            if(!mostrarOcultos) 
                hilo = await query.FiltrarEliminados().PorId(id);
            else 
                hilo = await query.PorId(id);
            

            if (hilo is null) return null;

            hiloFullView.Usuario =  await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == hilo.UsuarioId);
            hiloFullView.Hilo = new HiloViewModel(hilo);

             if(hilo.Encuesta != null) 
            {
                hiloFullView.Hilo.EncuestaData = new EncuestaViewModel(hilo.Encuesta, userId);
            }

            hiloFullView.Comentarios = await _context.Comentarios
                .AsNoTracking()
                .Where(c => c.HiloId == id)
                .Where(c => c.Estado != ComentarioEstado.Eliminado || mostrarOcultos)
                .OrderByDescending(c => c.Creacion)
                .Include(c => c.Media)
                .Select(c => new ComentarioViewModelMod(c, hilo))
                .ToListAsync();

             if (!string.IsNullOrEmpty(userId))
             {
                 hiloFullView.Acciones = await _context.HiloAcciones
                    .FirstOrDefaultAsync(a => a.UsuarioId == userId && a.HiloId == id);
             }
             
             hiloFullView.Acciones ??= new HiloAccionModel();

            return hiloFullView;

        }

        public Task<List<HiloViewModel>> GetHilosOrdenadosPorBump()
        {
            return GetHilosOrdenadosPorBump(new GetHilosOptions());
        }

        public async Task<List<HiloViewModel>> GetHilosOrdenadosPorBump(GetHilosOptions opciones)
        {
            // Mejorar esto
            var query = _context.Hilos
                .AsNoTracking()
                .Where(h => opciones.CategoriasId.Contains(h.CategoriaId) && 
                !_context.HiloAcciones.Any(a => a.HiloId ==  h.Id && a.UsuarioId == opciones.UserId && a.Hideado));

            if(opciones.CategoriasId.Length != 1) 
            {
                query = query.Where( h => !_context.Stickies.Any( s => s.HiloId == h.Id && s.Global ) );
            }

            var hilos = await query.FiltrarNoActivos()
                .Where( h => !_context.Stickies.Any( s => s.HiloId == h.Id && !s.Global))
                .OrderByDescending(h => h.Bump)
                .Take(opciones.Cantidad)
                .AViewModel(_context).ToListAsync();
            

            List<HiloViewModel> hilosStickies = null;
            if(opciones.CategoriasId.Length > 1)
            {
                hilosStickies = await  _context.Stickies
                    .AsNoTracking()
                    .Where( s => !_context.HiloAcciones.Any(a => a.HiloId ==  s.HiloId && a.UsuarioId == opciones.UserId && a.Hideado))
                    .Where(s => s.Global)
                    .Select(s => _context.Hilos.FirstOrDefault(h => h.Id == s.HiloId))
                    .AViewModel(_context).ToListAsync();
            }
            else if(opciones.CategoriasId.Length == 0) 
            {
                return new List<HiloViewModel>();
            }
            else 
            {
                hilosStickies = await  _context.Stickies
                    .AsNoTracking()
                    .Where( s => !_context.HiloAcciones.Any(a => a.HiloId ==  s.HiloId && a.UsuarioId == opciones.UserId && a.Hideado))
                    .Where(s => !s.Global)
                    .Select(s => _context.Hilos.FirstOrDefault(h => h.Id == s.HiloId))
                    .Where(h => h.CategoriaId == opciones.CategoriasId[0])
                    .AViewModel(_context).ToListAsync();
            }
            
            var stickies = await _context.Stickies.ToListAsync();
            
            hilosStickies.ForEach(h => h.Sticky = stickies.First(s => s.HiloId == h.Id).Importancia);

            return  hilosStickies.OrderByDescending(h => h.Sticky).Concat(hilos).ToList();
        }

        public async Task<List<HiloViewModel>> GetCategoria(int categoria, string usuarioId="", int cantidad = 16)
        {
            // Mejorar esto
            var query = _context.Hilos
                .AsNoTracking()
                .FiltrarNoActivos()
                .FiltrarOcultosDeUsuario(usuarioId, _context)
                .Where(h => h.CategoriaId == categoria);

            var hilos = await query.FiltrarNoActivos()
                .Where( h => !_context.Stickies.Any( s => s.HiloId == h.Id && !s.Global))
                .OrdenadosPorBump()
                .Take(cantidad)
                .AViewModel(_context).ToListAsync();
            

            List<HiloViewModel> hilosStickies = null;

            hilosStickies = await  _context.Stickies
                .AsNoTracking()
                .Where(s => !s.Global)
                .Select(s => _context.Hilos.FirstOrDefault(h => h.Id == s.HiloId))
                .FiltrarNoActivos()
                .FiltrarOcultosDeUsuario(usuarioId, _context)
                .Where(h => h.CategoriaId == categoria)
                .AViewModel(_context)
                .ToListAsync();
           
            
            var stickies = await _context.Stickies.Where(s => !s.Global).ToListAsync();
            
            hilosStickies.ForEach(h => h.Sticky = stickies.First(s => s.HiloId == h.Id).Importancia);

            return  hilosStickies.OrderByDescending(h => h.Sticky).Concat(hilos).ToList();
        }

        // public async Task<List<HiloViewModel>> GetHilosRecientes(GetHilosOptions opciones)
        // {
        // }

        public async Task<string> GuardarHilo(HiloModel hilo)
        {
            hilo.Id = hashService.Random();
            _context.Hilos.Add(hilo);
            await _context.SaveChangesAsync();
            // Ahora busco todos los hilos con estado activo
            var hilosParaArchivar = await _context.Hilos
                .FiltrarNoActivos()
                .OrdenadosPorBump()
                .FiltrarPorCategoria(hilo.CategoriaId)
                .Skip(options.Value.HilosMaximosPorCategoria)
                .ToListAsync();

            hilosParaArchivar.ForEach(h => h.Estado = HiloEstado.Archivado);

            // Flags
            if(hilo.Contenido.Contains(">>dados")) hilo.Flags += "d";
            if(hilo.Contenido.Contains(">>idunico")) hilo.Flags += "i";

            hilo.Contenido = formateador.Parsear(hilo.Contenido);
            await _context.SaveChangesAsync();
            return hilo.Id;
        }

        public IQueryable<HiloModel> OrdenadosPorBump() {
            return _context.Hilos
                .Include(h => h.Media)
                .OrderByDescending(h => h.Bump);
        }

        public Task EliminarHilos(params string[] ids) => EliminarHilos(ids, false);

        public async Task EliminarHilos(string[] ids, bool borrarMedias = false, string usuarioId="")
        {
            var hilos = await _context.Hilos
                .Where(h => ids.Contains(h.Id))
                .Where(h => h.Estado != HiloEstado.Eliminado)
                .ToListAsync();

            hilos.ForEach(h => h.Estado = HiloEstado.Eliminado);

            //Limpiar denuncias
            var denuncias = await _context.Denuncias.Where(d => ids.Contains(d.HiloId)).ToListAsync();
            denuncias.ForEach(d => d.Estado = EstadoDenuncia.Aceptada);

            if(borrarMedias) 
            {
                var mediaIds = hilos.Select(h => h.MediaId).ToArray();
                foreach (var m in mediaIds)
                {
                    await mediaService.Eliminar(m);
                }
            }

            await _context.SaveChangesAsync();

            // Historial
            await Task.WhenAll(hilos.Select( h => 
                historial.RegistrarEliminacion(usuarioId, h.Id)));

            await rchanHub.Clients.All.SendAsync("HilosEliminados", ids);

        }

        public async Task LimpiarHilo(HiloModel hilo)
        {
            var baneos = await _context.Bans.Where(d => d.HiloId == hilo.Id).ToListAsync();
            var acciones = await _context.HiloAcciones.Where(d => d.HiloId == hilo.Id).ToListAsync();
            var notis = await _context.Notificaciones.Where( n => n.HiloId == hilo.Id).ToListAsync();
            var denuncias = await _context.Denuncias.Where(d => d.HiloId == hilo.Id).ToListAsync();
            
            _context.RemoveRange(acciones);
            _context.RemoveRange(notis);
            _context.RemoveRange(denuncias);

            if(baneos.Count == 0) 
            {
                _context.Remove(hilo);
            } 
            else 
            {
                var comentarios = await  _context.Comentarios
                    .Where(c => c.Hilo.Id == hilo.Id)
                    .Where(c => !_context.Bans.Any(b => b.ComentarioId == c.Id))
                    .ToListAsync();
                _context.RemoveRange(comentarios);
            }
            await _context.SaveChangesAsync();
        }

        public async Task LimpiarHilo(string id)
        {
            var hilo = await _context.Hilos.PorId(id);
            if(hilo != null) await LimpiarHilo(hilo);
        }
        public async Task LimpiarHilosViejos () 
        {
            var dosDiasAtras = DateTimeOffset.Now - TimeSpan.FromDays(2);
            var hilosALimpiar = await _context.Hilos
                .Where(h => h.Estado == HiloEstado.Archivado || h.Estado == HiloEstado.Eliminado)
                .Where(h => h.Creacion < dosDiasAtras)
                .Where(h => !_context.Bans.Any(b => b.HiloId == h.Id && b.ComentarioId == null))
                .ToListAsync();
            
            int total = hilosALimpiar.Count();
            int limpiados = 0;
            foreach (var h in hilosALimpiar)
            {
                logger.LogInformation($"Limpeando hilo {h.Titulo}({limpiados}/{total})");
                await LimpiarHilo(h);
                limpiados++;
            }
            await _context.SaveChangesAsync();
            var ArchivosLimpiados = await mediaService.LimpiarMediasHuerfanos();

            logger.LogInformation($"{hilosALimpiar.Count()} hilos limpiados y {ArchivosLimpiados} archivos limpiados");

        } 
    }

    public class GetHilosOptions
    {
        public int Cantidad { get; set; } = 32;
        public int[] CategoriasId { get; set; } = new int[]{};
        public int[] IdsExcluidas { get; set; } = new int[0];
        public bool IncluirStickies { get; set; } = false;
        public string UserId { get; set; }
        public bool MostrarBorrados { get; set; } = false;
    }

     public static class HiloExtensions
    {
        public static IQueryable<HiloViewModel> AViewModel(this IQueryable<HiloModel> hilos, RChanContext context) {
            return hilos.Select(h => new HiloViewModel {
                    Bump = h.Bump,
                    CategoriaId = h.CategoriaId,
                    Contenido = h.Contenido,
                    Creacion = h.Creacion,
                    Media = h.Media,
                    Id = h.Id,
                    Titulo = h.Titulo,
                    Estado = h.Estado,
                    Dados = h.Flags.Contains("d"),
                    Encuesta = h.Encuesta != null,
                    CantidadComentarios = context.Comentarios.Where(c => c.HiloId == h.Id && c.Estado == ComentarioEstado.Normal).Count()
                });
        }
        public static IQueryable<HiloViewModelMod> AViewModelMod(this IQueryable<HiloModel> hilos, RChanContext context) {
            return hilos.Select(h => new HiloViewModelMod {
                    Bump = h.Bump,
                    CategoriaId = h.CategoriaId,
                    Contenido = h.Contenido,
                    Creacion = h.Creacion,
                    Media = h.Media,
                    Id = h.Id,
                    Titulo = h.Titulo,
                    Estado = h.Estado,
                    Usuario = h.Usuario,
                    Dados = h.Flags.Contains("d"),
                    Encuesta = h.Encuesta != null,
                    CantidadComentarios = context.Comentarios.Where(c => c.HiloId == h.Id && c.Estado == ComentarioEstado.Normal).Count(),
                    UsuarioId = h.UsuarioId,
                });
        }

        public static IOrderedQueryable<HiloModel> OrdenadosPorBump(this IQueryable<HiloModel> hilos) {
            return hilos.OrderByDescending(h => h.Bump);
        }

        public static IQueryable<HiloModel> FiltrarNoActivos(this IQueryable<HiloModel> hilos) {
            return hilos.Where(h => h.Estado == HiloEstado.Normal);
        }
        public static IQueryable<HiloModel> FiltrarEliminados(this IQueryable<HiloModel> hilos) {
            return hilos.Where(h => h.Estado == HiloEstado.Normal || h.Estado == HiloEstado.Archivado);
        }

        public static IQueryable<HiloModel> FiltrarPorCategoria(this IQueryable<HiloModel> hilos, params int[] categorias) {
            return hilos.Where(h => categorias.Contains(h.CategoriaId));
        }

        public static IQueryable<HiloModel> FiltrarOcultosDeUsuario(this IQueryable<HiloModel> hilos, string usuarioId, RChanContext context) {
            return hilos.Where( h => !context.HiloAcciones.Any(a => a.HiloId ==  h.Id && a.UsuarioId == usuarioId && a.Hideado));
        }
        public static IQueryable<T> DeUsuario<T>(this IQueryable<T> creaciones, string usuarioId) where T : CreacionUsuario{
            return creaciones.Where( h => h.UsuarioId == usuarioId);
        }
        public static IQueryable<T> Recientes<T> (this IQueryable<T> elemento) where T : BaseModel
        { 
            return elemento.OrderByDescending( h => h.Creacion);
        }
        public static Task<T> PorId<T> (this IQueryable<T> elemento, string id) where T : BaseModel
        { 
            return elemento.FirstOrDefaultAsync(e => e.Id == id);
        }

    }
}

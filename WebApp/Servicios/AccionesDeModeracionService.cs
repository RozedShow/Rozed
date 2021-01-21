using System;
using System.Linq;
using Data;
using Modelos;
using Microsoft.AspNetCore.SignalR;
using WebApp;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace Servicios
{
    public class AccionesDeModeracionService
    {
        private readonly RChanContext context;
        private readonly IHubContext<RChanHub> rchanHub;
        private readonly HashService hashService;
        private readonly IOptions<List<Categoria>> categorias;

        public AccionesDeModeracionService(
            RChanContext context,  
            IHubContext<RChanHub> rchanHub,
            HashService hashService,
            IOptions<List<Categoria>> categorias
        )
        {
            this.context = context;
            this.rchanHub = rchanHub;
            this.hashService = hashService;
            this.categorias = categorias;
        }

        async public Task RegistrarBan(string usuarioId, BaneoModel ban)
        {
            var accion  = new AccionDeModeracion {
                Id = hashService.Random(),
                UsuarioId = usuarioId,
                Ban = ban,
                HiloId = ban.HiloId,
                ComentarioId = ban.ComentarioId,
                TipoElemento = ban.Tipo,
                Tipo = TipoAccion.UsuarioBaneado
            };
            context.AccionesDeModeracion.Add(accion);
            await context.SaveChangesAsync();
        }
        async public Task RegistrarBanRemovido(string usuarioId, BaneoModel ban)
        {
            var accion  = new AccionDeModeracion {
                Id = hashService.Random(),
                UsuarioId = usuarioId,
                Ban = ban,
                HiloId = ban.HiloId,
                ComentarioId = ban.ComentarioId,
                TipoElemento = ban.Tipo,
                Tipo = TipoAccion.UsuarioDesbaneado
            };
            context.AccionesDeModeracion.Add(accion);
            await context.SaveChangesAsync();
        }

        async public Task RegistrarCambioDeCategoria(string usuarioId, string hiloId, int categoriaOrigen, int categoriaDestino)
        {
            var accion  = new AccionDeModeracion {
                Id = hashService.Random(),
                UsuarioId = usuarioId,
                HiloId = hiloId,
                TipoElemento = TipoElemento.Hilo,
                Tipo = TipoAccion.CategoriaCambiada,
                Nota = $"{categorias.Value.FirstOrDefault(c => c.Id == categoriaOrigen).NombreCorto} a {categorias.Value.FirstOrDefault(c => c.Id == categoriaDestino).NombreCorto}"
            };
            context.AccionesDeModeracion.Add(accion);
            await context.SaveChangesAsync();
        }

        async public Task RegistrarEliminacion(string usuarioId, string hiloId, string comentarioId=null)
        {
            var accion  = new AccionDeModeracion {
                Id = hashService.Random(),
                UsuarioId = usuarioId,
                HiloId = hiloId,
                ComentarioId = comentarioId,
                TipoElemento = comentarioId == null? TipoElemento.Hilo : TipoElemento.Comentario,
                Tipo =  comentarioId == null? TipoAccion.HiloBorrado : TipoAccion.ComentarioBorrado,
            };
            context.AccionesDeModeracion.Add(accion);
            await context.SaveChangesAsync();
        }
        async public Task RegistrarRestauracion(string usuarioId, string hiloId, string comentarioId=null)
        {
            var accion  = new AccionDeModeracion {
                Id = hashService.Random(),
                UsuarioId = usuarioId,
                HiloId = hiloId,
                ComentarioId = comentarioId,
                TipoElemento = comentarioId == null? TipoElemento.Hilo : TipoElemento.Comentario,
                Tipo =  comentarioId == null? TipoAccion.HiloRestaurado : TipoAccion.ComentarioRestaurado,
            };
            context.AccionesDeModeracion.Add(accion);
            await context.SaveChangesAsync();
        }

        async public Task RegistrarDenunciaRechazada(string usuarioId, DenunciaModel denuncia)
        {
            var accion  = new AccionDeModeracion {
                Id = hashService.Random(),
                UsuarioId = usuarioId,
                HiloId = denuncia.HiloId,
                ComentarioId = denuncia.ComentarioId,
                DenunciaId = denuncia.Id,
                TipoElemento = denuncia.Tipo,
                Tipo =  TipoAccion.DenunciaRechazada,
            };
            context.AccionesDeModeracion.Add(accion);
            await context.SaveChangesAsync();
        }
    }
}

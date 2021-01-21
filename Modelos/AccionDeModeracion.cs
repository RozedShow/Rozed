using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Modelos
{

    public class AccionDeModeracion:BaseModel
    {
        public string UsuarioId { get; set; }

        // [Required]
        public TipoElemento TipoElemento { get; set; }

        public string HiloId { get; set; }
        public string ComentarioId { get; set; }
        public string DenunciaId { get; set; }
        public string Nota { get; set; } = "";

        public HiloModel Hilo { get; set; }
        public ComentarioModel Comentario { get; set; }
        public UsuarioModel Usuario { get; set; }
        public BaneoModel Ban { get; set; }
        public TipoAccion Tipo { get; set; }
        public DenunciaModel Denuncia { get; set; }
    }

    public enum TipoAccion
    {
        ComentarioBorrado,
        HiloBorrado,
        CategoriaCambiada,
        DenunciaRechazada,
        UsuarioBaneado,
        UsuarioDesbaneado,
        ComentarioRestaurado,
        HiloRestaurado,
    }

}

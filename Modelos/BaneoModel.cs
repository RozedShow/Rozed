using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Modelos
{
    public class BaneoModel: BaseModel
    {
        public string UsuarioId { get; set; }

        public string ModId { get; set; }
        [Required]
        public DateTimeOffset Expiracion { get; set; }
        public TimeSpan Duracion => Expiracion - Creacion;

        [Required]
        public TipoElemento Tipo { get; set; }

        public string HiloId { get; set; }
        public string ComentarioId { get; set; }

        [Required]
        public MotivoDenuncia Motivo { get; set; }
        public string Aclaracion { get; set; } = "";

        public bool Visto { get; set; }

        public HiloModel Hilo { get; set; }
        public ComentarioModel Comentario { get; set; }

        [JsonIgnore]
        public string Ip { get; set; }

        public UsuarioModel Usuario { get; set; }
    }
    
}
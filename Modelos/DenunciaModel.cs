using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Modelos
{
    public class DenunciaModel
    {   
        public string Id { get; set; }
        public DateTimeOffset Creacion { get; set; } = DateTimeOffset.Now;
        public string UsuarioId { get; set; } = "Anonimo";
        [Required]
        public TipoElemento Tipo { get; set; }
        [Required]
        public string HiloId { get; set; }
        public string ComentarioId { get; set; }
        [Required]
        public MotivoDenuncia Motivo { get; set; }
        public string Aclaracion { get; set; } = "";

        public EstadoDenuncia  Estado { get; set; } = EstadoDenuncia.NoRevisada;

        public HiloModel Hilo { get; set; }
        public ComentarioModel Comentario { get; set; }
        public UsuarioModel Usuario { get; set; }
    }

    public enum MotivoDenuncia
    {
        CategoriaIncorrecta,
        Spam,
        Doxxeo,
        CoentenidoIlegal,
        Gore,
        MaltratoAnimal,
    }
    public enum TipoElemento
    {
        Hilo,
        Comentario
    }
    public enum EstadoDenuncia
    {
        Aceptada,
        Rechazada,
        NoRevisada
    }
}

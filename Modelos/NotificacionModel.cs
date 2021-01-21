using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Modelos
{
    public class NotificacionModel
    {
        public NotificacionModel()
        {
        }

        public NotificacionModel(string id, string usuarioId, string hiloId, string comentarioId, NotificacionType tipo = NotificacionType.Comentario)
        {
            UsuarioId = usuarioId;
            HiloId = hiloId;
            ComentarioId = comentarioId;
            Tipo = tipo;
            Actualizacion = DateTimeOffset.Now;
            Id = id;
        }
        public NotificacionModel(string id, string usuarioId, ComentarioModel comentario, NotificacionType tipo = NotificacionType.Comentario):
            this(id, usuarioId, comentario.HiloId, comentario.Id, tipo){}

        [Required]
        public string Id { get; set; }
        public string UsuarioId { get; set; }
        public string HiloId { get; set; }
        public string ComentarioId { get; set; } //Si el tipo es comentario, indica la id del comentario respondido
        public NotificacionType Tipo { get; set; }
        public DateTimeOffset Actualizacion { get; set; }
        public int Conteo { get; set; } = 1;
        
        public ComentarioModel Comentario { get; set; }

        public HiloModel Hilo { get; set; }
    }

    public enum NotificacionType {
        Comentario,
        Respuesta
    }
}

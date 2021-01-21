using System.Collections.Generic;
using Modelos;

namespace Modelos
{
    public class NotificacionViewModel
    {
        public string Id { get; set; }
        public int Conteo { get; set; }
        public string HiloId { get; set; }
        public string HiloTitulo { get; set; }
        public string HiloImagen { get; set; }
        public string ComentarioId { get; set; }
        public NotificacionType  Tipo { get; set; }
    }
}
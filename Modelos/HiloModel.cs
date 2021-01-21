using System;
using System.Collections.Generic;

namespace Modelos
{
    public class HiloModel : CreacionUsuario
    {
        public string Titulo { get; set; }
        public string Contenido { get; set; }
        public int CategoriaId { get; set; }
        public DateTimeOffset Bump { get; set; }
        public List<ComentarioModel> Comentarios { get; set; }
        public HiloEstado Estado { get; set; }

        public string Flags { get; set; } = "";

        public UsuarioModel Usuario { get; set; }

        public Encuesta Encuesta { get; set; }

    }

    public enum HiloEstado {
        Normal,
        Archivado,
        Eliminado,
    }
}

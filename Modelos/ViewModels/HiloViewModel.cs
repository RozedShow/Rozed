using System;

namespace Modelos
{
    public class HiloViewModel 
    {
        public HiloViewModel(){}
        public HiloViewModel(HiloModel hilo)
        {
            this.Bump = hilo.Bump;
            this.CategoriaId = hilo.CategoriaId;
            this.Contenido = hilo.Contenido;
            this.Creacion = hilo.Creacion;
            this.Media = hilo.Media;
            this.Bump = hilo.Bump;
            this.Titulo = hilo.Titulo;
            this.Id = hilo.Id;
            this.Estado = hilo.Estado;
            this.Rango = hilo.Rango;
            this.Nombre = hilo.Nombre;
            this.Dados = hilo.Flags.Contains("d");
            this.Encuesta = hilo.Encuesta != null;
        }

        public int CantidadComentarios { get; set; }
        public bool Nuevo =>  Creacion.AddMinutes(20) > DateTimeOffset.Now;

        public string Titulo { get; set; }
        public string Id { get; set; }
        public int Sticky { get; set; }
        public DateTimeOffset Bump { get; set; }
        public int CategoriaId { get; set; }
        public string Contenido { get; set; }
        public DateTimeOffset Creacion { get; set; }
        public MediaModel Media { get; set; }
        public string Thumbnail { get; set; }
        public HiloEstado Estado { get; set; }
        public CreacionRango Rango { get; set; }
        public string Nombre { get; set; }
        public bool Dados { get; set; }
        public bool Encuesta { get; set; }
        public EncuestaViewModel EncuestaData { get; set; }
    }

    public class HiloViewModelMod : HiloViewModel
    {
        public string UsuarioId { get; set; }
        public string UserName { get; set; }
        public UsuarioModel Usuario { get; set; }
    }
}

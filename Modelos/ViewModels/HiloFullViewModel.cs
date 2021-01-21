using System.Collections.Generic;
using Modelos;

namespace Modelos
{
    public interface IHiloFullView
    {
        HiloViewModel Hilo { get; set; }
        List<ComentarioViewModel> Comentarios { get; set; }
        HiloAccionModel Acciones { get; set; }
    }

    public class HiloFullViewModel : IHiloFullView
    {
        public HiloViewModel Hilo { get; set; }
        public List<ComentarioViewModel> Comentarios { get; set; }
        public HiloAccionModel Acciones { get; set; }
        public CreacionRango Rango { get; set; }
        public string Nombre { get; set; }
    }
    public class HiloFullViewModelMod :HiloFullViewModel
    {
        public new List<ComentarioViewModelMod> Comentarios { get; set; }
        public UsuarioModel Usuario { get; set; }
       
    }
}
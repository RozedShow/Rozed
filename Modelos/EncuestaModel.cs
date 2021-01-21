using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Modelos
{
    public class Encuesta
    {
        public IList<OpcionEncuesta>  Opciones { get; set; } = new List<OpcionEncuesta>();
        public List<string> Ips { get; set; } = new List<string>();
        public List<string> Ids { get; set; } = new List<string>();
    }

    public class OpcionEncuesta
    {
        public string Nombre { get; set; } = "";
        public int Votos { get; set; }
    }

    public class EncuestaViewModel {
        public EncuestaViewModel(Encuesta encuesta, string usuarioId)
        {
            Opciones = encuesta.Opciones;
            HaVotado = encuesta.Ids.Any(id => id == usuarioId);
        }
        public IList<OpcionEncuesta> Opciones { get; set; }
        public bool HaVotado { get; set; } = false;
    }
}

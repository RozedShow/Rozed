using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using Newtonsoft.Json;

namespace Modelos
{
    public class CreacionUsuario : BaseModel
    {
        [Required]
        public string UsuarioId { get; set; }
        public MediaModel Media { get; set; }
        public string Nombre{ get; set; } = "";
        public CreacionRango Rango{ get; set; } = CreacionRango.Anon;
        public string MediaId { get; set; }
        [JsonIgnore]
        public string Ip { get; set; }
    }
    
    public enum CreacionRango
    {
        Anon,
        Auxiliar,
        Mod,
        Admin,
    }
}

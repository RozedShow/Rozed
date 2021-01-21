using System;
using Newtonsoft.Json;

namespace Modelos
{
    public class HiloAccionModel : BaseModel
    {
        public string UsuarioId { get; set; }
        public string HiloId { get; set; }
        public bool Seguido { get; set; }
        public bool Favorito { get; set; }
        public bool Hideado { get; set; }

        [JsonIgnore]
        public HiloModel Hilo { get; set; }
        [JsonIgnore]
        public UsuarioModel Usuario { get; set; }
    }
}

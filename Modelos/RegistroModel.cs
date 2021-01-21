using System.IO;

namespace Modelos
{
    public class Registro : BaseModel
    {
        public string Ip { get; set; }
        public string UsuarioId { get; set; }
        public string Codigo { get; set; }

        public UsuarioModel Usuario { get; set; }
    }

}
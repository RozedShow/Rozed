using System.IO;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Modelos
{
    public class MediaModel : BaseModel
    {
        public string Url { get; set; }
        public string VistaPrevia =>  Tipo != MediaType.Eliminado? $"/Media/P_{Hash}.jpg{(TgMedia !=null?$"?t={TgMedia.VistaPreviaTgId}":"")}": "";
        public string VistaPreviaCuadrado =>  Tipo != MediaType.Eliminado? $"/Media/PC_{Hash}.jpg{(TgMedia !=null?$"?t={TgMedia.VistaPreviaCuadradoTgId}":"")}": "";
        [JsonIgnore, System.Text.Json.Serialization.JsonIgnore]
        public string VistaPreviaLocal =>  Tipo != MediaType.Eliminado? $"/P_{Hash}.jpg": "";
        [JsonIgnore, System.Text.Json.Serialization.JsonIgnore]
        public string VistaPreviaCuadradoLocal =>  Tipo != MediaType.Eliminado? $"/PC_{Hash}.jpg": "";
        public string Hash { get; set; }
        public MediaType Tipo { get; set; } = MediaType.Imagen;
        public bool EsGif => Url.Contains(".gif");

        [JsonIgnore]
        public TgMedia TgMedia { get; set; }
    }
      public enum MediaType
    {
        Imagen,
        Video,
        Youtube,
        Eliminado,
    }
    [Owned]
    public class TgMedia 
    {
        public string UrlTgId { get; set; }
        public string VistaPreviaTgId { get; set; }
        public string VistaPreviaCuadradoTgId { get; set; }
    }
}
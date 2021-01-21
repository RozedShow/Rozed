using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using Ganss.XSS;

namespace Servicios 
{
    public class FormateadorService
    {
        private readonly HtmlEncoder encoder;
        private readonly static HtmlSanitizer sanitizer = 
            new HtmlSanitizer("a span".Split(" "), 
            allowedAttributes:"href target class r-id".Split(" "));
        public FormateadorService( HtmlEncoder encoder)
        {
            this.encoder = encoder;
        }

        public string Parsear(string contenido) {
            var tags = new List<string>();
            string ret =  string.Join("\n", contenido.Split("\n").Select(t => {
                t = encoder.Encode(t);
                var esLink = false;
                var esTag = false;
                //Links
                t = Regex.Replace(t, @"&gt;(http(s)?:\/\/.)?(www\.)?[-a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\b(\S*)", m => {
                    var link = m.Value.Replace("&gt;", "");
                    link = Regex.Replace(link, @"(http(s)?:\/\/)", "");
                    esLink = true;
                    return $@"<a href=""//{link}"" target=""_blank"">&gt;{link}</a>";
                });
                if(esLink) return t;
                //Respuestas
                t =  Regex.Replace(t, @"&gt;&gt;([A-Z0-9]{8})", m => {
                    if(tags.Contains(m.Value)) return "";
                    esTag = true;
                    tags.Add(m.Value);
                    var id = m.Groups[1].Value;
                    return $"<a href=\"#{id}\" class=\"restag\" r-id=\" {id}\">&gt;&gt;{id}</a>";
                });

                //Texto verde
                t = Regex.Replace(t.Replace("&#xA;", "\n"),@"&gt;(?!https?).+(?:$|\n)", m => {
                    if(esLink || esTag) return m.Value;
                    var text = m.Value;
                    return $@"<span class=""verde"">{text}</span>";
                });
                return t;
            }));
                return sanitizer.Sanitize(ret);
        }

        public string[] GetIdsTageadas(string contenido) {
            return Regex.Matches(contenido, @">>([A-Z0-9]{8})")
                .Select(m => {
                    return m.Groups[1].Value;
                }).ToArray();
        }
    }
}
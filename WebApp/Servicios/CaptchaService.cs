

using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Servicios
{
    public class CaptchaService
    {
        private readonly  string key;
        private readonly  string secret;
        private HttpClient client = new HttpClient();

        public CaptchaService(IConfiguration conf)
        {
            key = conf.GetValue<string>("HCaptcha:SiteKey");
            secret = conf.GetValue<string>("HCaptcha:Secret");
        }

        public async Task<bool> Verificar(string token)
        {
            var form = new Dictionary<string,string> {
                {"secret", secret},
                {"response", token},
            };
            var res = await client.PostAsync("https://hcaptcha.com/siteverify", new FormUrlEncodedContent(form));
            var respuesta = await  res.Content.ReadAsStringAsync();
            return respuesta.Contains("\"success\":true");
        }
    }
}
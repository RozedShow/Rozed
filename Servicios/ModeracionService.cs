using System;
using System.Threading.Tasks;
using Data;
using Modelos;

namespace Servicios
{
    public class ModeracionService
    {
        private readonly RChanContext context;

        public ModeracionService(
            RChanContext context
        )
        {
            this.context = context;
        }

        public async Task DenunciarHilo(DenunciaModel denuncia) {
            context.Denuncias.Add(denuncia);
            await context.SaveChangesAsync();
        }
    }
}

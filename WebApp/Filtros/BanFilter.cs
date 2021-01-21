using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Modelos;

namespace WebApp
{
    public class BanFilter : IAsyncActionFilter
    {
        private readonly SignInManager<UsuarioModel> sm;
        private readonly RChanContext dbContext;

        public BanFilter(SignInManager<UsuarioModel> sm, RChanContext dbContext)
        {
            this.sm = sm;
            this.dbContext = dbContext;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var ctx = context.HttpContext;
            if(Regex.IsMatch(ctx.Request.Path, @"^/Domado") || ctx.Request.Path.Value.Contains("omado"))  
            {
                await next();
                return;
            }

            if(ctx.User != null)
            {
                string ip = ctx.Connection.RemoteIpAddress.MapToIPv4().ToString();

                var banNoVisto = await dbContext.Bans
                    .OrderByDescending(b => b.Expiracion)
                    .Where(b => !b.Visto)
                    .FirstOrDefaultAsync(b => b.UsuarioId == ctx.User.GetId() || b.Ip == ip);

                var ahora = DateTime.Now;
                var banActivo = await dbContext.Bans
                    .OrderByDescending(b => b.Expiracion)
                    .Where(b => b.Visto)
                    .Where(b => b.Expiracion > ahora)
                    .FirstOrDefaultAsync(b => b.UsuarioId == ctx.User.GetId() || b.Ip == ip);

                if(banNoVisto != null)
                {
                    ctx.Response.Redirect("/Domado");
                    return;
                }

                if(banActivo != null &&  ctx.User.Identity.IsAuthenticated)
                {
                    await sm.SignOutAsync();
                    return;
                }
            }
            await next();
        }
    }

}
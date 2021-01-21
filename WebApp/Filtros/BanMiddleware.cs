using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Modelos;

namespace WebApp
{
    public class BanMiddleware
    {
        private readonly RequestDelegate next;

        public BanMiddleware(RequestDelegate next)
        {
            this.next = next;
 
        }

        public async Task Invoke(HttpContext ctx,  RChanContext context, 
            SignInManager<UsuarioModel> sm)
        {
            if(Regex.IsMatch(ctx.Request.Path, @"^/Domado") || ctx.Request.Path.Value.Contains("omado"))  
            {
                await next(ctx);
                return;
            }

            if(ctx.User != null)
            {
                string ip = ctx.Connection.RemoteIpAddress.MapToIPv4().ToString();

                var banNoVisto = await context.Bans
                    .OrderByDescending(b => b.Expiracion)
                    .Where(b => !b.Visto)
                    .FirstOrDefaultAsync(b => b.UsuarioId == ctx.User.GetId() || b.Ip == ip);

                var ahora = DateTime.Now;
                var banActivo = await context.Bans
                    .OrderByDescending(b => b.Expiracion)
                    .Where(b => b.Visto)
                    .Where(b => b.Expiracion > ahora)
                    .FirstOrDefaultAsync(b => b.UsuarioId == ctx.User.GetId() || b.Ip == ip);

                if(banNoVisto != null)
                {
                    ctx.Response.Redirect("/Domado");
                }

                if(banActivo != null)
                {
                    await sm.SignOutAsync();
                }
            }
            await next(ctx);
        }
    }
    public static class RozedMiddlwaresExtension
    {
        public static IApplicationBuilder UseBanMiddleware(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<BanMiddleware>();
        }
        public static IApplicationBuilder RestriccionDeAccesoMiddleware(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RestriccionDeAccesoMiddleware>();
        }
    }
    
}
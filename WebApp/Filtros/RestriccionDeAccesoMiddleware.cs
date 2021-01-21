using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Modelos;

namespace WebApp
{
    public class RestriccionDeAccesoMiddleware
    {
        private readonly RequestDelegate next;

        public RestriccionDeAccesoMiddleware(RequestDelegate next)
        {
            this.next = next;
 
        }

        public async Task Invoke(HttpContext ctx, IOptionsSnapshot<GeneralOptions> grlOpts)
        {
            string url = ctx.Request.Path.ToString().ToLower();
            if(url.Contains("hocamo") || url.Contains("hub") || url.Contains("inicio"))
            {
                await next(ctx);
                return;
            }
            bool puedoAccerdor = false;
            if (grlOpts.Value.RestriccionDeAcceso == RestriccionDeAcceso.Publico)
            {
                puedoAccerdor = true;
            }
            else if(grlOpts.Value.RestriccionDeAcceso == RestriccionDeAcceso.Registrados && ctx.User.Identity.IsAuthenticated)
            {
                puedoAccerdor = true;
            }
            else if(grlOpts.Value.RestriccionDeAcceso == RestriccionDeAcceso.Administradores && ctx.User != null && ctx.User.EsMod())
            {
                puedoAccerdor = true;
            }
            if(puedoAccerdor)
            {
                await next(ctx);
            }
            else
            {
                if(ctx.Request.Path.StartsWithSegments(new PathString("/Login")) 
                || ctx.Request.Path.StartsWithSegments(new PathString("/Registro"))
                || ctx.Request.Path.StartsWithSegments(new PathString("/Inicio")))
                {
                    await next(ctx);
                }
                else 
                {
                    ctx.Response.Redirect("/chocamo");
                    await next(ctx);
                }

            }

        }
    }    
}
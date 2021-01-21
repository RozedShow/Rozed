using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using WebApp;

public class RestriccionDeAccesoAction : IAsyncActionFilter
{
    private readonly IOptionsSnapshot<GeneralOptions> grlOpts;

    public RestriccionDeAccesoAction(IOptionsSnapshot<GeneralOptions> grlOpts)
    {
        this.grlOpts = grlOpts;
    }
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var ctx = context.HttpContext;
        string url = ctx.Request.Path.ToString().ToLower();
        if (url.Contains("hocamo") || url.Contains("hub") || url.Contains("omado") || url.Contains("ogin") || url.Contains("inicio"))
        {
            await next();
            return;
        }
        bool puedoAccerdor = false;
        if (grlOpts.Value.RestriccionDeAcceso == RestriccionDeAcceso.Publico)
        {
            puedoAccerdor = true;
        }
        else if (grlOpts.Value.RestriccionDeAcceso == RestriccionDeAcceso.Registrados && ctx.User.Identity.IsAuthenticated)
        {
            puedoAccerdor = true;
        }
        else if (grlOpts.Value.RestriccionDeAcceso == RestriccionDeAcceso.Administradores && ctx.User != null && ctx.User.EsMod())
        {
            puedoAccerdor = true;
        }
        if (puedoAccerdor)
        {
            await next();
        }
        else
        {
            if (ctx.Request.Path.StartsWithSegments(new PathString("/Login")) || ctx.Request.Path.StartsWithSegments(new PathString("/Registro")))
            {
                await next();
            }
            else
            {
                ctx.Response.Redirect("/chocamo");
                return;
            }

        }

    }
}
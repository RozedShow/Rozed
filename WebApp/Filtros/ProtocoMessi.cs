using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using WebApp;

public class ProtocoloMessi : IAsyncActionFilter
{
    private readonly IOptionsSnapshot<GeneralOptions> grlOpts;

    public ProtocoloMessi(IOptionsSnapshot<GeneralOptions> grlOpts)
    {
        this.grlOpts = grlOpts;
    }
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        // Si el protocolo messi no esta activado no hago nada
        if(!grlOpts.Value.ModoMessi) 
        {
            await next();
            return;
        }

        var ctx = context.HttpContext;
        string url = ctx.Request.Path.ToString().ToLower();
        if (url.Contains("hocamo") || url.Contains("hub") || url.Contains("omado") || url.Contains("ogin") || url.Contains("inicio"))
        {
            await next();
            return;
        }

        // Si el protocolo messi esta activado solo dejo pasar las peticiones get y la de los mods
        if(context.HttpContext.Request.Method != HttpMethods.Get && !context.HttpContext.User.EsMod())
        {
            context.Result = new BadRequestResult();
            return;
        }
        await next();
    }
}
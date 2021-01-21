
using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using WebApp;

public class AntiFloodService
{
    static private readonly Dictionary<string, DateTimeOffset> ultimosComentarios =
        new Dictionary<string, DateTimeOffset>();
    static private readonly Dictionary<string, DateTimeOffset> ultimosHilos =
         new Dictionary<string, DateTimeOffset>();

    private readonly IOptionsSnapshot<GeneralOptions> options;

    public AntiFloodService(
        IOptionsSnapshot<GeneralOptions> options
    )
    {
        this.options = options;
    }

    public TimeSpan SegundosParaComentar(ClaimsPrincipal usuario)
    {
        return SegundosPara(ultimosComentarios, options.Value.TiempoEntreComentarios, usuario);
    }
    public TimeSpan SegundosParaHilo(ClaimsPrincipal usuario)
    {
        return SegundosPara(ultimosHilos, options.Value.TiempoEntreHilos, usuario);
    }

    public void ResetearSegundosParaHilo(string usuarioId)
    {
        ultimosHilos[usuarioId] = new DateTimeOffset();
    }
    public void ResetearSegundosComentario(string usuarioId)
    {
        ultimosComentarios[usuarioId] = new DateTimeOffset();
    }

    private TimeSpan SegundosPara(Dictionary<string, DateTimeOffset> dic, int tiempo, ClaimsPrincipal usuario)
    {
        if (usuario.EsMod()) return TimeSpan.FromSeconds(0);
        var horaUltimoComentario = dic.GetValueOrDefault(usuario.GetId());

        if (horaUltimoComentario == new DateTimeOffset()) return TimeSpan.FromSeconds(0);

        if (horaUltimoComentario.AddSeconds(tiempo) < DateTimeOffset.Now) return TimeSpan.FromSeconds(0);

        return TimeSpan.FromTicks((horaUltimoComentario.AddSeconds(tiempo) - DateTimeOffset.Now).Ticks);
    }

    public void HaComentado(string usuarioId)
    {
        ultimosComentarios[usuarioId] = DateTimeOffset.Now;
    }
    public void HaCreadoHilo(string usuarioId)
    {
        ultimosHilos[usuarioId] = DateTimeOffset.Now;
    }
}

public class AntiFloodFilter : ActionFilterAttribute
{
    private readonly AntiFloodService antiFlood;

    public AntiFloodFilter(AntiFloodService antiFlood)
    {
        this.antiFlood = antiFlood;
    }
    override public void OnActionExecuting(ActionExecutingContext context)
    {
        var controller = context.Controller as Controller;
        var user = context.HttpContext.User;
        if (antiFlood.SegundosParaComentar(context.HttpContext.User) != new TimeSpan(0))
        {
            
            context.ModelState.AddModelError("Para para", $"faltan {antiFlood.SegundosParaComentar(user).Seconds} segundos para que puedas comentar");
            context.Result =  controller.BadRequest(context.ModelState);
        }
        else
        {
            antiFlood.HaComentado(user.GetId());
        }
        base.OnActionExecuting(context);
    }

    // public void OnActionExecuted(ActionExecutedContext context)
    // {
    //     if(context.HttpContext.Response.StatusCode == 200)
    //     {

    //     }
    // }
}
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Servicios;

public class TelegramMediaHostingMiddleare
{
    private readonly RequestDelegate next;

    public TelegramMediaHostingMiddleare(RequestDelegate next) { this.next = next; }

    public async Task Invoke(HttpContext ctx, MediaTgService mediaTgService)
    {
        if (!ctx.Request.QueryString.Value.Contains("?t="))
        {
            await next.Invoke(ctx);
        }
        else
        {
            var archivoId = ctx.Request.Query["t"];

            Telegram.Bot.Types.File archivoInfo;
            try
            {
                archivoInfo = await mediaTgService.GetInfoArchivoTg(archivoId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                await next.Invoke(ctx);
                return;
            }

            string ext = ctx.Request.Path.Value.Split(".").Last();
            ctx.Response.Headers["content-type"] = $"{("mp4 webm".Split(" ").Contains(ext) ? "video" : "image")}/{ext}";
            ctx.Response.Headers["cache-control"] = "public, max-age=31536000";
            ctx.Response.Headers["content-length"] = archivoInfo.FileSize.ToString();

            // if (ext == "mp4" || ext == "webm")
            // {
            //     ctx.Response.StatusCode = 206;
            //     ctx.Response.Headers["content-range"] = $"bytes 0-{archivoInfo.FileSize - 1}/{archivoInfo.FileSize}";
            // }

            await mediaTgService.DescargarArchivoTg(archivoId, ctx.Response.Body);
        }
    }
}
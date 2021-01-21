using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Data;
using Servicios;
using System.IO;
using Microsoft.Extensions.FileProviders;
using Modelos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using System.Security.Claims;
using Westwind.AspNetCore.LiveReload;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace WebApp
{
    public class Startup
    {
        private readonly IWebHostEnvironment env;
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            this.env = env;
            Configuration = configuration;
        }

        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;

        }
                public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<CaptchaService>();
            services.AddScoped<AntiFloodService>();
            services.Configure<GeneralOptions>(Configuration.GetSection("General"));
            services.Configure<List<Categoria>>(Configuration.GetSection("Categorias"));
            // services.AddLiveReload(config =>
            // {
            //     // config.FolderToMonitor = env.ContentRootPath + "\\Views";
            // });
            services.AddSignalR();
            services.AddRazorPages()
                .AddRazorRuntimeCompilation();
            services.AddMvc(opts => {
                opts.Filters.Add<RestriccionDeAccesoAction>();
                opts.Filters.Add<BanFilter>();
                opts.Filters.Add<ProtocoloMessi>();
            }).AddRazorRuntimeCompilation();

            services.AddDbContext<RChanContext>(options =>
                options.UseNpgsql(
                    Configuration.GetConnectionString("DefaultConnection")));

            // services.AddDefaultIdentity<UsuarioModel>(options => options.SignIn.RequireConfirmedAccount = false)
            //     .AddEntityFrameworkStores<RChanContext>();

            services.AddDefaultIdentity<UsuarioModel>(opt =>
            {
                opt.Password.RequireDigit = false;
                opt.Password.RequiredUniqueChars = 0;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;
            })
                .AddSignInManager()
                .AddEntityFrameworkStores<RChanContext>();

            services.ConfigureApplicationCookie(opt => {
                opt.LoginPath = "/Login";
                opt.Events.OnRedirectToLogin = async (c) => {
                    c.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await c.Response.WriteAsync("{\"redirect\":\"/Inicio\"}");
                };
            });

            services.AddScoped<IAuthorizationHandler, AuxiliarAuthorizationHandler>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("esAdmin", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("Role", "admin");
                });
                options.AddPolicy("esMod", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("Role", "admin mod".Split(" "));
                });
                options.AddPolicy("esAuxiliar", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.AddRequirements(new AuxiliarAuthorizationRequirement());
                    policy.RequireClaim("Role", "admin mod auxiliar".Split(" "));
                });
            });
            services.AddRazorPages();
            services.AddMvc(options => {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            })
                .AddNewtonsoftJson(options =>
                    {
                        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                        if(env.IsDevelopment())
                        {
                            options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
                        }
                    }
                );

            services.AddScoped<IHiloService, HiloService>();
            services.AddScoped<NotificacioensService>();
            services.AddScoped<IComentarioService, ComentarioService>();
            // services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<HashService>();
            services.AddScoped<AccionesDeModeracionService>();

            if(Configuration.GetValue<bool>("Telegram:UsarTelegram"))
            {
                services.AddScoped<IMediaService, MediaTgService>(s =>
                {
                    var env = s.GetService<IWebHostEnvironment>();
                    var logger = s.GetService<ILogger<MediaTgService>>();
                    return new MediaTgService(Path.Combine(env.ContentRootPath, "Almacenamiento"), 
                    s.GetService<RChanContext>(), env, logger, s.GetService<IConfiguration>());
                });

                services.AddScoped(s =>
                {
                    var env = s.GetService<IWebHostEnvironment>();
                    var logger = s.GetService<ILogger<MediaTgService>>();
                    return new MediaTgService(Path.Combine(env.ContentRootPath, "Almacenamiento"), s.GetService<RChanContext>(), env, logger, s.GetService<IConfiguration>());
                });
            }
            else 
            {
                services.AddScoped<IMediaService>(s =>
                {
                    var env = s.GetService<IWebHostEnvironment>();
                    var logger = s.GetService<ILogger<MediaTgService>>();
                    return new MediaService(Path.Combine(env.ContentRootPath, "Almacenamiento"), 
                    s.GetService<RChanContext>(), env, logger);
                });
            }
            
            services.AddSingleton<FormateadorService>();

            // Estadisticas
            services.AddSingleton((sp)  => {
                string ubicacionDeArchivo = Path.Join(sp.GetService<IWebHostEnvironment>().ContentRootPath, "estadisticas.json");
                var estadisticas = new EstadisticasService(ubicacionDeArchivo, sp.GetService<IHubContext<RChanHub>>());
                return estadisticas;

            });

            services.AddAntiforgery(options =>
            {
                // Set Cookie properties using CookieBuilder properties†.
                // options.HeaderName = "X-CSRF-TOKEN-ROZED";
                options.SuppressXFrameOptionsHeader = false;
            });
            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddHostedService<RChanBackgroundService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IConfiguration conf)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
                context.Response.Headers.Add("X-Xss-Protection", "1; mode=block");
                context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                context.Response.Headers.Add("Referrer-Policy", "no-referrer");
                context.Response.Headers.Add("X-Permitted-Cross-Domain-Policies", "none");
                await next();
            });

            if (env.IsDevelopment())
            {
                // app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                // app.UseLiveReload();

            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                // app.UseHsts();
            }

            // app.UseHttpsRedirection();

            void ConfigurarCache(StaticFileResponseContext ctx)
            {
                if (!env.IsDevelopment())
                {
                    ctx.Context.Response.Headers.Append(
                        "Cache-Control", $"public, max-age={31536000}");
                }
            }

            app.UseStaticFiles(
                new StaticFileOptions { OnPrepareResponse = ConfigurarCache }
            );

            app.UseStaticFiles(new StaticFileOptions
            {
                // RequestPath = "/Media",
                RequestPath = "/Media",
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "Almacenamiento")
                ),
                OnPrepareResponse = ConfigurarCache
            });
            app.Map("/Media", (app) => {
                if(conf.GetValue<bool>("Telegram:UsarTelegram"))
                {
                    app.UseMiddleware<TelegramMediaHostingMiddleare>();
                }
            });

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Hilo}/{action=Index}/{id?}"
                );
                endpoints.MapHub<RChanHub>("/hub");
            });
        }
    }

    public static class Extensiones
    {
        public static bool EsAdmin(this ClaimsPrincipal user)
        {
            return user.HasClaim("Role", "admin");
        }
        public static bool EsMod(this ClaimsPrincipal user)
        {
            return user.HasClaim("Role", "mod")
                || user.HasClaim("Role", "admin");
        }
        public static bool EsAuxiliar(this ClaimsPrincipal user, bool modoSerenito=false)
        {
            if(!modoSerenito && user.HasClaim("Role", "auxiliar")) return false;
            return user.HasClaim("Role", "mod")
                || user.HasClaim("Role", "admin")
                || user.HasClaim("Role", "auxiliar");
        }
        public static string GetId(this ClaimsPrincipal user)
        {
            return user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? null;
        }

    }
}

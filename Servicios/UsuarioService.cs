using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Modelos;
using Data;
using SqlKata.Execution;
using SqlKata.Compilers;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace Servicios
{
    public interface IUsuarioService
    {
        Task<IdentityResult> GenerarUsuario(string nick, string contraseña);
        Task<(IdentityResult, string)> GenerarUsuarioAnonimo();
        Task<SignInResult> LoguearUsuario(string nick, string contraseña);
        Task<bool> LoguearUsuarioAnonimo(string id);
    }

    public class UsuarioService : ContextService, IUsuarioService
    {
        private readonly UserManager<UsuarioModel> userManager;
        private readonly SignInManager<UsuarioModel> signInManager;

        public UsuarioService(RChanContext context,
            HashService hashService,
            UserManager<UsuarioModel> userManager,
            SignInManager<UsuarioModel> signInManager)
        : base(context, hashService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public async Task<IdentityResult> GenerarUsuario(string nick, string contraseña)
        {
            return await userManager.CreateAsync(new UsuarioModel {UserName = nick}, contraseña);   
        }

        public async Task<(IdentityResult, string)> GenerarUsuarioAnonimo()
        {
            string id = hashService.Random();
            var result = await userManager.CreateAsync(new UsuarioModel {UserName = id});   
            return (result, id);
        }

        public async Task<SignInResult> LoguearUsuario(string nick, string contraseña)
        {
            var user = await userManager.Users.FirstOrDefaultAsync(u => u.UserName == nick);
            var result = await signInManager.PasswordSignInAsync(user, contraseña, true, false);
            return result;
        }

        public async Task<bool> LoguearUsuarioAnonimo(string id)
        {
            var user = await userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
            await signInManager.SignInAsync(user, true);
            return true;
        }


    }
}
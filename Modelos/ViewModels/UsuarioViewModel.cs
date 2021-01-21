using System;
using System.Collections.Generic;
using Modelos;

namespace Modelos
{
    public class UsuarioViewModel
    {
        public UsuarioViewModel(UsuarioModel u)
        {
            this.Id = u.Id;
            this.UserName = u.UserName;
            this.Creacion = u.Creacion;

        }
        public string Id { get; set; }
        public string UserName { get; set; }
        public DateTimeOffset Creacion { get; set; }
    }
}
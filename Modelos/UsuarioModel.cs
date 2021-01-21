using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Modelos

{
    public class UsuarioModel : IdentityUser
    {
        [ForeignKey("UsuarioId")]
        public ICollection<NotificacionModel> Notificaciones { get; set; }
        public DateTimeOffset Creacion { get; set; } = DateTimeOffset.Now;

        [JsonIgnore, System.Text.Json.Serialization.JsonIgnore]
        public override bool TwoFactorEnabled { get; set; }
        [JsonIgnore, System.Text.Json.Serialization.JsonIgnore]
        public override bool PhoneNumberConfirmed { get; set; }
        [JsonIgnore, System.Text.Json.Serialization.JsonIgnore]
        public override string PhoneNumber { get; set; }
        [JsonIgnore, System.Text.Json.Serialization.JsonIgnore]
        public override string ConcurrencyStamp { get; set; }
        [JsonIgnore, System.Text.Json.Serialization.JsonIgnore]
        public override string SecurityStamp { get; set; }
        [JsonIgnore, System.Text.Json.Serialization.JsonIgnore]
        public override string PasswordHash { get; set; }
        [JsonIgnore, System.Text.Json.Serialization.JsonIgnore]
        public override bool EmailConfirmed { get; set; }
        [JsonIgnore, System.Text.Json.Serialization.JsonIgnore]
        public override string NormalizedEmail { get; set; }
        [JsonIgnore, System.Text.Json.Serialization.JsonIgnore]
        public override string Email { get; set; }
        [JsonIgnore, System.Text.Json.Serialization.JsonIgnore]
        public override bool LockoutEnabled { get; set; }
        [JsonIgnore, System.Text.Json.Serialization.JsonIgnore]
        public override int AccessFailedCount { get; set; }
        [JsonIgnore, System.Text.Json.Serialization.JsonIgnore]
        public string Ip { get; set; }
        [JsonIgnore, System.Text.Json.Serialization.JsonIgnore]
        public string Token { get; set; } = "";
    }
}

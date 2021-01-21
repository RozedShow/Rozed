using System;
using System.ComponentModel.DataAnnotations;

namespace Modelos
{
    public abstract class BaseModel
    {
        [Required]
        public string Id { get; set; }
        public DateTimeOffset Creacion { get; set; } = DateTimeOffset.Now;
    }
}

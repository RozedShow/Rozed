using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Modelos
{

    public class Sticky
    {
        [Required, Key]
        public string HiloId { get; set; }
        // 10 años desde que se crea
        public DateTimeOffset Expiracion { get; set; } = DateTimeOffset.Now + new TimeSpan(3600,0,0,0); 
        public bool Global { get; set; } = true;
        public int Importancia { get; set; } = 1;
    }
}

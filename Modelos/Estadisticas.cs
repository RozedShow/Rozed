using System;
using Newtonsoft.Json;

namespace Modelos
{
    public class Estadisticas
    {
        public int ComputadorasConectadas { get; set; }
        public int HilosCreados { get; set; }
        public int ComentariosCreados { get; set; }
        public float Estabilidad =>  70 + new Random().Next(1000) / 100;
    }
}

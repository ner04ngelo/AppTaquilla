using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppTaquilla.Models
{
    public class Salas
    {
        public int sala_id { get; set; }
        public string nombre { get; set; }

        public List<Fila> fila { get; set; }
    }
}
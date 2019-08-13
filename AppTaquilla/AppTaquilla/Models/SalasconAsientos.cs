using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppTaquilla.Models
{
    public class SalasconAsientos
    {

        
        public int sala_id { get; set; }
        public string nombre { get; set; }
        public int numTotal { get; set; }
        public int numOcupado { get; set; }
    }
}
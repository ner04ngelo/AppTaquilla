using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppTaquilla.Models
{
    public class Compra
    {
        public int compra_id { get; set; }
        public int cliente_id { get; set; }
        public DateTime fecha { get; set; }

        public virtual List<Ticket> ticket { get; set; }
    }
}
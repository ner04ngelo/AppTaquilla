using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;
using AppTaquilla.Models;
using Newtonsoft.Json;


namespace AppTaquilla.Controllers
{
    public class CompraController : Controller
    {
        protected static string URL = "https://apitickets.azurewebsites.net/";

        [HttpPost]
        public ActionResult ComprarTicket(string[] asiento, int sala_id)
        {
            if (asiento == null) return View("Home");
            
            List<Compra> filasInfo = new List<Compra>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(URL);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization
                         = new AuthenticationHeaderValue("Bearer", Session["Token"].ToString());

                Compra compra = new Compra();
                String test = Session["Token"].ToString();
                compra.cliente_id = 0;
                compra.fecha = DateTime.Now.Date;
                compra.ticket = new List<Ticket>();

                foreach (String tempAsiento in asiento)
                {
                    String[] temp = tempAsiento.Split('-');
                    Ticket ticket = new Ticket();
                    ticket.fila_id = int.Parse(temp[0]);
                    ticket.num_asiento = int.Parse(temp[1]);
                    compra.ticket.Add(ticket);
                }
                
                //HTTP POST
                var postTask = client.PostAsJsonAsync("api/Compra/", compra);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("VerSalas/" + sala_id, "Sala");
                }

                return RedirectToAction("VerSalas/" + sala_id, "Sala");
            }
        }

    }
}
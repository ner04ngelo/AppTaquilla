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
    public class TicketController : Controller
    {
        // GET: Ticket
        public async Task<ActionResult> Tickets()
        {
            string URL = "URL DEL API";
            List<Ticket> filasInfo = new List<Ticket>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(URL);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //Llamada a todlos los metodos

                HttpResponseMessage res = await client.GetAsync("api/~");

                if (res.IsSuccessStatusCode)
                {
                    var TerrResponse = res.Content.ReadAsStringAsync().Result;

                    filasInfo = JsonConvert.DeserializeObject<List<Ticket>>(TerrResponse);
                }

                return View(filasInfo);
            }
        }
    }
}
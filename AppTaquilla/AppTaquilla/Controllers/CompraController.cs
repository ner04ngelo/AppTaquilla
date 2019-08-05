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
        public async Task<ActionResult> ComprarTicket()
        {
            string URL = "URL DEL API";
            List<Compra> filasInfo = new List<Compra>();

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

                    filasInfo = JsonConvert.DeserializeObject<List<Compra>>(TerrResponse);
                }

                return View(filasInfo);
            }
        }

    }
}
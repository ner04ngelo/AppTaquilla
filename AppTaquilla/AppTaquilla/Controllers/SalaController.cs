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
    [HandleError]
    public class SalaController : Controller
    {
        // GET: Sala
        public async Task<ActionResult> Salas()
        {
            string URL = "URL DEL API";
            List<Salas> salasInfo = new List<Salas>();

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

                    salasInfo = JsonConvert.DeserializeObject<List<Salas>>(TerrResponse);
                }

                return View(salasInfo);
            }

        }
    }
}
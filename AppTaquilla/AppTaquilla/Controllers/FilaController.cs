using AppTaquilla.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;


namespace AppTaquilla.Controllers
{
    public class FilaController : Controller
    {
        protected static string URL = "http://localhost:44015/";
        protected static List<Salas> salasInfo = new List<Salas>();
        protected static List<SelectListItem> listado;

        // GET: Fila
        public async Task<ActionResult> Filas()
        {

            List<Fila> filasInfo = new List<Fila>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(URL);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //Llamada a todlos los metodos

                HttpResponseMessage res = await client.GetAsync("api/Fila");

                if (res.IsSuccessStatusCode)
                {
                    var TerrResponse = res.Content.ReadAsStringAsync().Result;

                    filasInfo = JsonConvert.DeserializeObject<List<Fila>>(TerrResponse);
                }

                return View(filasInfo);
            }

        }
        //POST: Fila
        public ActionResult Create()
        {         
            ViewBag.ListasSalas = ObtenerSalas();
            return View();
        }
        private void RecuperarSalas()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(URL);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //Llamada a todlos los metodos

                var res = client.GetAsync("api/Sala").Result;

                if (res.IsSuccessStatusCode)
                {
                    var TerrResponse = res.Content.ReadAsStringAsync().Result;

                    salasInfo = JsonConvert.DeserializeObject<List<Salas>>(TerrResponse);
                }


            }
        }
        private List<SelectListItem> ObtenerSalas()
        {
            RecuperarSalas();
            listado = new List<SelectListItem>();
            foreach (Salas item in salasInfo)
            {
               
                listado.Add(new SelectListItem()
                {
                    Text = item.nombre,
                    Value = item.sala_id.ToString()
                    
                });
            }
            return listado;
        }

        //PUT: Fila
        public ActionResult Edit(int id)
        {
            Fila fila = null;
            listado = new List<SelectListItem>();


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(URL);
                //HTTP GET
                var responseTask = client.GetAsync("api/Fila/" + id.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Fila>();
                    readTask.Wait();

                    fila = readTask.Result;
                }
            }
                   
            listado = ObtenerSalas();
            //EL FOREACH LO USO PARA DETERMINAR EN EL DROPDOWNLIST CUÁL SALA ESTARA POR DEFECTO CUANDO SE CARGÉ LA VISTA, EJEMPLO
            // SI LA FILA COINCIDE CON LA SALA 1, EL DROPDOWNLIST ESTARÁ UBICADO EN LA CASILLA QUE DICE SALA 1.
            foreach (var item in listado)
            {
                if ( int.Parse(item.Value) == fila.sala_id)
                {
                    item.Selected = true;                    
                }
                
            }


            ViewBag.ListasSalas = listado;
            return View(fila);
        }

     
        ///METODOS HTTP


       

        [HttpPost]
        public ActionResult Create(Fila fila)
        {
            using (var client = new HttpClient())
            {              

                client.BaseAddress = new Uri(URL);

                //HTTP POST
                var postTask = client.PostAsJsonAsync<Fila>("api/Fila", fila);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Filas");
                }
            }

            ModelState.AddModelError(string.Empty, "Error con el servidor.");

            return View(fila);
        }

        [HttpPost]
        public ActionResult Edit(Fila fila)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(URL);

                //HTTP POST
                var putTask = client.PutAsJsonAsync("api/Fila/" + fila.fila_id, fila);
                putTask.Wait();

                var result = putTask.Result;
                if (result.IsSuccessStatusCode)
                {

                    return RedirectToAction("Filas");
                }
            }
            return View(fila);
        }

        //DELETE: Fila
        public ActionResult Delete(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(URL);

                //HTTP DELETE
                var deleteTask = client.DeleteAsync("api/Fila/" + id.ToString());
                deleteTask.Wait();

                var result = deleteTask.Result;
                if (result.IsSuccessStatusCode)
                {

                    return RedirectToAction("Filas");
                }
            }

            return RedirectToAction("Fila");
        }
    }
}

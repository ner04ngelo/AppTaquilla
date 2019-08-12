using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;
using AppTaquilla.Models;
using Newtonsoft.Json;
using RestSharp;

namespace AppTaquilla.Controllers
{
    public class UsuarioController : Controller
    {
        protected static string URL = "https://apiptickets.azurewebsites.net/";

        // GET: Sala
        public async Task<ActionResult> Usuarios()
        {

            List<Usuario> salasInfo = new List<Usuario>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(URL);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //Llamada a todlos los metodos

                HttpResponseMessage res = await client.GetAsync("api/Usuario");

                if (res.IsSuccessStatusCode)
                {
                    var TerrResponse = res.Content.ReadAsStringAsync().Result;

                    salasInfo = JsonConvert.DeserializeObject<List<Usuario>>(TerrResponse);
                }

                return View(salasInfo);
            }

        }

        public ActionResult Login()
        {
            return View();
        }

       

        [HttpPost]
        public ActionResult Login(Usuario usuario)
        {

            var client = new RestClient(URL + "api/Login");
            var request = new RestRequest(Method.POST);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("Connection", "keep-alive");
            request.AddHeader("Content-Length", "71");
            request.AddHeader("Accept-Encoding", "gzip, deflate");
            request.AddHeader("Cookie", "ARRAffinity=43741afb0eacd9be64969ac3797f5e272f22a23cd8446e29f3ebb3a6bd9797ab");
            request.AddHeader("Host", "apiptickets.azurewebsites.net");
            request.AddHeader("Postman-Token", "72026db5-5e05-4179-b20e-7688f6b63571,f46175df-16d0-4351-8f50-2c9cd6fa8a0d");
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Accept", "*/*");
            request.AddHeader("User-Agent", "PostmanRuntime/7.15.2");
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("undefined", "{\r\n   \"email\":\""+ usuario.email+ "\",\r\n   \"contrasena\":\""+usuario.contrasena+"\"\r\n}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
           
            if (response.IsSuccessful)
            {
                string token = response.Content.Substring(20,408);
                Session["Email"] = usuario.email;              

                return RedirectToAction("Index", "Index");
            }
            else
            {
                ViewBag.error = "Usuario no válido";
            }                  

            return View(usuario);

        }


        public ActionResult Edit(int id)
        {
            Usuario usuario = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(URL);
                //HTTP GET
                var responseTask = client.GetAsync("api/Usuario/" + id.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Usuario>();
                    readTask.Wait();

                    usuario = readTask.Result;
                }
            }

            return View(usuario);
        }

        public ActionResult Create()
        {
            return View();
        }

      /*  public ActionResult Ver_Usuarios(int Id)
        {
            Usuario usuario = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(URL);
                //HTTP GET
                var responseTask = client.GetAsync("api/Usuario/" + Id.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Usuario>();
                    readTask.Wait();

                    usuario = readTask.Result;
                }
            }
            return View(usuario);
        }
        */

        /// METODOS HTTP
        [HttpPost]
        public ActionResult Edit(Usuario user)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(URL);

                //HTTP POST
                var putTask = client.PutAsJsonAsync("api/Usuario/" + user.usuario_id, user);
                putTask.Wait();

                var result = putTask.Result;
                if (result.IsSuccessStatusCode)
                {

                    return RedirectToAction("Usuarios");
                }
            }
            return View(user);
        }

        [HttpPost]
        public ActionResult Create(Usuario usuario)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(URL);

                //HTTP POST
                var postTask = client.PostAsJsonAsync<Usuario>("api/Usuario", usuario);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Usuarios");
                }
            }

            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

            return View(usuario);
        }

        public ActionResult Delete(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(URL);

                //HTTP DELETE
                var deleteTask = client.DeleteAsync("api/Usuario/" + id.ToString());
                deleteTask.Wait();

                var result = deleteTask.Result;
                if (result.IsSuccessStatusCode)
                {

                    return RedirectToAction("Usuarios");
                }
            }

            return RedirectToAction("Usuarios");
        }

    }
}
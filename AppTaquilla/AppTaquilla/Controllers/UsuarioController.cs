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
        protected static List<Usuario> users= new List<Usuario>();


        // GET: Usuario
     /*   public  void Usuarios(string token)
        {                       
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(URL);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Authorization
                         = new AuthenticationHeaderValue("Bearer", token);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //Llamada a todlos los metodos

                var res = client.GetAsync("api/Usuario").Result;

                if (res.IsSuccessStatusCode)
                {
                    var TerrResponse = res.Content.ReadAsStringAsync().Result;

                    users = JsonConvert.DeserializeObject<List<Usuario>>(TerrResponse);
                }
               
            }

        }*/

        public ActionResult Login()
        {
            return View();
        }

   

        [HttpPost]
        public ActionResult Login(Usuario usuario)
        {
            

            var client = new RestClient(URL + "api/Login");
            var request = new RestRequest(Method.POST);         
            request.AddHeader("Host", "apiptickets.azurewebsites.net");          
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("undefined", "{\r\n   \"email\":\""+ usuario.email+ "\",\r\n   \"contrasena\":\""+usuario.contrasena+"\"\r\n}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);           

            if (response.IsSuccessful)            {
                string token = response.Content.Substring(20, 421);

                //Agregamos el email y el token a un objeto sesión
                    Session["Usuario"] = usuario.email;
                    Session["Token"] = token;

                    token = null;

                    return RedirectToAction("Index", "Index");                           
               
            }
            else
            {
                ViewBag.error = "Usuario no válido";
            }                  

            return View(usuario);

        }


        public ActionResult CerrarSession()
        {
            // Abandon es una forma de destruir la sesión del usuario borrando en ella todo lo que contenga.
               Session.Abandon();


             return RedirectToAction("Index", "Index");
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
﻿using System;
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
        protected static string URL = "http://localhost:44015/";
     
        // GET: Sala
        public async Task<ActionResult> Salas()
        {
         
            List<Salas> salasInfo = new List<Salas>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(URL);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //Llamada a todlos los metodos

                HttpResponseMessage res = await client.GetAsync("api/Sala");

                if (res.IsSuccessStatusCode)
                {
                    var TerrResponse = res.Content.ReadAsStringAsync().Result;

                    salasInfo = JsonConvert.DeserializeObject<List<Salas>>(TerrResponse);
                }

                return View(salasInfo);
            }

        }

        public ActionResult Edit(int id)
        {
            Salas sala = null;           
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(URL);
                //HTTP GET
                var responseTask = client.GetAsync("api/Sala/" + id.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Salas>();
                    readTask.Wait();

                    sala = readTask.Result;
                }
            }

            return View(sala);                   
        }
        
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult VerSalas(int Id)
        {
            Salas sala = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(URL);
                //HTTP GET
                var responseTask = client.GetAsync("api/Sala/" + Id.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Salas>();
                    readTask.Wait();

                    sala = readTask.Result;
                }
            }            
            return View(sala);
        }


        /// METODOS HTTP
        [HttpPost]
        public ActionResult Edit(Salas salas)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(URL);
              
                //HTTP POST
                var putTask = client.PutAsJsonAsync("api/Sala/" + salas.sala_id, salas);
                putTask.Wait();

                var result = putTask.Result;
                if (result.IsSuccessStatusCode)
                {

                    return RedirectToAction("Salas");
                }
            }
            return View(salas);
        }
          
        [HttpPost]
        public ActionResult Create(Salas salas)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(URL);

                //HTTP POST
                var postTask = client.PostAsJsonAsync<Salas>("api/Sala", salas);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Salas");
                }
            }

            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

            return View(salas);
        }

        public ActionResult Delete(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(URL);

                //HTTP DELETE
                var deleteTask = client.DeleteAsync("api/Sala/" + id.ToString());
                deleteTask.Wait();

                var result = deleteTask.Result;
                if (result.IsSuccessStatusCode)
                {

                    return RedirectToAction("Salas");
                }
            }

            return RedirectToAction("Salas");
        }

    }
}
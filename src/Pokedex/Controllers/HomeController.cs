using Newtonsoft.Json;
using Pokedex.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Pokedex.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (var webClient = new WebClient())
            {
                //get a string representation of our JSON
                String rawJSON = webClient.DownloadString("https://raw.githubusercontent.com/Biuni/PokemonGO-Pokedex/master/pokedex.json");
                //convert the JSON string to a series of objects
                PokemonCollection pokemonCollection = JsonConvert.DeserializeObject<PokemonCollection>(rawJSON);
                return View(pokemonCollection);
            }
            
        }


       
    }
}
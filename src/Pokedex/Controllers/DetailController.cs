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
    public class DetailController : Controller
    {
        [HttpPost]
        public ActionResult PokemonDetail(FormCollection id)
        {

            using (var webClient = new WebClient())
            {
                //get a string representation of our JSON
                String rawJSON = webClient.DownloadString("https://raw.githubusercontent.com/Biuni/PokemonGO-Pokedex/master/pokedex.json");
                //convert the JSON string to a series of objects
                PokemonCollection pokemonCollection = JsonConvert.DeserializeObject<PokemonCollection>(rawJSON);
                ViewBag.Id = id["idPokemon"];
                ViewBag.Id = Convert.ToInt32(ViewBag.Id);
                ViewBag.Id = ViewBag.Id - 1;
                var currentPokemon = pokemonCollection.Pokemon[ViewBag.Id];

                //Logic for finding Evolution tree
                try
                {
                    //Pokemon has a previous evolution
                    var hasPrevious = currentPokemon.prev_evolution[0];
                    try
                    {
                        //It has a previous evolution and a next evolution, in total : 3 evolutions and the selected is the middle evolution
                        var hasNext = currentPokemon.next_evolution[0];
                       
                        ViewBag.First = hasPrevious;
                        ViewBag.Second = currentPokemon;
                        ViewBag.Third = hasNext;

                        //Status for rendering view
                        ViewBag.OnlyStatus = false;
                        ViewBag.TwoEvolutions = false;
                        ViewBag.ThreeEvolutions = true;        
                    }
                    catch
                    {
                        //It doesn't have a next evolution, therefor it is the last of two evolutions
                        ViewBag.First = hasPrevious;
                        ViewBag.Second = currentPokemon;


                        ViewBag.OnlyStatus = false;
                        ViewBag.TwoEvolutions = true;
                        ViewBag.ThreeEvolutions = false;
                    }
                }
                catch
                {
                    //First pokemon in the evolution tree
                    try
                    {
                        //And it has more evolutions
                        int count = 0;
                        for(var i = 0; i < currentPokemon.next_evolution.Length; i++)
                        {
                            count++;
                        }

                        if (count == 2)
                        {
                            //2 more evolutions to be spesific
                            ViewBag.First = currentPokemon;
                            ViewBag.Second = currentPokemon.next_evolution[0];
                            ViewBag.Third = currentPokemon.next_evolution[1];
                            ViewBag.OnlyStatus = false;
                            ViewBag.TwoEvolutions = false;
                            ViewBag.ThreeEvolutions = true;
                        }
                        else
                        {
                            //One more evolution to be specific
                            ViewBag.First = currentPokemon;
                            ViewBag.Second = currentPokemon.next_evolution[0];

                            ViewBag.OnlyStatus = false;
                            ViewBag.TwoEvolutions = true;
                            ViewBag.ThreeEvolutions = false;

                        }
                    }
                    catch
                    {
                        //Only one pokemon, no evolution
                        ViewBag.OnlyStatus = true;
                        ViewBag.TwoEvolutions = false;
                        ViewBag.ThreeEvolutions = false;
                    }
                }

                return View(pokemonCollection);
            }
            
        }
    }
}
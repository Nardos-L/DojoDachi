using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DojoDachi.Models;
using Microsoft.AspNetCore.Http;

namespace DojoDachi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            if(HttpContext.Session.GetInt32("Happiness") == null || HttpContext.Session.GetInt32("Fullness") == null || HttpContext.Session.GetInt32("Energy") == null || HttpContext.Session.GetInt32("Meals") == null)
            {
                HttpContext.Session.SetInt32("Happiness",20);
                HttpContext.Session.SetInt32("Fullness",20);
                HttpContext.Session.SetInt32("Energy",50);
                HttpContext.Session.SetInt32("Meals",3);
            }

            //HttpContext.Session.SetString("sessionMessage","");
             

            int happiness = HttpContext.Session.GetInt32("Happiness").Value;
            int fullness = HttpContext.Session.GetInt32("Fullness").Value;
            int energy = HttpContext.Session.GetInt32("Energy").Value;
            int meals = HttpContext.Session.GetInt32("Meals").Value;

            ViewBag.Happiness = happiness;
            ViewBag.Fullness = fullness;
            ViewBag.Energy = energy;
            ViewBag.Meals = meals;

            ViewBag.Message = HttpContext.Session.GetString("sessionMessage");

            return View("Index");
        }

        [HttpGet("/feed")]
        public IActionResult Feed()
        {
            int countMeals = HttpContext.Session.GetInt32("Meals").Value;
            Console.WriteLine($"meals before you feed : {countMeals}");
            if (countMeals != 0)
            {
            
            HttpContext.Session.SetInt32("Meals",countMeals-=1);

            int mealafterfeed = HttpContext.Session.GetInt32("Meals").Value;
            Console.WriteLine($"mealafterfeed : {mealafterfeed}");
            int countFullness = HttpContext.Session.GetInt32("Fullness").Value;
            Random rand = new Random();
            
            int randomFullness = rand.Next(5,10);
            int quarterChance = rand.Next(0,100); 
            if ( quarterChance > 25)
            {
                HttpContext.Session.SetInt32("Fullness",countFullness+=randomFullness);
            }
            
            }
            return RedirectToAction("Index");
        }

        [HttpGet("/play")]
        public IActionResult Play()
        {
            int sessionEnergy = HttpContext.Session.GetInt32("Energy").Value;
            HttpContext.Session.SetInt32("Energy",sessionEnergy-=5);
            
            int sessionHappiness = HttpContext.Session.GetInt32("Happiness").Value;

            Random rand = new Random();
            int randomHappiness = rand.Next(5,10);
            HttpContext.Session.SetInt32("Happiness",sessionHappiness+=randomHappiness);
            
            string sessionMessage = HttpContext.Session.GetString("sessionMessage");
            Console.WriteLine($"session message is: {sessionMessage}");
            sessionMessage =  "You played with your DochoDachi Happiness +10, Energy -5";
            HttpContext.Session.SetString("sessionMessage",sessionMessage);
            
            return RedirectToAction("Index");
        }

        [HttpGet("/work")]
        public IActionResult Work()
        {
            int sessionEnergy = HttpContext.Session.GetInt32("Energy").Value;
            HttpContext.Session.SetInt32("Energy",sessionEnergy-=5);
            

            int sessionMeal = HttpContext.Session.GetInt32("Meals").Value;
            Random rand = new Random();
            int randomMeals = rand.Next(1,3);
            HttpContext.Session.SetInt32("Meals",sessionMeal+=randomMeals);

            string sessionMessage = HttpContext.Session.GetString("sessionMessage");
            Console.WriteLine($"session message is: {sessionMessage}");
            sessionMessage = "You played with your DochoDachi random meals, Energy -5";
            HttpContext.Session.SetString("sessionMessage",sessionMessage);
            return RedirectToAction("Index");
        }

        [HttpGet("/sleep")]
        public IActionResult Sleep()
        {
            int sessionEnergy = HttpContext.Session.GetInt32("Energy").Value;
            HttpContext.Session.SetInt32("Energy",sessionEnergy+=15);
            

            int sessionFullness = HttpContext.Session.GetInt32("Fullness").Value;
            HttpContext.Session.SetInt32("Fullness",sessionFullness-=5);

            int sessionHappiness = HttpContext.Session.GetInt32("Happiness").Value;
            HttpContext.Session.SetInt32("Happiness",sessionHappiness-=5);
            return RedirectToAction("Index");
        }

        [HttpGet("/restart")]
        public IActionResult Restart()
        {
            int sessionEnergy = HttpContext.Session.GetInt32("Energy").Value;
            int sessionFullness = HttpContext.Session.GetInt32("Fullness").Value;
            int sessionHappiness = HttpContext.Session.GetInt32("Happiness").Value;

            if (sessionEnergy > 100 && sessionFullness > 100 && sessionHappiness > 100)
            {
                return RedirectToAction("/Won");
            }

            if (sessionEnergy == 0 && sessionFullness == 0 && sessionHappiness == 0)
            {
                return RedirectToAction("/Dead");
            }
            
            return RedirectToAction("Index");

        }

        [HttpGet("/won")]
        public IActionResult Won()
        {
            return RedirectToAction("Won");
        }
         

        [HttpGet("/dead")]
        public IActionResult Dead()
        {
            return RedirectToAction("Dead");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

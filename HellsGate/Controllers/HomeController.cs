using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HellsGate.Models;
using HellsGate.Lib;

namespace HellsGate.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
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
        public AuthType AccessType = AuthType.User;//TODO: add configuration reading

        [HttpGet]
        public IActionResult VerifyPlate()
        {
            using (var context = new Context())
            {
                string plate = Request.Form["PLTNMB"];
                AccessModel newAccess = new AccessModel
                {
                    AccessTime = DateTime.Now,
                    Plate = plate
                };
                if (context.Cars.Any(a => a.LicencePlate == plate))
                {
                    newAccess.CarEntered = context.Cars.First(a => a.LicencePlate == plate);
                    newAccess.GrantedAccess = Lib.AutorizationManager.IsAutorized(newAccess.CarEntered, AccessType);
                }
                else
                {
                    //TODO: add plate after confirm
                }
                context.Access.Add(newAccess);
                context.SaveChanges();
                StaticEventHandler.SendMail(new MailEventArgs(ResourceString.AccessCarMailSubject, ResourceString.AccessCarMailBody, DateTime.Now));
            }
            return View();
        }

    }
}

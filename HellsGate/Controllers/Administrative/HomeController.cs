using System;
using System.Diagnostics;
using System.Threading.Tasks;
using HellsGate.Lib;
using HellsGate.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HellsGate.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<PeopleAnagraphicModel> _userManager;
        private readonly SignInManager<PeopleAnagraphicModel> _signInManager;

        public HomeController(UserManager<PeopleAnagraphicModel> userManager, SignInManager<PeopleAnagraphicModel> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<IActionResult> IndexAsync()
        {
            PeopleAnagraphicModel user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            var model = new MainMenuViewModel
            {
                MainMenus = MenuManager.GetMenuForUser(user.Id)
            };

            return View(model);
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

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

    }
}

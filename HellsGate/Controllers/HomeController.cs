using System;
using System.Diagnostics;
using System.Threading.Tasks;
using HellsGate.Lib;
using HellsGate.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HellsGate.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly UserManager<PeopleAnagraphicModel> _userManager;

        public HomeController(UserManager<PeopleAnagraphicModel> userManager)
        {
            _userManager = userManager;
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
    }
}

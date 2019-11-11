using System;
using System.Reflection;
using System.Threading.Tasks;
using HellsGate.Lib;
using HellsGate.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HellsGate.Controllers.Administrative
{
    public class LoginController : Controller
    {
        private readonly LoginManager<PeopleAnagraphicModel> _signInManager;

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid && model != null)
            {
                try
                {
                    Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, true, false).ConfigureAwait(false);
                    if (result.Succeeded)
                    {
                        model.Errore = string.Empty;
                        return View("MenuView");
                    }
                    else
                    {
                        model.Errore = "Error during login";
                        return View(model);
                    }
                }
                catch (Exception ex)
                {
                    StaticEventHandler.Log(System.Diagnostics.TraceLevel.Error, "error during Login", MethodBase.GetCurrentMethod(), ex);
                    model.Errore = "Error during login";
                    return View(model);
                }

            }
            model.Errore = "Error during login";
            return View(model);
        }
        public IActionResult Index() => View();
    }
}
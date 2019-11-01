using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using HellsGate.Lib;
using HellsGate.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HellsGate.Controllers.Administrative
{
    public class LoginController : Controller
    {

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (await LoginManager.VerifyLogin(await LoginManager.GetUserByInput(model.Username).ConfigureAwait(false), model.password).ConfigureAwait(false))
                    {
                        model.Errore = string.Empty;
                        return View("MenuView");
                    }
                    model.Errore = "Error during login";
                    return View(model);
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

    }
}
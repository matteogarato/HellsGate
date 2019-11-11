using HellsGate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace HellsGate.Lib
{
    public class MenuManager
    {
        public static List<MainMenuModel> GetMenuForUser(string p_UserId)
        {
            try
            {
                using (var c = new Context())
                {
                    var user = c.Peoples.FirstOrDefault(p => p.Id == p_UserId);
                    return c.MainMenu.Where(c => c.AuthLevel == user.AutorizationLevel.AuthValue).ToList();
                }
            }
            catch (Exception ex)
            {
                StaticEventHandler.Log(System.Diagnostics.TraceLevel.Error, "error during GetMenuForUser", MethodBase.GetCurrentMethod(), ex);
                return null;
            }
        }
    }
}

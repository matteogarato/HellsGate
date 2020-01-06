using HellsGate.Lib;
using HellsGate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace HellsGate.Services
{
    public class MenuService : IMenuService
    {
        public List<MainMenuModel> GetMenuForUser(string p_UserName)
        {
            try
            {
                var user = new PeopleAnagraphicModel();
                using (var _ctx = new HellsGateContext())
                {
                    user = _ctx.Peoples.FirstOrDefault(p => p.UserName == p_UserName);
                }
                if (user != null && user.AutorizationLevel != null)
                {
                    return GetMenuForAuthorization(user.AutorizationLevel.AuthValue.ToString());
                }
                else return null;
            }
            catch (Exception ex)
            {
                StaticEventHandler.Log(System.Diagnostics.TraceLevel.Error, "error during GetMenuForUser", MethodBase.GetCurrentMethod(), ex);
                return null;
            }
        }

        public List<MainMenuModel> GetMenuForAuthorization(string p_Auth)
        {
            try
            {
                var authlevel = (AuthType)Convert.ToInt32(p_Auth);
                using (var _ctx = new HellsGateContext())
                {
                    return _ctx.MainMenu.Where(c => c.AuthLevel == authlevel).ToList();
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
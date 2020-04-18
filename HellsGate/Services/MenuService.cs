using HellsGate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HellsGate.Services
{
    public class MenuService : IMenuService
    {
        private readonly HellsGateContext _context;

        public MenuService(HellsGateContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public List<MainMenuModel> GetMenuForUser(string p_UserName)
        {
            try
            {
                if (string.IsNullOrEmpty(p_UserName))
                {
                    StaticEventHandler.Log(System.Diagnostics.TraceLevel.Error, "p_UserName is null or empty", MethodBase.GetCurrentMethod());
                    return null;
                }
                var user = new PeopleAnagraphicModel();
                user = _context.Peoples.FirstOrDefault(p => p.UserName == p_UserName);

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
                if (string.IsNullOrEmpty(p_Auth))
                {
                    StaticEventHandler.Log(System.Diagnostics.TraceLevel.Error, "p_Auth is null or empty", MethodBase.GetCurrentMethod());
                    return null;
                }
                var authlevel = (AuthType)Convert.ToInt32(p_Auth);

                return _context.MainMenu.Where(c => c.AuthLevel == authlevel).ToList();
            }
            catch (Exception ex)
            {
                StaticEventHandler.Log(System.Diagnostics.TraceLevel.Error, "error during GetMenuForUser", MethodBase.GetCurrentMethod(), ex);
                return null;
            }
        }

        public List<MainMenuModel> CreateMenuFromPages()
        {
            var menu = new List<MainMenuModel>();
            foreach (var page in GetTypesInNamespace(Assembly.GetExecutingAssembly(), "HellsGate.Pages"))
            {
                menu.Add(new MainMenuModel()
                {
                    Text = page.Name
                }); ;
            }
            return menu;
        }

        private Type[] GetTypesInNamespace(Assembly assembly, string nameSpace)
        {
            return
              assembly.GetTypes()
                      .Where(t => String.Equals(t.Namespace, nameSpace, StringComparison.Ordinal))
                      .ToArray();
        }
    }
}
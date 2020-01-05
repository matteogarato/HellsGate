using HellsGate.Lib;
using System.Collections.Generic;

namespace HellsGate.Models
{
    public class MainMenuViewModel
    {
        public List<MainMenuModel> MainMenus(string loggedUser)
        {
            MenuManager m = new MenuManager();
            return m.GetMenuForUser(loggedUser);
        }
    }
}
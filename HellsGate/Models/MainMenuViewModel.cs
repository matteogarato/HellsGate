using HellsGate.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HellsGate.Models
{
    public class MainMenuViewModel
    {
        private PeopleAnagraphicModel UserAutenticated;

        public List<MainMenuModel> MainMenus 
        {
            get {return MenuManager.GetMenuForUser(UserAutenticated.Id); }
            
        }

    }
}

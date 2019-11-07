using System.Collections.Generic;
using HellsGate.Lib;

namespace HellsGate.Models
{
    public class MainMenuViewModel
    {
        public List<MainMenuModel> MainMenus => MenuManager.GetMenuForUser(User.Identity.GetUserId());

    }
}

using HellsGate.Models;
using System.Collections.Generic;

namespace HellsGate.Services
{
    public interface IMenuService
    {
        public List<MainMenuModel> GetMenuForUser(string p_UserId);

        public List<MainMenuModel> GetMenuForAuthorization(string Auth);

        public List<MainMenuModel> CreateMenuFromPages();
    }
}
using HellsGate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HellsGate.Services
{
    public interface IMenuService
    {
        public List<MainMenuModel> GetMenuForUser(string p_UserId);

        public List<MainMenuModel> GetMenuForAuthorization(string Auth);
    }
}
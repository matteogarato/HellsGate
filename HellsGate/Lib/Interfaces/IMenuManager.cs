using HellsGate.Models;
using System.Collections.Generic;

namespace HellsGate.Lib.Interfaces
{
    public interface IMenuManager
    {
        public List<MainMenuModel> GetMenuForUser(string p_UserId);
    }
}
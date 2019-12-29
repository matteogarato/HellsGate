using HellsGate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HellsGate.Lib.Interfaces
{
    public interface IMenuManager
    {
        public List<MainMenuModel> GetMenuForUser(string p_UserId);
    }
}
using HellsGate.Models;
using System.Threading.Tasks;

namespace HellsGate.Lib.Interfaces
{
    public interface IAccessManager
    {
        public Task<bool> Access(AccessModel newAccess, AuthType AccessType);
    }
}
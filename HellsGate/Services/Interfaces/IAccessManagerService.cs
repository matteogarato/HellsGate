using HellsGate.Models;
using HellsGate.Models.DatabaseModel;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace HellsGate.Services.Interfaces
{
    public interface IAccessManagerService
    {
        public Task<bool> Access(AccessModel newAccess, WellknownAuthorizationLevel AccessType);

        public Task<SignInResult> ValidateLoginAsync(string username, string password, bool rememberMe, bool shouldLockout);

        public Task<string> GetUserByInputAsync(string UserInput);
    }
}
using HellsGate.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace HellsGate.Services.Interfaces
{
    public interface IAccessManagerService
    {
        public Task<bool> Access(AccessModel newAccess, AuthType AccessType);

        public Task<SignInResult> ValidateLoginAsync(string username, string password, bool rememberMe, bool shouldLockout);

        public Task<string> GetUserByInputAsync(string UserInput);
    }
}
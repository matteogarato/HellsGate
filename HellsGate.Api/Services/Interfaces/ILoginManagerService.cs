using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace HellsGate.Services.Interfaces
{
    public interface ILoginManagerService
    {
        public Task<Guid> GetUserByInputAsync(string UserInput);

        public Task<SignInResult> PasswordSignInAsync(string username, string password, bool rememberMe, bool shouldLockout);
    }
}
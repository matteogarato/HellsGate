using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace HellsGate.Services.Interfaces
{
    public interface ILoginManagerService
    {
        public Task<string> GetUserByInputAsync(string UserInput);

        public Task<SignInResult> PasswordSignInAsync(string username, string password, bool rememberMe, bool shouldLockout);
    }
}
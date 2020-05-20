using HellsGate.Models;
using HellsGate.Models.DatabaseModel;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace HellsGate.Services.Interfaces
{
    public interface IAccessManagerService
    {
        public Task<bool> Access(AccessModel newAccess);

        public Task<Guid> GetUserByInputAsync(string UserInput);

        public Task<SignInResult> ValidateLoginAsync(string username, string password, bool rememberMe, bool shouldLockout);
    }
}
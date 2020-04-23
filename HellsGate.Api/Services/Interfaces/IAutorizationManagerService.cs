using HellsGate.Models;
using System;
using System.Threading.Tasks;

namespace HellsGate.Services.Interfaces
{
    public interface IAutorizationManagerService
    {
        public Task<bool> IsCarAutorized(string p_CarModelId, WellknownAuthorizationLevel p_AuthNeeded);

        public Task<bool> IsAutorized(Guid p_PeopleModelId, WellknownAuthorizationLevel p_AuthNeeded);

        public Task CreateAdmin();

        public Task AutorizationModify(Guid p_PeopleModelIdRequest, Guid p_PeopleModelId, WellknownAuthorizationLevel p_newAuthorization);
    }
}
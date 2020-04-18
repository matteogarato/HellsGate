using HellsGate.Models;
using System.Threading.Tasks;

namespace HellsGate.Services.Interfaces
{
    public interface IAutorizationManagerService
    {
        public Task<bool> IsCarAutorized(string p_CarModelId, WellknownAuthorizationLevel p_AuthNeeded);

        public Task<bool> IsAutorized(string p_PeopleModelId, WellknownAuthorizationLevel p_AuthNeeded);

        public void CreateAdmin();

        public Task AutorizationModify(string p_PeopleModelIdRequest, string p_PeopleModelId, WellknownAuthorizationLevel p_newAuthorization);
    }
}
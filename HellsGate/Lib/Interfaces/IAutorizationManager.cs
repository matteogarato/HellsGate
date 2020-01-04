using HellsGate.Models;
using System.Threading.Tasks;

namespace HellsGate.Lib.Interfaces
{
    public interface IAutorizationManager
    {
        public Task<bool> IsCarAutorized(string p_CarModelId, AuthType p_AuthNeeded);

        public Task<bool> IsAutorized(string p_PeopleModelId, AuthType p_AuthNeeded);

        public void CreateAdmin();

        public Task AutorizationModify(string p_PeopleModelIdRequest, string p_PeopleModelId, AuthType p_newAuthorization);
    }
}
using HellsGate.Models.DatabaseModel;
using System.Threading.Tasks;

namespace HellsGate.Services.Interfaces
{
    public interface ISecurLibService
    {
        public Task<bool> CompareHash(string hash, string toVerify);

        public Task<string> EncriptLine(string p_textToEncrypt);

        public Task<string> EncryptEntityRelation(PeopleAnagraphicModel p_UserModel, AutorizationLevelModel p_AuthModel);
    }
}
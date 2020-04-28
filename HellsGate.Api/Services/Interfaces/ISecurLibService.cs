using HellsGate.Models.DatabaseModel;
using System.Threading.Tasks;

namespace HellsGate.Services.Interfaces
{
    public interface ISecurLibService
    {
        public string EncriptLine(string p_textToEncrypt);

        public bool CompareHash(string hash, string toVerify);

        public string EncryptEntityRelation(PeopleAnagraphicModel p_UserModel, AutorizationLevelModel p_AuthModel);
    }
}
using HellsGate.Models;
using HellsGate.Models.DatabaseModel;
using System;
using System.Threading.Tasks;

namespace HellsGate.Services.Interfaces
{
    public interface IAutorizationManagerService
    {
        public Task<bool> IsCarAutorized(string p_CarModelId, WellknownAuthorizationLevel p_AuthNeeded);

        public Task<bool> IsAutorized(Guid p_PeopleModelId, WellknownAuthorizationLevel p_AuthNeeded);

        public void CreateAdmin();

        public Guid CreateUser(PeopleAnagraphicModel p_user, AutorizationLevelModel p_autorizationLevel);

        public Task AutorizationModify(Guid p_PeopleModelIdRequest, Guid p_PeopleModelId, WellknownAuthorizationLevel p_newAuthorization);

        public Task<bool> ChangeCardNumber(Guid p_PeopleModelId, string p_CardNumber);

        public Task<bool> UpdateCardExpirationDate(string p_CardNumber, DateTime p_newExpirationDate);

        public Task<bool> CreateCard(CardModel p_model);
    }
}
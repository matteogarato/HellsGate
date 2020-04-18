using HellsGate.Models;
using HellsGate.Models.DatabaseModel;
using HellsGate.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace HellsGate.Services
{
    public class AutorizationManagerService : IAutorizationManagerService
    {
        private readonly ISecurLibService _securLib;
        private readonly HellsGateContext _context;

        public AutorizationManagerService(ISecurLibService securLib, HellsGateContext context)
        {
            _securLib = securLib ?? throw new ArgumentNullException(nameof(securLib));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// determine if the authorization of the car match the needed Authorization
        /// </summary>
        /// <param name="p_CarModelId">car anagraphic</param>
        /// <param name="p_AuthNeeded">needed Authorization</param>
        /// <returns></returns>
        public async Task<bool> IsCarAutorized(string p_CarModelId, WellknownAuthorizationLevel p_AuthNeeded)
        {
            try
            {
                CarAnagraphicModel car = await _context.Cars.FirstOrDefaultAsync(ca => ca.LicencePlate == p_CarModelId).ConfigureAwait(false);
                return await IsAutorized(car.Owner.Id, p_AuthNeeded).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                StaticEventHandler.Log(System.Diagnostics.TraceLevel.Error, "error during IsAutorized of CAR", MethodBase.GetCurrentMethod(), ex);
                return false;
            }
        }

        /// <summary>
        /// private member that verify the user an the autorization
        /// </summary>
        /// <param name="p_PeopleModelId"></param>
        /// <param name="p_AuthNeeded"></param>
        /// <returns></returns>
        public async Task<bool> IsAutorized(string p_PeopleModelId, WellknownAuthorizationLevel p_AuthNeeded)
        {
            try
            {
                if (await _context.Peoples.AnyAsync(p => p.Id == p_PeopleModelId).ConfigureAwait(false))
                {
                    PeopleAnagraphicModel usr = await _context.Peoples.FirstOrDefaultAsync(p => p.Id == p_PeopleModelId).ConfigureAwait(false);
                    if (usr.AutorizationLevel.AuthValue == p_AuthNeeded
                        && await AuthNotModified(usr.Id).ConfigureAwait(false)
                        && (usr.AutorizationLevel.ExpirationDate.Date >= DateTime.Today.Date || usr.AutorizationLevel.AuthValue == WellknownAuthorizationLevel.Root))
                    {
                        return true;
                    }
                    return false;
                }
                return false;
            }
            catch (Exception ex)
            {
                StaticEventHandler.Log(System.Diagnostics.TraceLevel.Error, "error during IsAutorized of people", MethodBase.GetCurrentMethod(), ex);
                return false;
            }
        }

        public async void CreateAdmin()
        {
            try
            {
                if (await _context.Peoples.AnyAsync(p => p.UserName == "Admin").ConfigureAwait(false))
                {
                    return;
                }
                else
                {
                    PeopleAnagraphicModel usr = new PeopleAnagraphicModel()
                    {
                        UserName = "Admin",
                        Email = "Admin@admin.com",
                        Password = Convert.ToBase64String(await _securLib.EncriptLineAsync("Admin").ConfigureAwait(false)),
                    };
                    var auth = new AutorizationLevelModel()
                    {
                        Id = 1,
                        AuthName = "ROOT",
                        AuthValue = WellknownAuthorizationLevel.Root
                    };

                    var safeAuth = new SafeAuthModel()
                    {
                        Id = 1,
                        AutId = 1,
                        UserId = usr.Id,
                        Control = await _securLib.EncryptLineToStringAsync(usr.Id + "1" + WellknownAuthorizationLevel.Root.ToString()).ConfigureAwait(false)
                    };
                    usr.AutorizationLevel = auth;
                    usr.SafeAuthModel = safeAuth;
                    await _context.Autorizations.AddAsync(auth);
                    await _context.SafeAuthModels.AddAsync(safeAuth);
                    await _context.Peoples.AddAsync(usr);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                StaticEventHandler.Log(System.Diagnostics.TraceLevel.Error, "error during IsAutorized od people", MethodBase.GetCurrentMethod(), ex);
            }
        }

        public async Task AutorizationModify(string p_PeopleModelIdRequest, string p_PeopleModelId, WellknownAuthorizationLevel p_newAuthorization)
        {
            try
            {
                PeopleAnagraphicModel Usr = await _context.Peoples.FirstOrDefaultAsync(p => p.Id == p_PeopleModelId).ConfigureAwait(false);
                //in case of lowering the authorization i can do only if i'm not the only one with it, and only if thiere is at least one root
                if (p_newAuthorization < Usr.AutorizationLevel.AuthValue && await _context.Peoples.AnyAsync(p => p.AutorizationLevel.AuthValue == Usr.AutorizationLevel.AuthValue && p.Id != Usr.Id).ConfigureAwait(false) &&
                    await _context.Peoples.AnyAsync(p => p.AutorizationLevel.AuthValue == WellknownAuthorizationLevel.Root && p.Id != Usr.Id).ConfigureAwait(false))
                {
                    Usr.AutorizationLevel.AuthValue = p_newAuthorization;
                    await ModifySafeAut(Usr.Id, Usr.AutorizationLevel.Id, Usr.AutorizationLevel.AuthValue).ConfigureAwait(false);
                }
                else if (p_newAuthorization > Usr.AutorizationLevel.AuthValue)
                {
                    PeopleAnagraphicModel UsrRequest = await _context.Peoples.FirstOrDefaultAsync(p => p.Id == p_PeopleModelIdRequest).ConfigureAwait(false);
                    if (Usr.AutorizationLevel.AuthValue == WellknownAuthorizationLevel.Root)
                    {
                        Usr.AutorizationLevel.AuthValue = p_newAuthorization;
                        await ModifySafeAut(Usr.Id, Usr.AutorizationLevel.Id, Usr.AutorizationLevel.AuthValue).ConfigureAwait(false);
                    }
                }
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                StaticEventHandler.Log(System.Diagnostics.TraceLevel.Error, "error during AutorizationModify", MethodBase.GetCurrentMethod(), ex);
            }
        }

        private async Task ModifySafeAut(string p_UserId, int p_newAuthorization, WellknownAuthorizationLevel p_NewWellknownAuthorizationLevel)
        {
            try
            {
                var authSaved = new SafeAuthModel();
                if (await _context.SafeAuthModels.AnyAsync(sa => sa.UserId == p_UserId).ConfigureAwait(false))
                {
                    authSaved = await _context.SafeAuthModels.FirstOrDefaultAsync(sa => sa.UserId == p_UserId).ConfigureAwait(false);
                }
                else
                {
                    _context.SafeAuthModels.Add(authSaved);
                }
                authSaved.AutId = p_newAuthorization;
                authSaved.Control = await _securLib.EncryptLineToStringAsync(p_UserId.ToString() + p_newAuthorization.ToString() + p_NewWellknownAuthorizationLevel.ToString()).ConfigureAwait(false);

                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                StaticEventHandler.Log(System.Diagnostics.TraceLevel.Error, "error during ModifySafeAut", MethodBase.GetCurrentMethod(), ex);
            }
        }

        private async Task<bool> AuthNotModified(string p_UserId)
        {
            try
            {
                if (await _context.Peoples.AnyAsync(p => p.Id == p_UserId).ConfigureAwait(false)
                    && await _context.SafeAuthModels.AnyAsync(sa => sa.UserId == p_UserId).ConfigureAwait(false))
                {
                    PeopleAnagraphicModel user = await _context.Peoples.FirstOrDefaultAsync(p => p.Id == p_UserId).ConfigureAwait(false);
                    SafeAuthModel authSaved = await _context.SafeAuthModels.FirstOrDefaultAsync(sa => sa.UserId == p_UserId).ConfigureAwait(false);
                    if (authSaved != null)
                    {
                        return
                            await _securLib.CompareHashAsync(
                                Convert.FromBase64String(authSaved.Control)
                                , await _securLib.EncriptLineAsync(user.Id.ToString() + user.AutorizationLevel.Id.ToString() + user.AutorizationLevel.AuthValue.ToString()).ConfigureAwait(false)).ConfigureAwait(false);
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                StaticEventHandler.Log(System.Diagnostics.TraceLevel.Error, "error during AuthNotModified", MethodBase.GetCurrentMethod(), ex);
                return false;
            }
        }
    }
}
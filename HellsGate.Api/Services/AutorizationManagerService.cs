using HellsGate.Models;
using HellsGate.Models.Context;
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
        private readonly IMenuService _menuService;
        private readonly HellsGateContext _context;

        public AutorizationManagerService(ISecurLibService securLib, IMenuService menuService, HellsGateContext context)
        {
            _securLib = securLib ?? throw new ArgumentNullException(nameof(securLib));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _menuService = menuService ?? throw new ArgumentNullException(nameof(menuService));
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
        public async Task<bool> IsAutorized(Guid p_PeopleModelId, WellknownAuthorizationLevel p_AuthNeeded)
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

        public async Task<Guid> CreateUser(PeopleAnagraphicModel user, AutorizationLevelModel autorizationLevel)
        {
            try
            {
                if (!user.Id.Equals(Guid.Empty) || !autorizationLevel.Id.Equals(Guid.Empty))
                {
                    return Guid.Empty;
                }
                user.Id = Guid.NewGuid();
                autorizationLevel.Id = Guid.NewGuid();
                _context.Peoples.Add(user);
                _context.Autorizations.Add(autorizationLevel);
                var safeAuth = new SafeAuthModel()
                {
                    Id = Guid.NewGuid(),
                    AutId = autorizationLevel.Id,
                    UserId = user.Id,
                    Control = await _securLib.EncryptLineToStringAsync($"{user.Id}{autorizationLevel.Id}{autorizationLevel.AuthValue}").ConfigureAwait(false)
                };
                _context.SafeAuthModels.Add(safeAuth);
                user.AutorizationLevel = autorizationLevel;
                user.SafeAuthModel = safeAuth;
                _context.SaveChanges();
                return user.Id;
            }
            catch (Exception ex)
            {
                StaticEventHandler.Log(System.Diagnostics.TraceLevel.Error, "error during IsAutorized od people", MethodBase.GetCurrentMethod(), ex);
                return Guid.Empty;
            }
        }

        public async Task CreateAdmin()
        {
            try
            {
                PeopleAnagraphicModel usr = new PeopleAnagraphicModel()
                {
                    UserName = "Admin",
                    Email = "Admin@admin.com",
                    Password = Convert.ToBase64String(await _securLib.EncriptLineAsync("Admin").ConfigureAwait(false)),
                };
                var auth = new AutorizationLevelModel()
                {
                    AuthName = "ROOT",
                    AuthValue = WellknownAuthorizationLevel.Root
                };
                await CreateUser(usr, auth);
                //var safeAuth = new SafeAuthModel()
                //{
                //    Id = 1,
                //    AutId = 1,
                //    UserId = usr.Id,
                //    Control = await _securLib.EncryptLineToStringAsync(usr.Id + "1" + WellknownAuthorizationLevel.Root.ToString()).ConfigureAwait(false)
                //};
                //usr.AutorizationLevel = auth;
                //usr.SafeAuthModel = safeAuth;
                //_context.Autorizations.Add(auth);
                //_context.SafeAuthModels.Add(safeAuth);
                //_context.Peoples.Add(usr);
                var menu = _menuService.CreateMenuFromPages();
                foreach (var vm in menu)
                {
                    vm.AuthLevel = HellsGate.Models.WellknownAuthorizationLevel.User;
                }
                _context.MainMenu.AddRange(menu);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                StaticEventHandler.Log(System.Diagnostics.TraceLevel.Error, "error during IsAutorized od people", MethodBase.GetCurrentMethod(), ex);
            }
        }

        public async Task AutorizationModify(Guid p_PeopleModelIdRequest, Guid p_PeopleModelId, WellknownAuthorizationLevel p_newAuthorization)
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

        private async Task ModifySafeAut(Guid p_UserId, Guid p_newAuthorization, WellknownAuthorizationLevel p_NewWellknownAuthorizationLevel)
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

        private async Task<bool> AuthNotModified(Guid p_UserId)
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
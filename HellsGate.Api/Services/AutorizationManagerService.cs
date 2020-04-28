﻿using HellsGate.Models;
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

        public AutorizationManagerService(ISecurLibService p_securLib, IMenuService p_menuService, HellsGateContext p_context)
        {
            _securLib = p_securLib ?? throw new ArgumentNullException(nameof(p_securLib));
            _context = p_context ?? throw new ArgumentNullException(nameof(p_context));
            _menuService = p_menuService ?? throw new ArgumentNullException(nameof(p_menuService));
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
                    if (usr.AutorizationLevel.AuthValue >= p_AuthNeeded
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

        /// <summary>
        /// </summary>
        /// <param name="p_user"></param>
        /// <param name="p_autorizationLevel"></param>
        /// <returns></returns>
        public Guid CreateUser(PeopleAnagraphicModel p_user, AutorizationLevelModel p_autorizationLevel)
        {
            try
            {
                if (!p_user.Id.Equals(Guid.Empty) || !p_autorizationLevel.Id.Equals(Guid.Empty))
                {
                    return Guid.Empty;
                }
                p_user.Id = Guid.NewGuid();
                p_autorizationLevel.Id = Guid.NewGuid();
                _context.Peoples.Add(p_user);
                _context.Autorizations.Add(p_autorizationLevel);
                p_user.AutorizationLevel = p_autorizationLevel;
                var safeAuth = new SafeAuthModel()
                {
                    Id = Guid.NewGuid(),
                    AutId = p_autorizationLevel.Id,
                    UserId = p_user.Id,
                    Control = _securLib.EncriptLine(_securLib.EncryptEntityRelation(p_user, p_autorizationLevel))
                };
                _context.SafeAuthModels.Add(safeAuth);
                p_user.AutorizationLevel = p_autorizationLevel;
                p_user.SafeAuthModel = safeAuth;
                _context.SaveChanges();
                return p_user.Id;
            }
            catch (Exception ex)
            {
                StaticEventHandler.Log(System.Diagnostics.TraceLevel.Error, "error during IsAutorized od people", MethodBase.GetCurrentMethod(), ex);
                return Guid.Empty;
            }
        }

        public async Task<bool> ChangeCardNumber(Guid p_PeopleModelId, string p_CardNumber)
        {
            if (p_PeopleModelId.Equals(Guid.Empty))
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(p_CardNumber))
            {
                return false;
            }
            try
            {
                if (!await _context.CardModels.AnyAsync(c => c.CardNumber == p_CardNumber).ConfigureAwait(false))
                {
                    return false;
                }
                if (await _context.Peoples.AnyAsync(p => p.Id == p_PeopleModelId).ConfigureAwait(false))
                {
                    return false;
                }
                var usr = await _context.Peoples.FirstAsync(p => p.Id == p_PeopleModelId).ConfigureAwait(false);
                usr.CardNumber = await _context.CardModels.FirstAsync(c => c.CardNumber == p_CardNumber).ConfigureAwait(false);
                await ModifySafeAut(p_PeopleModelId, usr.AutorizationLevel.Id, usr.AutorizationLevel.AuthValue);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                StaticEventHandler.Log(System.Diagnostics.TraceLevel.Error, "error during IsAutorized od people", MethodBase.GetCurrentMethod(), ex);
                return false;
            }
        }

        public async Task<bool> CreateCard(CardModel p_model)
        {
            if (p_model == null)
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(p_model.CardNumber))
            {
                return false;
            }
            try
            {
                if (await _context.CardModels.AnyAsync(c => c.CardNumber == p_model.CardNumber).ConfigureAwait(false))
                {
                    return false;
                }
                await _context.CardModels.AddAsync(p_model).ConfigureAwait(false);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                StaticEventHandler.Log(System.Diagnostics.TraceLevel.Error, "error during IsAutorized od people", MethodBase.GetCurrentMethod(), ex);
                return false;
            }
        }

        public async Task<bool> UpdateCardExpirationDate(string p_CardNumber, DateTime p_newExpirationDate)
        {
            if (string.IsNullOrWhiteSpace(p_CardNumber))
            {
                return false;
            }
            try
            {
                if (!await _context.CardModels.AnyAsync(c => c.CardNumber == p_CardNumber).ConfigureAwait(false))
                {
                    return false;
                }
                var card = await _context.CardModels.FirstAsync(c => c.CardNumber == p_CardNumber).ConfigureAwait(false);
                card.ExpirationDate = p_newExpirationDate;
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                StaticEventHandler.Log(System.Diagnostics.TraceLevel.Error, "error during IsAutorized od people", MethodBase.GetCurrentMethod(), ex);
                return false;
            }
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public void CreateAdmin()
        {
            try
            {
                var card = new CardModel()
                {
                    CardNumber = "123456",
                    ExpirationDate = DateTime.UtcNow.AddYears(1),
                    CreatedAt = DateTime.UtcNow
                };
                var usr = new PeopleAnagraphicModel()
                {
                    UserName = "Admin",
                    Email = "Admin@admin.com",
                    Password = _securLib.EncriptLine("Admin"),
                    CardNumber = card
                };
                var auth = new AutorizationLevelModel()
                {
                    AuthName = "ROOT",
                    AuthValue = WellknownAuthorizationLevel.Root,
                    ExpirationDate = DateTime.UtcNow.AddYears(1)
                };
                CreateUser(usr, auth);
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

        /// <summary>
        /// </summary>
        /// <param name="p_PeopleModelIdRequest"></param>
        /// <param name="p_PeopleModelId"></param>
        /// <param name="p_newAuthorization"></param>
        /// <returns></returns>
        public async Task AutorizationModify(Guid p_PeopleModelIdRequest, Guid p_PeopleModelId, WellknownAuthorizationLevel p_newAuthorization)
        {
            try
            {
                var Usr = await _context.Peoples.FirstOrDefaultAsync(p => p.Id == p_PeopleModelId).ConfigureAwait(false);
                //in case of lowering the authorization i can do only if i'm not the only one with it, and only if thiere is at least one root
                if (p_newAuthorization < Usr.AutorizationLevel.AuthValue && await _context.Peoples.AnyAsync(p => p.AutorizationLevel.AuthValue == Usr.AutorizationLevel.AuthValue && p.Id != Usr.Id).ConfigureAwait(false) &&
                    await _context.Peoples.AnyAsync(p => p.AutorizationLevel.AuthValue == WellknownAuthorizationLevel.Root && p.Id != Usr.Id).ConfigureAwait(false))
                {
                    Usr.AutorizationLevel.AuthValue = p_newAuthorization;
                    await ModifySafeAut(Usr.Id, Usr.AutorizationLevel.Id, Usr.AutorizationLevel.AuthValue).ConfigureAwait(false);
                }
                else if (p_newAuthorization > Usr.AutorizationLevel.AuthValue)
                {
                    var UsrRequest = await _context.Peoples.FirstOrDefaultAsync(p => p.Id == p_PeopleModelIdRequest).ConfigureAwait(false);
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

        /// <summary>
        /// </summary>
        /// <param name="p_UserId"></param>
        /// <param name="p_newAuthorization"></param>
        /// <param name="p_NewWellknownAuthorizationLevel"></param>
        /// <returns></returns>
        private async Task ModifySafeAut(Guid p_UserId, Guid p_newAuthorization, WellknownAuthorizationLevel p_NewWellknownAuthorizationLevel)
        {
            try
            {
                if (await _context.Peoples.AnyAsync(p => p.Id == p_UserId).ConfigureAwait(false))
                {
                    if (await AuthNotModified(p_UserId).ConfigureAwait(false))
                    {
                        return;
                    }
                    var user = await _context.Peoples.FirstAsync(p => p.Id == p_UserId).ConfigureAwait(false);
                    if (user.SafeAuthModel == null)
                    {
                        return;
                    }
                    user.SafeAuthModel.AutId = p_newAuthorization;
                    user.SafeAuthModel.Control = _securLib.EncriptLine(_securLib.EncryptEntityRelation(user, user.AutorizationLevel));
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                StaticEventHandler.Log(System.Diagnostics.TraceLevel.Error, "error during ModifySafeAut", MethodBase.GetCurrentMethod(), ex);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="p_UserId"></param>
        /// <returns></returns>
        private async Task<bool> AuthNotModified(Guid p_UserId)
        {
            try
            {
                if (await _context.Peoples.AnyAsync(p => p.Id == p_UserId).ConfigureAwait(false))
                {
                    var user = await _context.Peoples.Include(Auth => Auth.AutorizationLevel).Include(Card => Card.CardNumber).FirstAsync(p => p.Id == p_UserId).ConfigureAwait(false);
                    if (user.SafeAuthModel == null)
                    {
                        return false;
                    }
                    return _securLib.CompareHash(user.SafeAuthModel.Control, _securLib.EncryptEntityRelation(user, user.AutorizationLevel));
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
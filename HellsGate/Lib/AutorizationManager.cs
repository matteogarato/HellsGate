﻿using HellsGate.Lib.Interfaces;
using HellsGate.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace HellsGate.Lib
{
    public class AutorizationManager : IAutorizationManager
    {
        private readonly ISecurLib SecurLib;

        /// <summary>
        /// determine if the authorization of the car match the needed Authorization
        /// </summary>
        /// <param name="p_CarModelId">car anagraphic</param>
        /// <param name="p_AuthNeeded">needed Authorization</param>
        /// <returns></returns>
        public async Task<bool> IsCarAutorized(string p_CarModelId, AuthType p_AuthNeeded)
        {
            try
            {
                using (var c = new HellsGateContext())
                {
                    CarAnagraphicModel car = await c.Cars.FirstOrDefaultAsync(ca => ca.LicencePlate == p_CarModelId).ConfigureAwait(false);
                    return await IsAutorized(car.Owner.Id, p_AuthNeeded).ConfigureAwait(false);
                }
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
        public async Task<bool> IsAutorized(string p_PeopleModelId, AuthType p_AuthNeeded)
        {
            try
            {
                using (var c = new HellsGateContext())
                {
                    if (await c.Peoples.AnyAsync(p => p.Id == p_PeopleModelId).ConfigureAwait(false))
                    {
                        PeopleAnagraphicModel usr = await c.Peoples.FirstOrDefaultAsync(p => p.Id == p_PeopleModelId).ConfigureAwait(false);
                        if (usr.AutorizationLevel.AuthValue == p_AuthNeeded
                            && await AuthNotModified(usr.Id).ConfigureAwait(false)
                            && (usr.AutorizationLevel.ExpirationDate.Date >= DateTime.Today.Date || usr.AutorizationLevel.AuthValue == AuthType.Root))
                        {
                            return true;
                        }
                        return false;
                    }
                    return false;
                }
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
                using (var c = new HellsGateContext())
                {
                    if (await c.Peoples.AnyAsync(p => p.UserName == "Admin").ConfigureAwait(false))
                    {
                        return;
                    }
                    else
                    {
                        PeopleAnagraphicModel usr = new PeopleAnagraphicModel()
                        {
                            UserName = "Admin",
                            Email = "Admin@admin.com",
                            Password = Convert.ToBase64String(await SecurLib.EncriptLineAsync("Admin").ConfigureAwait(false)),
                        };
                        var auth = new AutorizationLevelModel()
                        {
                            Id = 1,
                            AuthName = "ROOT",
                            AuthValue = AuthType.Root
                        };

                        var safeAuth = new SafeAuthModel()
                        {
                            Id = 1,
                            AutId = 1,
                            UserId = usr.Id,
                            Control = await SecurLib.EncryptLineToStringAsync(usr.Id + "1" + AuthType.Root.ToString()).ConfigureAwait(false)
                        };
                        usr.AutorizationLevel = auth;
                        usr.SafeAuthModel = safeAuth;
                        await c.Autorizations.AddAsync(auth);
                        await c.SafeAuthModels.AddAsync(safeAuth);
                        await c.Peoples.AddAsync(usr);
                        await c.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                StaticEventHandler.Log(System.Diagnostics.TraceLevel.Error, "error during IsAutorized od people", MethodBase.GetCurrentMethod(), ex);
            }
        }

        public async Task AutorizationModify(string p_PeopleModelIdRequest, string p_PeopleModelId, AuthType p_newAuthorization)
        {
            try
            {
                using (var c = new HellsGateContext())
                {
                    PeopleAnagraphicModel Usr = await c.Peoples.FirstOrDefaultAsync(p => p.Id == p_PeopleModelId).ConfigureAwait(false);
                    //in case of lowering the authorization i can do only if i'm not the only one with it, and only if thiere is at least one root
                    if (p_newAuthorization < Usr.AutorizationLevel.AuthValue && await c.Peoples.AnyAsync(p => p.AutorizationLevel.AuthValue == Usr.AutorizationLevel.AuthValue && p.Id != Usr.Id).ConfigureAwait(false) &&
                        await c.Peoples.AnyAsync(p => p.AutorizationLevel.AuthValue == AuthType.Root && p.Id != Usr.Id).ConfigureAwait(false))
                    {
                        Usr.AutorizationLevel.AuthValue = p_newAuthorization;
                        await ModifySafeAut(Usr.Id, Usr.AutorizationLevel.Id, Usr.AutorizationLevel.AuthValue).ConfigureAwait(false);
                    }
                    else if (p_newAuthorization > Usr.AutorizationLevel.AuthValue)
                    {
                        PeopleAnagraphicModel UsrRequest = await c.Peoples.FirstOrDefaultAsync(p => p.Id == p_PeopleModelIdRequest).ConfigureAwait(false);
                        if (Usr.AutorizationLevel.AuthValue == AuthType.Root)
                        {
                            Usr.AutorizationLevel.AuthValue = p_newAuthorization;
                            await ModifySafeAut(Usr.Id, Usr.AutorizationLevel.Id, Usr.AutorizationLevel.AuthValue).ConfigureAwait(false);
                        }
                    }
                    await c.SaveChangesAsync().ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                StaticEventHandler.Log(System.Diagnostics.TraceLevel.Error, "error during AutorizationModify", MethodBase.GetCurrentMethod(), ex);
            }
        }

        private async Task ModifySafeAut(string p_UserId, int p_newAuthorization, AuthType p_NewAuthType)
        {
            try
            {
                using (var c = new HellsGateContext())
                {
                    var authSaved = new SafeAuthModel();
                    if (await c.SafeAuthModels.AnyAsync(sa => sa.UserId == p_UserId).ConfigureAwait(false))
                    {
                        authSaved = await c.SafeAuthModels.FirstOrDefaultAsync(sa => sa.UserId == p_UserId).ConfigureAwait(false);
                    }
                    else
                    {
                        c.SafeAuthModels.Add(authSaved);
                    }
                    authSaved.AutId = p_newAuthorization;
                    authSaved.Control = await SecurLib.EncryptLineToStringAsync(p_UserId.ToString() + p_newAuthorization.ToString() + p_NewAuthType.ToString()).ConfigureAwait(false);

                    await c.SaveChangesAsync().ConfigureAwait(false);
                }
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
                using (var c = new HellsGateContext())
                {
                    if (await c.Peoples.AnyAsync(p => p.Id == p_UserId).ConfigureAwait(false)
                        && await c.SafeAuthModels.AnyAsync(sa => sa.UserId == p_UserId).ConfigureAwait(false))
                    {
                        PeopleAnagraphicModel user = await c.Peoples.FirstOrDefaultAsync(p => p.Id == p_UserId).ConfigureAwait(false);
                        SafeAuthModel authSaved = await c.SafeAuthModels.FirstOrDefaultAsync(sa => sa.UserId == p_UserId).ConfigureAwait(false);
                        if (authSaved != null)
                        {
                            return
                                await SecurLib.CompareHashAsync(
                                    Convert.FromBase64String(authSaved.Control)
                                    , await SecurLib.EncriptLineAsync(user.Id.ToString() + user.AutorizationLevel.Id.ToString() + user.AutorizationLevel.AuthValue.ToString()).ConfigureAwait(false)).ConfigureAwait(false);
                        }
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                StaticEventHandler.Log(System.Diagnostics.TraceLevel.Error, "error during AuthNotModified", MethodBase.GetCurrentMethod(), ex);
                return false;
            }
        }
    }
}
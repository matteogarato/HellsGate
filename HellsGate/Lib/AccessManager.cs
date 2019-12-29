using HellsGate.Lib.Interfaces;
using HellsGate.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace HellsGate.Lib
{
    public class AccessManager : IAccessManager
    {
        private readonly IAutorizationManager auth;

        public async Task<bool> Access(AccessModel newAccess, AuthType AccessType)
        {
            newAccess.GrantedAccess = false;
            if (!string.IsNullOrEmpty(newAccess.PeopleEntered) || !string.IsNullOrEmpty(newAccess.Plate))
            {
                using (var context = new HellsGateContext())
                {
                    try
                    {
                        PeopleAnagraphicModel owner = new PeopleAnagraphicModel();
                        if (string.IsNullOrEmpty(newAccess.PeopleEntered) && !string.IsNullOrEmpty(newAccess.Plate))
                        {
                            if (await context.Cars.AnyAsync(c => c.LicencePlate == newAccess.Plate))
                            {
                                var entered = await context.Cars.FirstAsync(c => c.LicencePlate == newAccess.Plate).ConfigureAwait(false);
                                owner = entered.Owner;
                            }
                        }
                        else if (await context.Peoples.AnyAsync(c => c.CardNumber.CardNumber == newAccess.CardNumber).ConfigureAwait(false))
                        {
                            owner = await context.Peoples.FirstAsync(a => a.CardNumber.CardNumber == newAccess.CardNumber).ConfigureAwait(false);
                        }
                        if (await auth.IsAutorized(owner.Id, AccessType).ConfigureAwait(false))
                        {
                            newAccess.PeopleEntered = owner.Id;
                            newAccess.GrantedAccess = true;
                        }
                        await context.Access.AddAsync(newAccess).ConfigureAwait(false);
                        await context.SaveChangesAsync().ConfigureAwait(false);
                        //StaticEventHandler.SendMail(new MailEventArgs(ResourceString.AccessCarMailSubject, ResourceString.AccessCarMailBody, DateTime.Now));
                    }
                    catch (Exception ex)
                    {
                        StaticEventHandler.Log(System.Diagnostics.TraceLevel.Error, "error during Card verification", MethodBase.GetCurrentMethod(), ex);
                    }
                }
            }
            return newAccess.GrantedAccess;
        }
    }
}
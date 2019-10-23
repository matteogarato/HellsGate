using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using HellsGate.Models;
using Microsoft.EntityFrameworkCore;

namespace HellsGate.Lib
{
    public static class AutorizationManager
    {
        /// <summary>
        /// determine if the authorization of the car match the needed Authorization
        /// </summary>
        /// <param name="p_CarModelId">car anagraphic</param>
        /// <param name="p_AuthNeeded">needed Authorization</param>
        /// <returns></returns>
        public static async Task<bool> IsAutorized(string p_CarModelId, AuthType p_AuthNeeded)
        {
            using (var c = new Context())
            {
                CarAnagraphicModel car = await c.Cars.FirstOrDefaultAsync(ca => ca.LicencePlate == p_CarModelId).ConfigureAwait(false);
                return car.Owner.AutorizationLevel.AuthValue == p_AuthNeeded;
            }
        }

        /// <summary>
        /// private member that verify the user an the autorization
        /// </summary>
        /// <param name="p_PeopleModelId"></param>
        /// <param name="p_AuthNeeded"></param>
        /// <returns></returns>
        public static Task<bool> IsAutorized(int p_PeopleModelId, AuthType p_AuthNeeded)
        {
            using (var c = new Context())
            {
                PeopleAnagraphicModel usr = c.Peoples.FirstOrDefaultAsync(p => p.Id == p_PeopleModelId).ConfigureAwait(false).GetAwaiter().GetResult();
                if (usr.AutorizationLevel.AuthValue == p_AuthNeeded && AuthNotModified(usr.SafeAuthModel.Id).ConfigureAwait(false).GetAwaiter().GetResult())
                {
                    return Task.FromResult(true);
                }
                return Task.FromResult(false);
            }
        }

        private static Task<string> Encryptline(string p_textToEncrypt) => Task.FromResult(Convert.ToBase64String(EncriptLine(p_textToEncrypt).ConfigureAwait(false).GetAwaiter().GetResult()));

        public static async Task<bool> AutorizationModify(int p_PeopleModelIdRequest, int p_PeopleModelId, AuthType p_newAuthorization)
        {
            using (var c = new Context())
            {
                bool ret = false;

                PeopleAnagraphicModel Usr = await c.Peoples.FirstOrDefaultAsync(p => p.Id == p_PeopleModelId).ConfigureAwait(false);
                //in case of lowering the authorization i can do only if i'm not the only one with it, and only if thiere is at least one root 
                if (p_newAuthorization < Usr.AutorizationLevel.AuthValue && await c.Peoples.AnyAsync(p => p.AutorizationLevel.AuthValue == Usr.AutorizationLevel.AuthValue && p.Id != Usr.Id).ConfigureAwait(false) &&
                    await c.Peoples.AnyAsync(p => p.AutorizationLevel.AuthValue == AuthType.Root && p.Id != Usr.Id).ConfigureAwait(false))
                {
                    Usr.AutorizationLevel.AuthValue = p_newAuthorization;
                }
                else if (p_newAuthorization > Usr.AutorizationLevel.AuthValue)
                {
                    PeopleAnagraphicModel UsrRequest = await c.Peoples.FirstOrDefaultAsync(p => p.Id == p_PeopleModelIdRequest).ConfigureAwait(false);
                    if (Usr.AutorizationLevel.AuthValue == AuthType.Root)
                    {
                        Usr.AutorizationLevel.AuthValue = p_newAuthorization;
                    }
                }
                await c.SaveChangesAsync().ConfigureAwait(false);
                return ret;
            }
        }
        private static Task<byte[]> EncriptLine(string p_textToEncrypt)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
            var salted = new Rfc2898DeriveBytes(p_textToEncrypt, salt, 1000);
            byte[] hash = salted.GetBytes(20);
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);
            return Task.FromResult(hashBytes);
        }

        private static Task<bool> CompareHash(byte[] hashBase, byte[] hashToVerify)
        {
            for (int i = 16; i < 20; i++)
            {
                if (hashBase[i] != hashToVerify[i])
                {
                    return Task.FromResult(false);
                }
            }
            return Task.FromResult(true);
        }

        private static Task<bool> AuthNotModified(int p_AuthId)
        {
            using (var c = new Context())
            {
                SafeAuthModel authSaved = c.SafeAuthModels.FirstOrDefaultAsync(sa => sa.Id == p_AuthId).ConfigureAwait(false).GetAwaiter().GetResult();
                if (authSaved != null)
                {
                    return Task.FromResult(CompareHash(Convert.FromBase64String(authSaved.Control), EncriptLine(authSaved.AutId.ToString() + authSaved.UserId.ToString()).ConfigureAwait(false).GetAwaiter().GetResult()).ConfigureAwait(false).GetAwaiter().GetResult());
                }
                else
                {
                    return Task.FromResult(false);
                }
            }

        }

    }
}

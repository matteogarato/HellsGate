using System.Threading.Tasks;
using HellsGate.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System;

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
                CarAnagraphicModel car = await c.Cars.FirstOrDefaultAsync(ca => ca.LicencePlate == p_CarModelId);
                return car.Owner.AutorizationLevel.AuthValue == p_AuthNeeded;
            }
        }

        /// <summary>
        /// private member that verify the user an the autorization
        /// </summary>
        /// <param name="p_PeopleModelId"></param>
        /// <param name="p_AuthNeeded"></param>
        /// <returns></returns>
        public static async Task<bool> IsAutorized(int p_PeopleModelId, AuthType p_AuthNeeded)
        {
            using (var c = new Context())
            {
                bool ret = false;
                var Usr = await c.Peoples.FirstOrDefaultAsync(p => p.Id == p_PeopleModelId).ConfigureAwait(false);
                if (Usr.AutorizationLevel.AuthValue == p_AuthNeeded && await AuthNotModified(Usr.SafeAuthModel.Id))
                {
                    return true;
                }
                return false;
            }
        }

        private static string Encryptline(string p_textToEncrypt)
        {
            return Convert.ToBase64String(EncriptLine(p_textToEncrypt));
        }

        public static async Task<bool> AutorizationModify(int p_PeopleModelIdRequest, int p_PeopleModelId, AuthType p_newAuthorization)
        {
            using (var c = new Context())
            {
                bool ret = false;

                PeopleAnagraphicModel Usr = await c.Peoples.FirstOrDefaultAsync(p => p.Id == p_PeopleModelId);
                //in case of lowering the authorization i can do only if i'm not the only one with it, and only if thiere is at least one root 
                if (p_newAuthorization < Usr.AutorizationLevel.AuthValue && await c.Peoples.AnyAsync(p => p.AutorizationLevel.AuthValue == Usr.AutorizationLevel.AuthValue && p.Id != Usr.Id) &&
                    await c.Peoples.AnyAsync(p => p.AutorizationLevel.AuthValue == AuthType.Root && p.Id != Usr.Id))
                {
                    Usr.AutorizationLevel.AuthValue = p_newAuthorization;
                }
                else if (p_newAuthorization > Usr.AutorizationLevel.AuthValue)
                {
                    PeopleAnagraphicModel UsrRequest = await c.Peoples.FirstOrDefaultAsync(p => p.Id == p_PeopleModelIdRequest);
                    if (Usr.AutorizationLevel.AuthValue == AuthType.Root)
                    {
                        Usr.AutorizationLevel.AuthValue = p_newAuthorization;
                    }
                }
                await c.SaveChangesAsync();
                return ret;
            }
        }
        private static byte[] EncriptLine(string p_textToEncrypt)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
            var salted = new Rfc2898DeriveBytes(p_textToEncrypt, salt, 1000);
            byte[] hash = salted.GetBytes(20);
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);
            return hashBytes;
        }

        private static bool CompareHash(byte[] hashBase, byte[] hashToVerify)
        {
            for (int i = 16; i < 20; i++)
            {
                if (hashBase[i] != hashToVerify[i])
                {
                    return false;
                }
            }
            return true;
        }

        private static async Task<bool> AuthNotModified(int p_AuthId)
        {
            using (Context c = new Context())
            {
                var AuthSaved = await c.SafeAuthModels.FirstOrDefaultAsync(sa => sa.Id == p_AuthId);
                if (AuthSaved != null)
                {
                    return CompareHash(Convert.FromBase64String(AuthSaved.Control), EncriptLine(AuthSaved.AutId.ToString() + AuthSaved.UserId.ToString()));
                }
                else
                {
                    return false;
                }
            }

        }

    }
}

using HellsGate.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

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
            using (Context c = new Context())
            {
                var car = await c.Cars.FirstOrDefaultAsync(ca => ca.LicencePlate == p_CarModelId);
                return car.Owner.AutorizationLevel.AuthValue == p_AuthNeeded;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p_PeopleModelId"></param>
        /// <param name="p_AuthNeeded"></param>
        /// <returns></returns>
        public static async Task<bool> IsAutorized(int p_PeopleModelId, AuthType p_AuthNeeded)
        {
            return await _IsAutorized(p_PeopleModelId, p_AuthNeeded);
        }

        /// <summary>
        /// private member that verify the user an the autorization
        /// </summary>
        /// <param name="p_PeopleModelId"></param>
        /// <param name="p_AuthNeeded"></param>
        /// <returns></returns>
        private static async Task<bool> _IsAutorized(int p_PeopleModelId, AuthType p_AuthNeeded)
        {
            using (Context c = new Context())
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

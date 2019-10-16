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
                PeopleAnagraphicModel Usr = await c.Peoples.FirstOrDefaultAsync(p => p.Id == p_PeopleModelId);
                if (Usr.AutorizationLevel.AuthValue == p_AuthNeeded)
                {
                    SafeAuthModel certAuth = await c.SafeAuthModels.FirstOrDefaultAsync(sa => sa.User.Id == Usr.Id);
                    if (certAuth.UserSafe == SafeModel(Usr) && certAuth.AutSafe == SafeModel(Usr.AutorizationLevel))
                    {
                        ret = true;
                    }
                }
                return ret;
            }
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

        private static byte[] SafeModel(object src)
        {
            string toRet = "";
            foreach (System.Reflection.PropertyInfo prop in src.GetType().GetProperties())
            {
                toRet += prop.GetValue(src).ToString();
            }
            return CreateMD5(toRet);
        }

        private static byte[] CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] hashBytes = md5.ComputeHash(System.Text.Encoding.ASCII.GetBytes(input));

                // Convert the byte array to hexadecimal string
                //StringBuilder sb = new StringBuilder();
                //for (int i = 0; i < hashBytes.Length; i++)
                //{
                //    sb.Append(hashBytes[i].ToString("X2"));
                //}
                return hashBytes;
            }
        }
    }
}

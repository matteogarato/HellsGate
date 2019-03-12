using HellsGate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public static bool IsAutorized(string p_CarModelId, AuthType p_AuthNeeded)
        {
            using (Context c = new Context())
            {
                return c.Cars.FirstOrDefault(ca => ca.LicencePlate == p_CarModelId).AutorizationLevel.AuthValue == p_AuthNeeded;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p_PeopleModelId"></param>
        /// <param name="p_AuthNeeded"></param>
        /// <returns></returns>
        public static bool IsAutorized(int p_PeopleModelId, AuthType p_AuthNeeded)
        {
            return _IsAutorized(p_PeopleModelId, p_AuthNeeded);
        }

        /// <summary>
        /// private member that verify the user an the autorization
        /// </summary>
        /// <param name="p_PeopleModelId"></param>
        /// <param name="p_AuthNeeded"></param>
        /// <returns></returns>
        private static bool _IsAutorized(int p_PeopleModelId, AuthType p_AuthNeeded)
        {
            using (Context c = new Context())
            {
                bool ret = false;
                var Usr = c.Peoples.FirstOrDefault(p => p.Id == p_PeopleModelId);
                if (Usr.AutorizationLevel.AuthValue == p_AuthNeeded)
                {
                    var certAuth = c.SafeAuthModels.FirstOrDefault(sa => sa.User.Id == Usr.Id);
                    if (certAuth.UserSafe == SafeModel(Usr) && certAuth.AutSafe == SafeModel(Usr.AutorizationLevel))
                    {
                        ret = true;
                    }
                }
                return ret;
            }
        }

        private static byte[] SafeModel(object src)
        {
            string toRet = "";
            foreach (var prop in src.GetType().GetProperties())
            {
                toRet += prop.GetValue(src).ToString();
            }
            return CreateMD5(toRet);
        }

        public static byte[] CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
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

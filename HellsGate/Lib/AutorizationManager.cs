using HellsGate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
            using (Context c = new Context())
            {
                return c.Peoples.FirstOrDefault(p => p.Id == p_PeopleModelId).AutorizationLevel.AuthValue == p_AuthNeeded;
            }
        }
    }
}

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
        /// <param name="p_CarModel">car anagraphic</param>
        /// <param name="p_AuthNeeded">needed Authorization</param>
        /// <returns></returns>
        public static bool IsAutorized(CarAnagraphicModel p_CarModel, AuthType p_AuthNeeded)
        {
            return p_CarModel.AutorizationLevel.AuthValue == p_AuthNeeded;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p_PeopleModel"></param>
        /// <param name="p_AuthNeeded"></param>
        /// <returns></returns>
        public static bool IsAutorized(PeopleAnagraphicModel p_PeopleModel, AuthType p_AuthNeeded)
        {
            return p_PeopleModel.AutorizationLevel.AuthValue == p_AuthNeeded;
        }
    }
}

using HellsGate.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace HellsGate.Lib
{
    public static class LoginManager
    {

        public static async Task<int> GetUserByInput(string UserInput)
        {
            try
            {
                PeopleAnagraphicModel user = new PeopleAnagraphicModel();
                if (string.IsNullOrEmpty(UserInput) || string.IsNullOrEmpty(UserInput.Trim())) { return user.Id; }
                string ToFind = UserInput.Trim();
                using (var c = new Context())
                {
                    if (IsValidEmail(ToFind) && await c.Peoples.AnyAsync(p => p.Email == UserInput).ConfigureAwait(false))
                    {
                        user = await c.Peoples.FirstOrDefaultAsync(p => p.Email == UserInput).ConfigureAwait(false);

                    }
                    else if (!IsValidEmail(ToFind) && await c.Peoples.AnyAsync(p => p.Username == UserInput).ConfigureAwait(false))
                    {
                        user = await c.Peoples.FirstOrDefaultAsync(p => p.Username == UserInput).ConfigureAwait(false);

                    }
                    return user.Id;
                }
            }
            catch (Exception ex)
            {
                StaticEventHandler.Log(System.Diagnostics.TraceLevel.Error, "error during Login", MethodBase.GetCurrentMethod(), ex);
                throw new System.Exception();
            }
        }

        private static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch (Exception ex)
            {
                StaticEventHandler.Log(System.Diagnostics.TraceLevel.Error, "not a valid email", MethodBase.GetCurrentMethod(), ex);
                return false;
            }
        }

        public static async Task<bool> VerifyLogin(int user, string CyphredPassword)
        {
            try
            {
                using (var c = new Context())
                {
                    if (await c.Peoples.AnyAsync(p => p.Id == user).ConfigureAwait(false))
                    {
                        var userSelected = await c.Peoples.FirstOrDefaultAsync(p => p.Id == user).ConfigureAwait(false);
                        return await SecurLib.CompareHash(
                                    Convert.FromBase64String(userSelected.Password)
                                    , await SecurLib.EncriptLine(CyphredPassword).ConfigureAwait(false)).ConfigureAwait(false);
                    }
                    return false;

                }
            }
            catch (Exception ex)
            {
                StaticEventHandler.Log(System.Diagnostics.TraceLevel.Error, "error during Login", MethodBase.GetCurrentMethod(), ex);
                throw new System.Exception();
            }
        }


    }
}

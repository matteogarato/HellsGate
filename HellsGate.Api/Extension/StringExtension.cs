using HellsGate.Services;
using System;
using System.Reflection;

namespace HellsGate.Extension
{
    public static class StringExtension
    {
        public static bool IsValidEmail(this String str)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(str);
                return addr.Address == str;
            }
            catch (Exception ex)
            {
                StaticEventHandler.Log(System.Diagnostics.TraceLevel.Error, "not a valid email", MethodBase.GetCurrentMethod(), ex);
                return false;
            }
        }
    }
}
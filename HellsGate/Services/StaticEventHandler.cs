using HellsGate.Models;
using System;
using System.Diagnostics;
using System.Reflection;

namespace HellsGate.Services
{
    public static class StaticEventHandler
    {
        public static EventHandler<MailEventArgs> SendMailEvent;
        public static EventHandler<LogEventArgs> LogEvent;

        public static void Log(TraceLevel p_Trace, string p_Message, MethodBase p_Method, Exception p_Ex = null)
        {
            using (LogEventArgs log = new LogEventArgs(p_Trace, p_Message, p_Method, p_Ex))
            {
                LogEvent?.Invoke(null, log);
            }
        }

        public static void SendMail(MailEventArgs p_mailEventArgs)
        {
            SendMailEvent?.Invoke(null, p_mailEventArgs);
        }
    }
}
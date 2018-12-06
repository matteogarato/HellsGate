using System;
using System.Diagnostics;
using System.Reflection;

namespace HellsGate.Models
{
    public class MailEventArgs : EventArgs
    {
        public string Subject { get; set; }
        public string Message { get; set; }
        public DateTime TimeOfEvent { get; set; }
        public MailEventArgs(string p_Subject, string p_Message, DateTime p_TimeOfEvent)
        {
            Subject = p_Subject;
            Message = p_Message;
            TimeOfEvent = p_TimeOfEvent;
        }
    }

    public class LogEventArgs : EventArgs
    {
        public TraceLevel Level { get; set; }
        public string Message { get; set; }
        public Exception Ex { get; set; }
        public MethodBase Method { get; set; }
        public LogEventArgs(TraceLevel p_TraceLevel, string p_Message, MethodBase p_Method, Exception p_Ex = null)
        {
            Level = p_TraceLevel;
            Message = p_Message;
            Method = p_Method;
            if (p_Ex != null)
            {
                Ex = p_Ex;
            }
        }
    }
}

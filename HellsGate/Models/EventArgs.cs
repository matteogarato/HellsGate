using System;
using System.Reflection;

namespace HellsGate.Models
{
    public class MailEventArgs : EventArgs
    {
        public string Subject { get; set; }
        public string Message { get; set; }
        public DateTime TimeofEvent { get; set; }
    }

    public class LogEventArgs : EventArgs
    {
        public string Message { get; set; }
        public MethodBase Method { get; set; }
    }
}

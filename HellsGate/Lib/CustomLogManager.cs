using HellsGate.Models;
using log4net;
using log4net.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace HellsGate.Lib
{
    public class CustomLogManager
    {
        readonly Startup startup;
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public CustomLogManager(Startup p_Startup)
        {
            this.startup = p_Startup;
            this.startup.LogEvent += Log;
        }

        private void Log(object sender, LogEventArgs p_Logevent)
        {
            switch (p_Logevent.Level)
            {
                case System.Diagnostics.TraceLevel.Off:
                    break;
                case System.Diagnostics.TraceLevel.Error:
                    _log.Error(p_Logevent.Message, p_Logevent.Ex);
                    break;
                case System.Diagnostics.TraceLevel.Warning:
                    _log.Warn(p_Logevent.Message, p_Logevent.Ex);
                    break;
                case System.Diagnostics.TraceLevel.Info:
                    _log.Info(p_Logevent.Message);
                    break;
                case System.Diagnostics.TraceLevel.Verbose:
                    _log.Debug(p_Logevent.Message);
                    break;
            }
        }
    }
}

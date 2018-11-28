using HellsGate.Lib;


namespace HellsGate
{
    public class Locator
    {
        private readonly Startup _startup;
        private MailSenderManager _mailSenderManager;
        internal MailSenderManager MailSenderManager
        {
            get
            {
                if (_mailSenderManager == null)
                {
                    _mailSenderManager = new MailSenderManager(_startup);
                }
                return _mailSenderManager;
            }
            set
            {
                _mailSenderManager = value;
            }
        }

        internal void Init()
        {
            MailSenderManager = new MailSenderManager(_startup);
        }

        public Locator(Startup p_startup)
        {
            _startup = p_startup;
            Init();
        }
    }
}

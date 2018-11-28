using HellsGate.Lib;


namespace HellsGate
{
    public class Locator
    {
        
        private MailSenderManager _mailSenderManager;
        internal MailSenderManager MailSenderManager
        {
            get
            {
                if (_mailSenderManager == null)
                {
                    _mailSenderManager = new MailSenderManager();
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
            MailSenderManager = new MailSenderManager();
        }

        public Locator()
        {
            Init();
        }
    }
}

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
            private set
            {
                _mailSenderManager = value;
            }
        }

        internal void Init()
        {
            MailSenderManager = new MailSenderManager();
            CustomLogManager = new CustomLogManager();
        }

        private CustomLogManager _customLogManager;

        internal CustomLogManager CustomLogManager
        {
            get
            {
                if (_customLogManager == null)
                {
                    _customLogManager = new CustomLogManager();
                }
                return _customLogManager;
            }
            private set
            {
                _customLogManager = value;
            }
        }

        public Locator()
        {
            Init();
        }
    }
}
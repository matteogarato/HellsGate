using HellsGate.Services;

namespace HellsGate
{
    public class Locator
    {
        private CustomLogManagerService _customLogManager;
        private MailSenderManagerService _mailSenderManager;

        public Locator()
        {
            Init();
        }

        internal CustomLogManagerService CustomLogManager
        {
            get
            {
                if (_customLogManager == null)
                {
                    _customLogManager = new CustomLogManagerService();
                }
                return _customLogManager;
            }
            private set
            {
                _customLogManager = value;
            }
        }

        internal MailSenderManagerService MailSenderManager
        {
            get
            {
                if (_mailSenderManager == null)
                {
                    _mailSenderManager = new MailSenderManagerService();
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
            MailSenderManager = new MailSenderManagerService();
            CustomLogManager = new CustomLogManagerService();
        }
    }
}
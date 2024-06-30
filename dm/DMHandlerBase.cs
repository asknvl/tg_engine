using logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tg_engine.database.postgre.models;
using tg_engine.userapi;

namespace tg_engine.dm
{
    public class DMHandlerBase
    {
        #region vars
        IUserApiFactory userApiFactory;
        ILogger logger;
        #endregion

        #region properties                
        protected DMStartupSettings settings { get; private set; }
        protected UserApiHandlerBase user { get; private set; }
        string is_active { get; set; }
        #endregion

        public DMHandlerBase(DMStartupSettings settings, ILogger logger)
        {
            this.settings = settings;
            this.logger = logger;
            userApiFactory = new UserApiFactory(settings.account.api_id, settings.account.api_hash, logger);
        }

        #region public
        public virtual async Task Start()
        {
            user = userApiFactory.Get(settings.account.phone_number, settings.account.two_fa);
            await Task.CompletedTask;
        }

        public virtual void Stop()
        {            
        }
        #endregion

    }
}

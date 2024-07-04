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
        string tag;
        #endregion

        #region properties                
        public DMStartupSettings settings { get; private set; }
        public UserApiHandlerBase user { get; private set; }        
        public DMHandlerStatus status { get; private set; }
        #endregion

        public DMHandlerBase(DMStartupSettings settings, ILogger logger)
        {
            tag = $"dm {settings.source}";

            this.settings = settings;
            this.logger = logger;
            userApiFactory = new UserApiFactory(settings.account.api_id, settings.account.api_hash, logger);

            status = DMHandlerStatus.inactive;
        }

        #region public
        public virtual async Task Start()
        {
            if (status == DMHandlerStatus.active)
            {
                logger.err($"{tag}", "Уже запущен");
                return;
            }
                

            user = userApiFactory.Get(settings.account.phone_number, settings.account.two_fa);
            await Task.CompletedTask;
            status = DMHandlerStatus.active;
            logger.inf_urgent($"{tag}", "Запуск выполнен");
        }

        public virtual void Stop()
        {
            status = DMHandlerStatus.inactive;
            logger.warn($"{tag}", "Остановлен");
        }
        #endregion

    }

    public enum DMHandlerStatus
    {        
        inactive,
        active,
        verification,
        banned
    }
}


using logger;
using System.Diagnostics;
using System.Formats.Asn1;
using System.Reflection;
using tg_engine.config;
using tg_engine.database.postgre;
using tg_engine.database.postgre.models;
using tg_engine.dm;

namespace tg_engine
{
    public class tg_engine_base
    {
        #region const
        const string tag = "tgengn";
        #endregion

        #region vars
        ILogger logger;                
        IPostgreProvider postgreProvider;
        #endregion

        #region properties
        public Version Version
        { 
            get => Assembly.GetExecutingAssembly().GetName().Version;
        }
        public List<DMHandlerBase> DMHandlers { get; } = new();

        public bool IsActive { get; set; }
        #endregion

        public tg_engine_base(ILogger logger)
        {
            this.logger = logger;

            #region dependencies              
            #endregion
        }

        #region private
        async Task initDMhandlers(List<DMStartupSettings> startupSettings)
        {
            foreach (var settings in startupSettings)
            {

                Debug.WriteLine($"{settings.source} {settings.account.phone_number}");

                var dm = new DMHandlerBase(settings, logger);                                
                DMHandlers.Add(dm);                
            }
        }
        async Task initService()
        {
            try
            {
                logger?.inf(tag, $"Загрузка конфигурации сервиса...");

                var vars = variables.getInstance();       
                postgreProvider = new PostgreProvider(vars.tg_engine_variables.accounts_settings_db);

                var dMStartupSettings  = await postgreProvider.GetStatupData();
                await initDMhandlers(dMStartupSettings);                

            } catch (Exception ex)
            {
                logger.err(tag, $"Не удалось загрузить конфигурацию сервиса {ex.Message}");
            }
        }
        #endregion

        #region public
        public virtual async Task Start()
        {
            try
            {

                if (IsActive)
                    throw new Exception("Сервис уже запущен");

                await initService();


            } catch (Exception ex)
            {
                logger.err(tag, $"{ex.Message}");
            }

            logger?.inf(tag, $"Запуск сервиса (вер. {Version})...");
            await Task.CompletedTask;
            IsActive = true;
            logger?.inf_urgent(tag, $"Запуск выполнен");
        }
        #endregion

        public virtual async Task Stop()
        {
            await Task.CompletedTask;
            logger.warn(tag, "Cервис остановлен");
        }
    }
}

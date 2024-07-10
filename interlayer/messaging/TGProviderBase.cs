using logger;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tg_engine.database.mongo;
using tg_engine.database.postgre;
using tg_engine.database.postgre.models;
using tg_engine.interlayer.chats;
using TL;

namespace tg_engine.interlayer.messaging
{
    public abstract class TGProviderBase
    {
        #region vars
        Guid account_id;
        IPostgreProvider postgreProvider;
        IMongoProvider mongoProvider;
        IChatsProvider chatsProvider;
        ILogger logger;
        string tag;
        #endregion

        public TGProviderBase(Guid account_id, IPostgreProvider postgreProvider, IMongoProvider mongoProvider, ILogger logger) {

            tag = $"prvdr";

            this.account_id = account_id;
            this.postgreProvider = postgreProvider;
            this.mongoProvider = mongoProvider;

            this.logger = logger;

            chatsProvider = new ChatsProvider(postgreProvider);
        }

        #region private
        Task processMessage(MessageBase message)
        {
            return Task.CompletedTask;
        }
        #endregion

        #region public
        //Сообщение получено из ТГ
        public async Task OnMessageRX(TL.UpdateNewMessage unm, TL.User user) {

            try
            {
                var u = new telegram_user()
                {
                    telegram_id = user.ID,
                    firstname = user.first_name,
                    lastname = user.last_name,
                    username = user.username
                };

                Stopwatch stopwatch = new Stopwatch();

                stopwatch.Start();
                var exists = await chatsProvider.CollectUserChat(account_id, u);
                stopwatch.Stop();

                logger.inf(tag, $"{user.ID} exists={exists} time={stopwatch.ElapsedMilliseconds} ms");

            } catch (Exception ex)
            {
                
            }

            //return Task.CompletedTask;
        }

        //Сообщение получено от Клиента и должно быть отправлено в ТГ 
        public Task OnMessageTX(MessageBase message) {
            MessageTXRequest?.Invoke(message);
            return Task.CompletedTask;
        }
        #endregion

        #region events
        public event Action<MessageBase> MessageTXRequest;
        #endregion
    }
}

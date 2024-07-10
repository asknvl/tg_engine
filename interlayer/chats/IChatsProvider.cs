using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tg_engine.database.postgre.models;

namespace tg_engine.interlayer.chats
{
    public interface IChatsProvider
    {
        Task<bool> CollectUserChat(Guid account_id, telegram_user user);
    }
}

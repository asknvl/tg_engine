using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tg_engine.interlayer.messaging
{
    public class MessageBase
    {
        public Guid chat_id { get; set; }
        public string direction { get; set; }   
        public int telegram_message_id { get; set; }
        public long src_telegram_user_id { get; set; }
        public long dst_telegram_user_id { get; set; }
        public string text { get; set; }   
        public DateTime date { get; set; }  
        public DateTime edited_date { get; set; }
        public bool is_read { get; set; }
        public int reply_to_message_id { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set;}

    }
}

using logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tg_engine.userapi
{
    public class userapi_handler_v0 : UserApiHandlerBase
    {
        public userapi_handler_v0(string phone_number, string _2fa_password, string api_id, string api_hash, ILogger logger) : base(phone_number, _2fa_password, api_id, api_hash, logger)
        {
        }
    }
}

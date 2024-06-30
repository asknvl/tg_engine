using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tg_engine.userapi
{
    public interface IUserApiFactory
    {
        UserApiHandlerBase Get(string phone_number, string _2fa_password);
    }
}

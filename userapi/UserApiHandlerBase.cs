using logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TL;

namespace tg_engine.userapi
{
    public class UserApiHandlerBase
    {
        #region const
        const string tag = "usrapi";        
        #endregion

        #region properties
        public string phone_number { get; set; }
        public string _2fa_password { get; set; }
        public string api_id { get; set; }
        public string api_hash { get; set; }
        public long tg_id { get; set; }
        public string username { get; set; }
        public UserApiStatus status { get; set; }
        #endregion

        #region vars
        ILogger logger;
        string session_directory = Path.Combine("C:", "userpool");
        string verifyCode;
        readonly ManualResetEventSlim verifyCodeReady = new();
        #endregion

        public UserApiHandlerBase(string phone_number, string _2fa_password, string api_id, string api_hash, ILogger logger)
        {
            this.phone_number = phone_number;
            this._2fa_password = _2fa_password;
            this.api_id = api_id;
            this.api_hash = api_hash;

            this.logger = logger;

            status = UserApiStatus.stopped;
        }

        #region private
        void processRpcException(RpcException ex)
        {
            switch (ex.Message)
            {
                case "PHONE_NUMBER_BANNED":
                    status = UserApiStatus.banned;
                    break;

                case "SESSION_REVOKED":
                case "AUTH_KEY_UNREGISTERED":
                    status = UserApiStatus.revked;
                    break;

            }

            logger.err(tag, $"RcpException: {ex.Message}");
        }

        private string config(string what)
        {
            if (!Directory.Exists(session_directory))
                Directory.CreateDirectory(session_directory);

            switch (what)
            {
                case "api_id": return api_id;
                case "api_hash": return api_hash;
                case "session_pathname": return $"{session_directory}/{phone_number}.session";
                case "phone_number": return phone_number;
                case "verification_code":
                    status = UserApiStatus.verification;                  
                    verifyCodeReady.Reset();
                    verifyCodeReady.Wait();
                    return verifyCode;                
                case "password": return _2fa_password;
                default: return null;
            }
        }
        #endregion

        #region public
        public virtual Task Start()
        {
            try
            {

            }
            catch (RpcException ex)
            {
                processRpcException(ex);
            }
            catch (Exception ex)
            {

            }
            return Task.CompletedTask;
        }

        public void SetVerifyCode(string code)
        {
            verifyCode = code;
        }

        public virtual void Stop()
        {
        }

        public UserApiStatus GetStatus() { return status; }
        #endregion
    }

    public enum UserApiStatus : int
    {
        stopped = 0,
        active = 1,
        verification = 2,
        banned = 3,
        revked = 4
    }
}

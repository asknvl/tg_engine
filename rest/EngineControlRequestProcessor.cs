using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace tg_engine.rest
{
    public class EngineControlRequestProcessor : IRequestProcessor
    {
        #region vars
        tg_engine_base tg_engine;
        #endregion

        public EngineControlRequestProcessor(tg_engine_base tg_engine) {
            this.tg_engine = tg_engine;
        }

        #region dtos  
        class dmHandlerDto
        {
            public Guid id { get; set; }
            public string source { get; set; }
            public string phone_number { get; set; }
            public string status { get; set; }
        }
        #endregion

        #region helpers
        List<dmHandlerDto> getDMHandlers()
        {
            List<dmHandlerDto> res = new();

            foreach (var dm in tg_engine.DMHandlers) {
                res.Add(new dmHandlerDto() { 
                    id = dm.settings.account.id,
                    source = dm.settings.source,
                    phone_number = dm.settings.account.phone_number,
                    status = dm.status.ToString()
                });
            }

            return res;
        }
        #endregion

        #region public
        public async Task<(HttpStatusCode, string)> ProcessGetRequest(string[] splt_route)
        {

            var code = HttpStatusCode.NotFound;
            var responseText = code.ToString();

            switch (splt_route[2])
            {
                case "pmhandlers":
                    code = HttpStatusCode.OK;
                    responseText = JsonConvert.SerializeObject(getDMHandlers(), Formatting.Indented);
                    break;
            }

            await Task.CompletedTask;
            return (code, responseText);
        }

        public Task<(HttpStatusCode, string)> ProcessPostRequest(string[] splt_route, string data)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}

using Common;
using CRMApiModel.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ui.Service
{
    public class CRMService
    {
        public CRMService()
        {
            Token = GetToken();
        }

        public string Token { get; set; }

        public string GetToken()
        {
            string httpResponse = string.Empty;
            string queryParam = "grant_type=password&client_id=ea1c08c2488f9b27bd5063aa5e2f3db9&client_secret=c6fbf8989db2d8c4228de2a550e15205&redirect_uri=https://api.xiaoshouyi.com&username=crm@sokan.com.cn&password=Sokan8719177754t6VPVD";
            HttpService.HttpGet($"https://api.xiaoshouyi.com/oauth2/token?{queryParam}", out httpResponse, 6000);
            var res = JsonHelper.DeserializeObject<TokenResponseModel>(httpResponse);
            return res.token_type+" "+res.access_token;
        }

        public QueryXoqlResponseModel<T> GetQueryXoqlData<T>(string query)
        {
            string httpResponse = string.Empty;
            byte[] data = System.Text.Encoding.UTF8.GetBytes($"xoql={query.Trim()}");
            HttpService.HttpPost("https://api.xiaoshouyi.com/rest/data/v2.0/query/xoql", data, out httpResponse, 6000,$"Authorization:{Token}");
            return JsonHelper.DeserializeObject<QueryXoqlResponseModel<T>>(httpResponse);
        }


        //public EntityDescriptionResponseModel GetEntityDescription(string apiKey)
        //{
        //    string httpResponse = string.Empty;
        //    HttpService.HttpGet($"https://api.xiaoshouyi.com/rest/data/v2/objects/{apiKey}/description", out httpResponse, 6000, $"Authorization:{Token}");
        //    return JsonHelper.DeserializeObject<EntityDescriptionResponseModel>(httpResponse);
        //}

        //public string GetEntityDescriptionString(string apiKey)
        //{
        //    string httpResponse = string.Empty;
        //    HttpService.HttpGet($"https://api.xiaoshouyi.com/rest/data/v2/objects/{apiKey}/description", out httpResponse, 6000, $"Authorization:{Token}");
        //    return httpResponse;
        //}


    }
}

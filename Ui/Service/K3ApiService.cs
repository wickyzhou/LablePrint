using Common;
using K3ApiModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ui.Service
{
    public class K3ApiService
    {
        private static readonly string apiUrl = "http://192.168.2.150/K3API";
        //松井测试库授权码：d179f673dda1188267877536c7712d28dc6914c66d438264  ‘
        //松井正式库授权码：cc7c2a733c0a29eca5d9f62f5d4f225a8668d638842116d6
        //松润测试库： 3bfc737f19b0432dd0efba5a3c35a815d0600147724683d0
        //红玲测试： 9c1f17bc02ee5aa4972414559c39c835eebd86aa2f3e7391
        private static readonly string authCode = "9c1f17bc02ee5aa4972414559c39c835eebd86aa2f3e7391";
        private  readonly string _entity;
        public  K3ApiService(string entity)
        {
            _entity = entity;
            Token = GetToken();
        }

        public string Token { get; set; } 

        public string GetToken()
        {
            string httpResponse = string.Empty;
            HttpService.HttpGet(apiUrl + "/Token/Create?authorityCode=" + authCode, out httpResponse, 6000);
            return JsonHelper.DeserializeObject<K3ApiTokenResponseModel>(httpResponse).Data.Token;
        }


        public K3ApiResponseModel GetTemplate()
        {
            string httpResponse = string.Empty;
            HttpService.HttpGet(apiUrl + "/"+ _entity + "/GetTemplate?Token=" + authCode, out httpResponse, 6000);
            return JsonHelper.DeserializeObject<K3ApiResponseModel>(httpResponse);
        }

        public K3ApiResponseModel GetList(string postJson)
        {
            string httpResponse = string.Empty;
            byte[] data = System.Text.Encoding.UTF8.GetBytes(postJson);
            HttpService.HttpPost(apiUrl + "/" + _entity + "/GetList?Token=" + Token, data, out httpResponse, 6000);
            return JsonHelper.DeserializeObject<K3ApiResponseModel>(httpResponse);
        }

        public K3ApiResponseModel GetDetail(string postJson)
        {
            string httpResponse = string.Empty;
            byte[] data = System.Text.Encoding.UTF8.GetBytes(postJson);
            HttpService.HttpPost(apiUrl + "/" + _entity + "/GetDetail?Token=" + Token, data, out httpResponse, 6000);
            return JsonHelper.DeserializeObject<K3ApiResponseModel>(httpResponse);
        }

        public K3ApiInsertResponseModel Insert(string postJson)
        {
            string httpResponse = string.Empty;
            byte[] data = System.Text.Encoding.UTF8.GetBytes(postJson);
            HttpService.HttpPost(apiUrl + "/" + _entity + "/Save?Token=" + Token, data, out httpResponse, 6000);
            return JsonHelper.DeserializeObject<K3ApiInsertResponseModel>(httpResponse);
        }

        public K3ApiResponseModel Update(string postJson)
        {
            string httpResponse = string.Empty;
            byte[] data = System.Text.Encoding.UTF8.GetBytes(postJson);
            HttpService.HttpPost(apiUrl + "/" + _entity + "/Update?Token=" + Token, data, out httpResponse, 6000);
            return JsonHelper.DeserializeObject<K3ApiResponseModel>(httpResponse);
        }

        public K3ApiCheckBillResponseModel CheckBill(string postJson)
        {
            string httpResponse = string.Empty;
            byte[] data = System.Text.Encoding.UTF8.GetBytes(postJson);
            HttpService.HttpPost(apiUrl + "/" + _entity + "/CheckBill?Token=" + Token, data, out httpResponse, 6000);
            return JsonHelper.DeserializeObject<K3ApiCheckBillResponseModel>(httpResponse);
        }
    }
}

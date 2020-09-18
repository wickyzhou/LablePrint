using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;

namespace Ui.Service
{
    public static class HttpService
    {
        private static readonly string DefaultUserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";

        private static Encoding requestEncoding = Encoding.UTF8;

        public static bool HttpGet(string url, out string HttpWebResponseString, int timeout,string requestHeader = "")
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }
            HttpWebResponseString = "";
            try
            {
                HttpWebRequest httpWebRequest = WebRequest.Create(url) as HttpWebRequest;
                httpWebRequest.Method = "GET";
                httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                httpWebRequest.UserAgent = DefaultUserAgent;
                httpWebRequest.Timeout = timeout;
                if(!string.IsNullOrEmpty(requestHeader))
                AddRequestHeaders(httpWebRequest, requestHeader);
                HttpWebResponse httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse;
                HttpWebResponseString = ReadHttpWebResponse(httpWebResponse);
                return true;
            }
            catch (Exception ex)
            {
                HttpWebResponseString = ex.ToString();
                return false;
            }
        }

        public static bool HttpPost(string url, byte[] Data, out string HttpWebResponseString, int timeout, string requestHeader = "")
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }
            HttpWebResponseString = "";
            HttpWebRequest httpWebRequest = null;
            Stream stream = null;
            try
            {
                httpWebRequest = (WebRequest.Create(url) as HttpWebRequest);
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                httpWebRequest.UserAgent = DefaultUserAgent;
                httpWebRequest.Timeout = timeout;
                if (!string.IsNullOrEmpty(requestHeader))
                    AddRequestHeaders(httpWebRequest, requestHeader);
                if (Data != null)
                {
                    requestEncoding.GetString(Data);
                    stream = httpWebRequest.GetRequestStream();
                    stream.Write(Data, 0, Data.Length);
                }
                HttpWebResponse httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse;
                HttpWebResponseString = ReadHttpWebResponse(httpWebResponse);
                return true;
            }
            catch (Exception ex)
            {
                HttpWebResponseString = ex.ToString();
                return false;
            }
            finally
            {
                stream?.Close();
            }
        }

        public static string ReadHttpWebResponse(HttpWebResponse HttpWebResponse)
        {
            Stream stream = null;
            StreamReader streamReader = null;
            string text = null;
            try
            {
                stream = HttpWebResponse.GetResponseStream();
                streamReader = new StreamReader(stream, Encoding.GetEncoding("utf-8"));
                return streamReader.ReadToEnd();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                streamReader?.Close();
                stream?.Close();
                HttpWebResponse?.Close();
            }
        }

        /// <summary>
        /// 根据t_item_source_list表的list_request_headers 和 info_request_headers 内容添加到HttpRequest.Headers
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requestHeaders"></param>
        public static void AddRequestHeaders(HttpWebRequest request, string requestHeaders)
        {
            //&& request.CookieContainer.GetCookies(request.RequestUri) == null
            try
            {   //将头部格式不改变的加入  不需要替换中间的-； Content-Type  √    ContentType ×
                if (requestHeaders.Length > 0)
                {
                    foreach (var item in requestHeaders.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        SetHeaderValue(request.Headers, item.Split(new char[] { ':', '：' }, 2)[0].Trim(), item.Split(new char[] { ':', '：' }, 2)[1].Trim());
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("requestHeader格式错误，多个Header用回车换行分割，单个Header用冒号分隔。示例：Referrer Policy: unsafe-url \r\n accept-encoding: gzip, deflate, br", e);
            }


        }

        /// <summary>
        /// 将字串形式的Headers，添加到HttpRequest.Headers对象中
        /// </summary>
        /// <param name="header"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public static void SetHeaderValue(WebHeaderCollection header, string name, string value)
        {
            var property = typeof(WebHeaderCollection).GetProperty("InnerCollection", BindingFlags.Instance | BindingFlags.NonPublic);
            if (property != null)
            {
                var collection = property.GetValue(header, null) as NameValueCollection;
                collection[name] = value;
            }

        }

    }
}

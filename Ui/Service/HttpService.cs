using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Ui.Service
{
    public static class HttpService
    {
        private static readonly string DefaultUserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";

        private static Encoding requestEncoding = Encoding.UTF8;

        public static bool HttpGet(string url, out string HttpWebResponseString, int timeout)
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

        public static bool HttpPost(string url, byte[] Data, out string HttpWebResponseString, int timeout)
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
    }
}

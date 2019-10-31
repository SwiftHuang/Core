using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace hwj.CommonLibrary.Object
{
    /// <summary>
    /// HttpHelper
    /// </summary>
    public class HttpHelper
    {
        #region Property

        //private const string GetContentType = "text/plain";
        private const int defaultTimeOut = 30000;

        private const string postContentType = "application/x-www-form-urlencoded";

        #endregion Property

        #region Public Function

        /// <summary>
        /// 使用Post的方式提交数据（Timeout默认30秒;ContentType为application/x-www-form-urlencoded）
        /// </summary>
        /// <param name="url">提交Url</param>
        /// <param name="param">提交参数</param>
        /// <param name="encoding">字符编码</param>
        /// <returns></returns>
        public static string PostAction(string url, string param, Encoding encoding)
        {
            return PostAction(url, param, encoding, defaultTimeOut);
        }

        /// <summary>
        /// 使用Post的方式提交数据（Timeout默认30秒;ContentType为application/x-www-form-urlencoded）
        /// </summary>
        /// <param name="url">提交Url</param>
        /// <param name="param">提交参数</param>
        /// <param name="encoding">字符编码</param>
        /// <param name="timeout">超时时间(单位:毫秒)</param>
        /// <returns></returns>
        public static string PostAction(string url, string param, Encoding encoding, int timeout)
        {
            return PostAction(url, postContentType, param, null, encoding, timeout);
        }

        /// <summary>
        ///使用Post的方式提交Json数据（Timeout默认30秒;ContentType为application/json）
        /// </summary>
        /// <param name="url">提交Url</param>
        /// <param name="param">提交参数</param>
        /// <param name="encoding">字符编码</param>
        /// <param name="timeout">超时时间(单位:毫秒)</param>
        /// <returns></returns>
        public static string PostJsonAction(string url, string param, Dictionary<string, string> header, Encoding encoding, int timeout)
        {
            return PostAction(url, "application/json", param, header, encoding, timeout);
        }
        public static string PostJsonAction(string url, string param, Dictionary<string, string> header, Encoding encoding, int timeout, out HttpStatusCode statusCode)
        {
            return PostAction(url, "application/json", param, header, encoding, timeout, out statusCode);
        }

        public static string PostJsonAction(string url, string param, Encoding encoding, int timeout)
        {
            return PostAction(url, "application/json", param, null, encoding, timeout);
        }
        /// <summary>
        /// 使用Post的方式提交数据（Timeout默认30秒）
        /// </summary>
        /// <param name="url">提交Url</param>
        /// <param name="contentType">Http标头</param>
        /// <param name="param">提交参数</param>
        /// <param name="encoding">字符编码</param>
        /// <param name="timeout">超时时间(单位:毫秒)</param>
        /// <returns></returns>
        public static string PostAction(string url, string contentType, string param, Encoding encoding, int timeout)
        {
            return PostAction(url, contentType, param, null, encoding, timeout);
        }

        /// <summary>
        /// 使用Post的方式提交数据（Timeout默认30秒）
        /// </summary>
        /// <param name="url">提交Url</param>
        /// <param name="contentType">Http标头</param>
        /// <param name="param">Http标头 list</param>
        /// <param name="param">Heade</param>
        /// <param name="encoding">字符编码</param>
        /// <param name="timeout">超时时间(单位:毫秒)</param>
        /// <returns></returns>
        public static string PostAction(string url, string contentType, string param, Dictionary<string, string> header, Encoding encoding, int timeout, out HttpStatusCode statusCode)
        {
            //SetServicePointManager();
            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            //if (header != null)
            //{
            //    foreach (var item in header)
            //    {
            //        request.Headers.Add(item.Key, item.Value);
            //    }
            //}
            //request.Timeout = timeout > defaultTimeOut ? timeout : defaultTimeOut;
            //request.Method = "POST";
            //request.ContentType = contentType;

            //Stream dataStream = request.GetRequestStream();
            //byte[] bytes = DataToBytes(param, encoding);

            //dataStream.Write(bytes, 0, bytes.Length);
            //dataStream.Close();

            //string rs = GetResponeString(request);
            return Action(url, "POST", contentType, param, header, encoding, timeout, out statusCode);
        }

        /// <summary>
        /// 使用Post的方式提交数据（Timeout默认30秒）
        /// </summary>
        /// <param name="url">提交Url</param>
        /// <param name="contentType">Http标头</param>
        /// <param name="param">Http标头 list</param>
        /// <param name="param">Heade</param>
        /// <param name="encoding">字符编码</param>
        /// <param name="timeout">超时时间(单位:毫秒)</param>
        /// <returns></returns>
        public static string PostAction(string url, string contentType, string param, Dictionary<string, string> header, Encoding encoding, int timeout)
        {
            //SetServicePointManager();
            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            //if (header != null)
            //{
            //    foreach (var item in header)
            //    {
            //        request.Headers.Add(item.Key, item.Value);
            //    }
            //}
            //request.Timeout = timeout > defaultTimeOut ? timeout : defaultTimeOut;
            //request.Method = "POST";
            //request.ContentType = contentType;

            //Stream dataStream = request.GetRequestStream();
            //byte[] bytes = DataToBytes(param, encoding);

            //dataStream.Write(bytes, 0, bytes.Length);
            //dataStream.Close();

            //string rs = GetResponeString(request);
            return Action(url, "POST", contentType, param, header, encoding, timeout);
        }

        /// <summary>
        ///使用Patch的方式提交Json数据（Timeout默认30秒;ContentType为application/json）
        /// </summary>
        /// <param name="url">提交Url</param>
        /// <param name="param">提交参数</param>
        /// <param name="encoding">字符编码</param>
        /// <param name="timeout">超时时间(单位:毫秒)</param>
        /// <returns></returns>
        public static string PatchJsonAction(string url, string param, Dictionary<string, string> header, Encoding encoding, int timeout)
        {
            return PatchAction(url, "application/json", param, header, encoding, timeout);
        }


        public static string PatchJsonAction(string url, string param, Encoding encoding, int timeout)
        {
            return PatchAction(url, "application/json", param, null, encoding, timeout);
        }

        /// <summary>
        /// 使用PATCH的方式提交数据（Timeout默认30秒）
        /// </summary>
        /// <param name="url">提交Url</param>
        /// <param name="contentType">Http标头</param>
        /// <param name="param">Http标头 list</param>
        /// <param name="param">Heade</param>
        /// <param name="encoding">字符编码</param>
        /// <param name="timeout">超时时间(单位:毫秒)</param>
        /// <returns></returns>
        public static string PatchAction(string url, string contentType, string param, Dictionary<string, string> header, Encoding encoding, int timeout)
        {

            return Action(url, "PATCH", contentType, param, header, encoding, timeout);
        }
        /// <summary>
        /// 使用不同的method方式提交数据（Timeout默认30秒）
        /// </summary>
        /// <param name="url">提交Url</param>
        /// <param name="method">"POST PUT PATCH"</param>
        /// <param name="contentType">Http标头</param>
        /// <param name="param">Http标头 list</param>
        /// <param name="param">Heade</param>
        /// <param name="encoding">字符编码</param>
        /// <param name="timeout">超时时间(单位:毫秒)</param>
        /// <returns></returns>
        public static string Action(string url, string method, string contentType, string param, Dictionary<string, string> header, Encoding encoding, int timeout)
        {
            SetServicePointManager((SecurityProtocolType)3072);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            if (header != null)
            {
                foreach (var item in header)
                {
                    request.Headers.Add(item.Key, item.Value);
                }
            }
            request.Timeout = timeout > defaultTimeOut ? timeout : defaultTimeOut;
            request.Method = method;
            request.ContentType = contentType;

            Stream dataStream = request.GetRequestStream();
            byte[] bytes = DataToBytes(param, encoding);

            dataStream.Write(bytes, 0, bytes.Length);
            dataStream.Close();

            HttpStatusCode code = 0;

            string rs = GetResponeString(request, out code);
            return rs;
        }

        /// <summary>
        /// 使用不同的method方式提交数据（Timeout默认30秒）
        /// </summary>
        /// <param name="url">提交Url</param>
        /// <param name="method">"POST PUT PATCH"</param>
        /// <param name="contentType">Http标头</param>
        /// <param name="param">Http标头 list</param>
        /// <param name="param">Heade</param>
        /// <param name="encoding">字符编码</param>
        /// <param name="timeout">超时时间(单位:毫秒)</param>
        /// <returns></returns>
        public static string Action(string url, string method, string contentType, string param, Dictionary<string, string> header, Encoding encoding, int timeout, out HttpStatusCode statusCode)
        {
            SetServicePointManager((SecurityProtocolType)3072);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            if (header != null)
            {
                foreach (var item in header)
                {
                    request.Headers.Add(item.Key, item.Value);
                }
            }
            request.Timeout = timeout > defaultTimeOut ? timeout : defaultTimeOut;
            request.Method = method;
            request.ContentType = contentType;

            Stream dataStream = request.GetRequestStream();
            byte[] bytes = DataToBytes(param, encoding);

            dataStream.Write(bytes, 0, bytes.Length);
            dataStream.Close();

            string rs = GetResponeString(request, out statusCode);
            return rs;
        }

        public static string GetAction(string url, Dictionary<string, string> data)
        {
            return GetAction(url, data, null);
        }
        public static string GetAction(string url, Dictionary<string, string> data, Dictionary<string, string> header)
        {
            //SetServicePointManager();

            if (data != null && data.Count > 0)
            {
                url = CombineQueryUrl(url, data);
            }
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            if (header != null)
            {
                foreach (var item in header)
                {
                    request.Headers.Add(item.Key, item.Value);
                }
            }

            //request.Timeout = timeOut;
            //request.ContentType = GetContentType;
            request.Method = "GET";
            HttpStatusCode code = 0;
            string rs = GetResponeString(request, out code);
            return rs;
        }
        public static string GetAction(string url, Dictionary<string, string> data, Dictionary<string, string> header, out HttpStatusCode statusCode)
        {
            //SetServicePointManager();

            if (data != null && data.Count > 0)
            {
                url = CombineQueryUrl(url, data);
            }
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            if (header != null)
            {
                foreach (var item in header)
                {
                    request.Headers.Add(item.Key, item.Value);
                }
            }

            //request.Timeout = timeOut;
            //request.ContentType = GetContentType;
            request.Method = "GET";

            string rs = GetResponeString(request, out statusCode);
            return rs;
        }

        ///// <summary>
        ///// 获取IP地址
        ///// </summary>
        ///// <returns></returns>
        //public static string GetIP()
        //{
        //    string IP4Address = String.Empty;

        //    foreach (IPAddress ipa in Dns.GetHostAddresses(System.Web.HttpContext.Current.Request.UserHostAddress))
        //    {
        //        if (ipa.AddressFamily.ToString() == "InterNetwork")
        //        {
        //            IP4Address = ipa.ToString();
        //            break;
        //        }
        //    }

        //    if (IP4Address != String.Empty)
        //    {
        //        return IP4Address;
        //    }

        //    foreach (IPAddress IPA in Dns.GetHostAddresses(Dns.GetHostName()))
        //    {
        //        if (IPA.AddressFamily.ToString() == "InterNetwork")
        //        {
        //            IP4Address = IPA.ToString();
        //            break;
        //        }
        //    }

        //    return IP4Address;
        //}

        // For Https
        public static void SetServicePointManager(SecurityProtocolType securityProtocolType)
        {
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            ServicePointManager.CheckCertificateRevocationList = true;
            ServicePointManager.DefaultConnectionLimit = 100;
            ServicePointManager.Expect100Continue = false;
            ServicePointManager.SecurityProtocol = securityProtocolType;
        }
        #endregion Public Function

        #region Private Function

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors) { return true; }

        private static byte[] DataToBytes(string param, Encoding encoding)
        {
            byte[] bytes = new byte[0];
            bytes = encoding.GetBytes(param);
            return bytes;
        }

        private static string GetResponeString(HttpWebRequest rq, out HttpStatusCode statuscode)
        {
            HttpWebResponse httpWebResponse = null;
            try
            {
                httpWebResponse = (HttpWebResponse)rq.GetResponse();
                statuscode = HttpStatusCode.OK;
            }
            catch (WebException wx)
            {
                httpWebResponse = (HttpWebResponse)wx.Response;
                statuscode = httpWebResponse.StatusCode;
            }
            using (Stream responseStream = httpWebResponse.GetResponseStream())
            using (StreamReader sr = new StreamReader(responseStream))
            {
                string str = sr.ReadToEnd();
                return str;
            }
        }

        private static string CombineQueryUrl(string url, Dictionary<string, string> data)
        {
            string query = GetFormatedData(data);
            if (!url.Contains("?"))
            {
                url += "?" + query;
            }
            else
            {
                url += "&" + query;
            }
            return url;
        }

        private static string GetFormatedData(Dictionary<string, string> data)
        {
            StringBuilder sb = new StringBuilder(200);
            if (data != null && data.Count > 0)
            {
                foreach (var i in data)
                {
                    sb.AppendFormat("{0}={1}&", i.Key, i.Value);
                }
            }
            string rs = sb.ToString();
            if (!string.IsNullOrEmpty(rs))
            {
                rs = rs.TrimEnd('&');
            }
            return rs;
        }

        private static byte[] DataToBytes(Dictionary<string, string> data, Encoding encoding)
        {
            string param = GetFormatedData(data);
            return DataToBytes(param, encoding);
        }

        #endregion Private Function
    }
}
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace hwj.CommonLibrary.WebSite
{
    public class WebLogInfo
    {
        #region Property
        private const string _LogTimeFmt = "DateTime: {0}\r\n";
        private const string _LogServerFmt = "Machine Name: {0}\r\n";
        private const string _LogRequestFmt = "User Agent: {0}\r\n" +
                                              "User Host: {1}\r\n" +
                                              "Url Referrer: {2}\r\n" +
                                              "Page Url: {3}\r\n" +
                                              "Page Path: {4}\r\n" +
                                              "RequestType: {5}\r\n" +
                                              "Params: {6}";
        public string UserAgent { get; set; }
        public string PhysicalPath { get; set; }
        public string Url { get; set; }
        public string UserHostAddress { get; set; }
        public string RequestType { get; set; }
        public string UrlReferrer { get; set; }
        public string Params { get; set; }
        public string ServerMachineName { get; set; }
        public string HTML { get; set; }
        #endregion

        public WebLogInfo()
        {
            Initialization(null);
        }
        public WebLogInfo(HttpRequest request)
        {
            Initialization(request);
        }
        public void Initialization(HttpRequest rq)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("---------------------------------------------------------------------");
            sb.AppendFormat(_LogTimeFmt, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            if (HttpContext.Current != null)
            {
                if (HttpContext.Current.Server != null)
                {
                    HttpServerUtility sr = HttpContext.Current.Server;
                    ServerMachineName = sr.MachineName;
                    sb.AppendFormat(_LogServerFmt, ServerMachineName);
                }
            }

            if (rq != null)
                SetRequest(rq);
            else if (HttpContext.Current != null && HttpContext.Current.Request != null)
                SetRequest(HttpContext.Current.Request);

            sb.AppendFormat(_LogRequestFmt,
                UserAgent,
                UserHostAddress,
                UrlReferrer,
                Url,
                PhysicalPath,
                RequestType,
                Params);

            HTML = sb.ToString();
        }
        private void SetRequest(HttpRequest rq)
        {
            UserAgent = rq.UserAgent;
            PhysicalPath = rq.PhysicalPath;
            UrlReferrer = rq.UrlReferrer == null ? "" : rq.UrlReferrer.ToString();
            UserHostAddress = rq.UserHostAddress;
            RequestType = rq.RequestType;
            Url = rq.Url == null ? "" : rq.Url.ToString().Split('?')[0];
            if (RequestType == "POST")
                Params = rq.Form == null ? "" : rq.Form.ToString();
            else
                Params = rq.QueryString == null ? "" : rq.QueryString.ToString();
        }
    }
}

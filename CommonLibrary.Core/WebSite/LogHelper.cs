using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace hwj.CommonLibrary.WebSite
{
    public abstract class LogHelper : Object.Base.LogHelper
    {
        public LogHelper(string fileName)
        {
            base.Initialization(fileName);
        }

        private string GetWebInfo(string log, WebLogInfo webLogInfo)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(webLogInfo.HTML);
            sb.AppendLine();
            sb.Append(log);
            return sb.ToString();
        }
        private string GetWebInfo(string log, HttpRequest request)
        {
            WebLogInfo w = new WebLogInfo(request);
            StringBuilder sb = new StringBuilder();
            sb.Append(w.HTML);
            sb.AppendLine();
            sb.Append(log);
            return sb.ToString();
        }

        #region Info
        public void InfoWithoutEmail(string log, Exception ex, WebLogInfo webLogInfo)
        {
            base.InfoWithoutEmail(GetWebInfo(log, webLogInfo), ex);
        }

        public void InfoWithEmail(string log, Exception ex, string emailSubject)
        {
            InfoWithEmail(log, ex, emailSubject, null, null);
        }
        public void InfoWithEmail(string log, Exception ex, string emailSubject, string attachmentText, string attachmentFileName)
        {
            InfoWithEmail(log, ex, emailSubject, HttpContext.Current.Request, attachmentText, attachmentFileName);
        }

        public void InfoWithEmail(string log, Exception ex, string emailSubject, HttpRequest request)
        {
            InfoWithEmail(log, ex, emailSubject, request, null, null);
        }
        public void InfoWithEmail(string log, Exception ex, string emailSubject, HttpRequest request, string attachmentText, string attachmentFileName)
        {
            base.InfoWithEmail(GetWebInfo(log, request), ex, emailSubject, attachmentText, attachmentFileName);
        }

        public void InfoWithEmail(string log, Exception ex, string emailSubject, WebLogInfo webLogInfo)
        {
            base.InfoWithEmail(GetWebInfo(log, webLogInfo), ex, emailSubject);
        }
        #endregion

        #region Warn
        public void WarnWithoutEmail(string log, Exception ex, WebLogInfo webLogInfo)
        {
            base.WarnWithoutEmail(GetWebInfo(log, webLogInfo), ex);
        }

        public void WarnWithEmail(string log, Exception ex, string emailSubject)
        {
            WarnWithEmail(log, ex, emailSubject, null, null);
        }
        public void WarnWithEmail(string log, Exception ex, string emailSubject, string attachmentText, string attachmentFileName)
        {
            WarnWithEmail(log, ex, emailSubject, HttpContext.Current.Request, attachmentText, attachmentFileName);
        }

        public void WarnWithEmail(string log, Exception ex, string emailSubject, HttpRequest request)
        {
            WarnWithEmail(log, ex, emailSubject, request, null, null);
        }
        public void WarnWithEmail(string log, Exception ex, string emailSubject, HttpRequest request, string attachmentText, string attachmentFileName)
        {
            base.WarnWithEmail(GetWebInfo(log, request), ex, emailSubject, attachmentText, attachmentFileName);
        }

        public void WarnWithEmail(string log, Exception ex, string emailSubject, WebLogInfo webLogInfo)
        {
            base.WarnWithEmail(GetWebInfo(log, webLogInfo), ex, emailSubject);
        }

        #endregion

        #region Error
        public void ErrorWithoutEmail(string log, Exception ex, WebLogInfo webLogInfo)
        {
            base.ErrorWithoutEmail(GetWebInfo(log, webLogInfo), ex);
        }

        public void ErrorWithEmail(string log, Exception ex, string emailSubject)
        {
            ErrorWithEmail(log, ex, emailSubject, null, null);
        }
        public void ErrorWithEmail(string log, Exception ex, string emailSubject, string attachmentText, string attachmentFileName)
        {
            ErrorWithEmail(log, ex, emailSubject, HttpContext.Current.Request, attachmentText, attachmentFileName);
        }

        public void ErrorWithEmail(string log, Exception ex, string emailSubject, HttpRequest request)
        {
            ErrorWithEmail(log, ex, emailSubject, request, null, null);
        }
        public void ErrorWithEmail(string log, Exception ex, string emailSubject, HttpRequest request, string attachmentText, string attachmentFileName)
        {
            base.ErrorWithEmail(GetWebInfo(log, request), ex, emailSubject, attachmentText, attachmentFileName);
        }

        public void ErrorWithEmail(string log, Exception ex, string emailSubject, WebLogInfo webLogInfo)
        {
            base.ErrorWithEmail(GetWebInfo(log, webLogInfo), ex, emailSubject);
        }
        #endregion
    }
}

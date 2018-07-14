using System;
using System.Web;

namespace hwj.CommonLibrary.WebSite
{
    public class ErrorInfo : Base.BaseEntity<ErrorInfo>
    {
        public enum ErrorTypes
        {
            None,
            Login,
            Unauthorized,
            DefaultException,
            Exception,
            Information,
            Warning,
        }

        #region Property
        public WebLogInfo webLogInfo { get; set; }
        public ErrorTypes ErrorType { get; set; }
        public string Message { get; set; }
        public Exception Exceptions { get; set; }
        public string RedirectUrl { get; set; }
        public HttpRequest ErrorRequest { get; set; }
        public bool SendEmail { get; set; }
        #endregion

        public ErrorInfo()
        {
            ErrorType = ErrorTypes.None;
            Message = string.Empty;
            Exceptions = null;
            RedirectUrl = string.Empty;
            ErrorRequest = null;
            SendEmail = true;
        }
    }

}

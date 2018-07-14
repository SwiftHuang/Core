using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
namespace hwj.CommonLibrary.Object
{
    public class LogHelper : Base.LogHelper
    {
        public LogHelper()
            : this(string.Empty)
        {

        }
        public LogHelper(string fileName)
            : base(fileName)
        {

        }

        //#region Info
        //public void Info(string log, Exception ex, string emailSubject)
        //{
        //    base.InfoWithEmail(log, ex, emailSubject);
        //}
        //public void Info(string log, Exception ex, string emailSubject, string emailTo, string emailCC)
        //{
        //    base.InfoAction(log, ex, emailSubject, emailTo, emailCC);
        //}
        //public void Info(string log, Exception ex, string emailSubject, string emailTo, string emailCC, List<Attachment> attachments)
        //{
        //    base.InfoAction(log, ex, emailSubject, emailTo, emailCC, attachments);
        //}
        //#endregion

        //#region Warn
        //public void Warn(string log, Exception ex, string emailSubject)
        //{
        //    base.WarnWithEmail(log, ex, emailSubject);
        //}
        //public void Warn(string log, Exception ex, string emailSubject, string emailTo, string emailCC)
        //{
        //    base.WarnAction(log, ex, emailSubject, emailTo, emailCC);
        //}
        //public void Warn(string log, Exception ex, string emailSubject, string emailTo, string emailCC, List<Attachment> attachments)
        //{
        //    base.WarnAction(log, ex, emailSubject, emailTo, emailCC, attachments);
        //}
        //#endregion

        //#region Error
        //public void Error(string log, Exception ex, string emailSubject)
        //{
        //    base.ErrorWithEmail(log, ex, emailSubject);
        //}
        //public void Error(string log, Exception ex, string emailSubject, string emailTo, string emailCC)
        //{
        //    base.ErrorAction(log, ex, emailSubject, emailTo, emailCC);
        //}
        //public void Error(string log, Exception ex, string emailSubject, string emailTo, string emailCC, List<Attachment> attachments)
        //{
        //    base.ErrorAction(log, ex, emailSubject, emailTo, emailCC, attachments);
        //}
        //#endregion


    }
}

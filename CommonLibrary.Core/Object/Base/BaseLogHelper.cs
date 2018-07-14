using hwj.CommonLibrary.Object;
using hwj.CommonLibrary.Object.Email;
using log4net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net.Mail;
using System.Text;

namespace hwj.CommonLibrary.Object.Base
{
    public abstract class LogHelper
    {
        #region Property

        private ILog LogInfo { get; set; }

        private ILog LogError { get; set; }

        private ILog LogWarn { get; set; }

        /// <summary>
        /// 是否抛出SmtpList异常
        /// </summary>
        public bool ShowInvalidSmtpError { get; set; }

        public bool EmailOpened { get; set; }

        /// <summary>
        /// 多个收件人请用逗号分隔
        /// </summary>
        public string EmailTo { get; set; }

        /// <summary>
        /// 多个抄送人请用逗号分隔
        /// </summary>
        public string EmailCC { get; set; }

        internal string EmailBodyFormat { get; private set; }

        /// <summary>
        /// 获取或设置是否使用多个SMTP服务器的模式
        /// </summary>
        public bool MultSmtpEnabled { get; set; }

        /// <summary>
        /// 获取或设置SMTP服务器列表
        /// </summary>
        public Email.SmtpInfoList SmtpList { get; set; }

        //public string Subject { get; set; }
        //public string Body { get; set; }

        #endregion Property

        public LogHelper()
            : this(null)
        {
        }

        public LogHelper(string configFileName)
        {
            ShowInvalidSmtpError = true;
            Initialization(configFileName);
        }

        public void Initialization(string configFileName)
        {
            if (!string.IsNullOrEmpty(configFileName))
            {
                log4net.Config.DOMConfigurator.Configure(new FileInfo(configFileName));
            }
            else
            {
                log4net.Config.DOMConfigurator.Configure(new FileInfo("LogHelper.Config"));
            }

            LogInfo = log4net.LogManager.GetLogger("loginfo");
            LogError = log4net.LogManager.GetLogger("logerror");
            LogWarn = log4net.LogManager.GetLogger("logwarn");

            EmailOpened = false;
            EmailTo = "vinsonhwj@qq.com";
            EmailBodyFormat = "{0}\r\n{1}";
        }

        #region Info Function

        public void ChangeInfoOutputFile(string fileName)
        {
            if (LogInfo != null)
            {
                ChangeOutputFile(LogInfo, fileName);
            }
        }

        public void InfoWithoutEmail(string log)
        {
            InfoWithoutEmail(log, null);
        }

        public void InfoWithoutEmail(string log, Exception ex)
        {
            InfoAction(log, ex, false, null, null, null, null);
        }

        public void InfoWithEmail(string log, Exception ex, string emailSubject)
        {
            InfoAction(log, ex, emailSubject, EmailTo, EmailCC, null, null);
        }

        public void InfoWithEmail(string log, Exception ex, string emailSubject, string attachmentText, string attachmentFileName)
        {
            InfoAction(log, ex, emailSubject, EmailTo, EmailCC, attachmentText, attachmentFileName);
        }

        public void InfoAction(string log, Exception ex, string emailSubject, string emailTo, string emailCC, string attachmentText, string attachmentFileName)
        {
            Attachment atch = GetTextAttachment(emailSubject, attachmentText, attachmentFileName);
            InfoAction(log, ex, emailSubject, emailTo, emailCC, new List<Attachment>() { atch });
        }

        public void InfoAction(string log, Exception ex, string emailSubject, string emailTo, string emailCC, List<Attachment> attachments)
        {
            InfoAction(log, ex, true, emailSubject, emailTo, emailCC, attachments);
        }

        private void InfoAction(string log, Exception ex, bool sendEmail, string emailSubject, string emailTo, string emailCC, List<Attachment> attachments)
        {
            if (LogInfo.IsInfoEnabled)
            {
                if (ex == null)
                    LogInfo.Info(log);
                else
                    LogInfo.Info(log, ex);

                if (sendEmail)
                    Email(emailSubject + " <Info>", log, ex, emailTo, emailCC, attachments);
            }
        }

        #endregion Info Function

        #region Warn Function

        public void ChangeWarnOutputFile(string fileName)
        {
            if (LogWarn != null)
            {
                ChangeOutputFile(LogWarn, fileName);
            }
        }

        public void WarnWithoutEmail(string log)
        {
            WarnWithoutEmail(log, null);
        }

        public void WarnWithoutEmail(string log, Exception ex)
        {
            WarnAction(log, ex, false, null, null, null, null, null);
        }

        public void WarnWithEmail(string log, Exception ex, string emailSubject)
        {
            WarnAction(log, ex, emailSubject, EmailTo, EmailCC, null, null);
        }

        public void WarnWithEmail(string log, Exception ex, string emailSubject, string attachmentText, string attachmentFileName)
        {
            WarnAction(log, ex, emailSubject, EmailTo, EmailCC, attachmentText, attachmentFileName);
        }

        public void WarnAction(string log, Exception ex, string emailSubject, string emailTo, string emailCC, string attachmentText, string attachmentFileName)
        {
            Attachment atch = GetTextAttachment(emailSubject, attachmentText, attachmentFileName);
            WarnAction(log, ex, true, emailSubject, emailTo, emailCC, attachmentText, new List<Attachment>() { atch });
        }

        public void WarnAction(string log, Exception ex, string emailSubject, string emailTo, string emailCC, List<Attachment> attachments)
        {
            WarnAction(log, ex, true, emailSubject, emailTo, emailCC, null, attachments);
        }

        private void WarnAction(string log, Exception ex, bool sendEmail, string emailSubject, string emailTo, string emailCC, string attachmentText, List<Attachment> attachments)
        {
            if (LogWarn.IsWarnEnabled)
            {
                string tmpLog = GetTextAttachmentForFileLog(log, attachmentText, ex);
                if (ex == null)
                {
                    LogWarn.Warn(tmpLog);
                }
                else
                {
                    LogWarn.Warn(tmpLog, ex);
                }
                if (sendEmail)
                {
                    Email(emailSubject + " <Warn>", log, ex, emailTo, emailCC, attachments);
                }
            }
        }

        #endregion Warn Function

        #region Error Function

        public void ChangeErrorOutputFile(string fileName)
        {
            if (LogError != null)
            {
                ChangeOutputFile(LogError, fileName);
            }
        }

        public void ErrorWithoutEmail(string log)
        {
            ErrorWithoutEmail(log, null);
        }

        public void ErrorWithoutEmail(string log, Exception ex)
        {
            ErrorAction(log, ex, false, null, null, null, null, null);
        }

        public void ErrorWithEmail(string log, Exception ex, string emailSubject)
        {
            ErrorAction(log, ex, emailSubject, EmailTo, EmailCC, null, null);
        }

        public void ErrorWithEmail(string log, Exception ex, string emailSubject, string attachmentText, string attachmentFileName)
        {
            ErrorAction(log, ex, emailSubject, EmailTo, EmailCC, attachmentText, attachmentFileName);
        }

        public void ErrorAction(string log, Exception ex, string emailSubject, string emailTo, string emailCC, string attachmentText, string attachmentFileName)
        {
            Attachment atch = GetTextAttachment(emailSubject, attachmentText, attachmentFileName);
            ErrorAction(log, ex, true, emailSubject, emailTo, emailCC, attachmentText, new List<Attachment>() { atch });
        }

        public void ErrorAction(string log, Exception ex, string emailSubject, string emailTo, string emailCC, List<Attachment> attachments)
        {
            ErrorAction(log, ex, true, emailSubject, emailTo, emailCC, null, attachments);
        }

        private void ErrorAction(string log, Exception ex, bool sendEmail, string emailSubject, string emailTo, string emailCC, string attachmentText, List<Attachment> attachments)
        {
            if (LogError.IsErrorEnabled)
            {
                string tmpLog = GetTextAttachmentForFileLog(log, attachmentText, ex);
                LogError.Error(tmpLog, ex);

                if (sendEmail)
                {
                    Email(emailSubject + " <Error>", log, ex, emailTo, emailCC, attachments);
                }
            }
        }

        #endregion Error Function

        #region Smtp Function

        public void SetSingleSmtp(string smtpServer, string emailFrom, string emailFromPassword)
        {
            SmtpList = new SmtpInfoList(smtpServer, emailFrom, emailFromPassword);
        }

        #endregion Smtp Function

        #region Email Function

        public void Email(string subject, string log, Exception ex, string emailTo, string emailCC)
        {
            Email(subject, log, ex, emailTo, emailCC, null);
        }

        public void Email(string subject, string log, Exception ex, string emailTo, string emailCC, List<Attachment> attachments)
        {
            if (!string.IsNullOrEmpty(subject))
            {
                if (ex == null)
                    Email(subject, log, emailTo, emailCC, attachments);
                else
                    Email(subject, string.Format(EmailBodyFormat, log, FormatException(ex)), emailTo, emailCC, attachments);
            }
        }

        public void Email(string subject, string body, string emailTo, string emailCC)
        {
            Email(subject, body, emailTo, emailCC, null);
        }

        public void Email(string subject, string body, string emailTo, string emailCC, List<Attachment> attachments)
        {
            //Subject = subject;
            //Body = body;
            if (EmailOpened)
            {
                if (SmtpList != null && SmtpList.Count > 0)
                {
                    if (MultSmtpEnabled)
                    {
                        SmtpInfoList smtpList = SmtpList;
                        EmailHelper.Send(emailTo, emailCC, subject, body, false, attachments, ref smtpList);
                        SmtpList = smtpList;
                    }
                    else
                    {
                        try
                        {
                            EmailHelper.Send(SmtpList[0].SmtpServer, SmtpList[0].UserName, SmtpList[0].Password, emailTo, emailCC, subject, body, false, attachments);
                        }
                        catch
                        {
                            try
                            {
                                EmailHelper.Send(SmtpList[0].SmtpServer, SmtpList[0].UserName, SmtpList[0].Password, emailTo, emailCC, subject, body, false, attachments);
                            }
                            catch (Exception ex)
                            {
                                LogError.Error(ex.Message);
                            }
                        }
                    }
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<<Invaild SmtpList>> Email Body Content:");
                    sb.AppendLine();
                    sb.Append(body);
                    LogWarn.Warn(sb.ToString());

                    if (ShowInvalidSmtpError)
                        throw new Exception("Invaild SmtpList");
                }
            }
        }

        private string FormatException(Exception ex)
        {
            StringBuilder sb = new StringBuilder();

            if (ex.Data != null && ex.Data.Count > 0)
            {
                sb.AppendLine(string.Empty);
                sb.AppendLine("----------------Exception.Data List----------------");
                foreach (DictionaryEntry obj in ex.Data)
                {
                    try
                    {
                        sb.AppendLine(string.Format("Key:[{0}]  Value:[{1}]", obj.Key, obj.Value));
                    }
                    catch
                    {
                        sb.AppendLine("Exception.Data Error");
                    }
                }
            }
            if (ex.GetBaseException() != null)
            {
                sb.AppendLine();
                sb.AppendLine("----------------Base Exception----------------");
                sb.AppendLine(ex.GetBaseException().Message);
                sb.AppendLine(ex.GetBaseException().StackTrace);
            }
            sb.AppendLine();
            sb.AppendLine("----------------Current Exception----------------");
            sb.AppendLine(ex.Message);
            sb.AppendLine(ex.StackTrace);

            return sb.ToString();
        }

        #endregion Email Function

        public static void ChangeOutputFile(ILog iLog, string fileName)
        {
            if (iLog != null)
            {
                log4net.Appender.AppenderCollection ac = ((log4net.Repository.Hierarchy.Logger)iLog.Logger).Appenders;
                for (int i = 0; i < ac.Count; i++)
                {
                    log4net.Appender.RollingFileAppender rfa = ac[i] as log4net.Appender.RollingFileAppender;
                    if (rfa != null)
                    {
                        rfa.File = fileName;
                        rfa.ActivateOptions();
                    }
                }
            }
        }

        private Attachment GetTextAttachment(string emailSubject, string text, string fileName)
        {
            Attachment atch = null;
            if (!string.IsNullOrEmpty(text))
            {
                if (string.IsNullOrEmpty(fileName))
                {
                    fileName = string.Format("{0}_{1}.log", emailSubject, DateTime.Now.ToString("yyyyMMddHHmmss"));
                }
                atch = EmailHelper.GetAttachment(text, fileName, true);
            }
            return atch;
        }

        private string GetTextAttachmentForFileLog(string log, string text, Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(log);
            sb.AppendLine(FormatException(ex));

            if (!string.IsNullOrEmpty(text))
            {
                sb.AppendLine("**** Begin Attachment Text ****");
                sb.AppendLine(text);
                sb.AppendLine("**** End Attachment Text ****");

                return sb.ToString();
            }
            else
            {
                return sb.ToString();
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Net.Mail;
using hwj.CommonLibrary.Object.Email;

namespace hwj.CommonLibrary.Object
{
    public class EmailHelper
    {
        #region 单一SMTP服务器
        /// <summary>
        /// 发送电子邮件
        /// </summary>
        /// <param name="smtpServer">SMTP服务器</param>
        /// <param name="emailFrom">电子邮件的发件人地址</param>
        /// <param name="emailFromPassword">发件人密码（如果该密码为空，则取消验证发件人身份）</param>
        /// <param name="emailTo">收件人的地址</param>
        /// <param name="cc">抄送 (CC) 收件人的地址</param>
        /// <param name="subject">电子邮件的主题行</param>
        /// <param name="body">邮件正文</param>
        /// <param name="isBodyHtml">邮件正文是否为 Html 格式的值</param>
        /// <returns></returns>
        public static bool Send(string smtpServer, string emailFrom, string emailFromPassword, string emailTo, string cc, string subject, string body, bool isBodyHtml)
        {
            return Send(smtpServer, emailFrom, emailFromPassword, emailTo, cc, subject, body, isBodyHtml, null);
        }
        /// <summary>
        /// 发送电子邮件
        /// </summary>
        /// <param name="smtpServer">SMTP服务器</param>
        /// <param name="emailFrom">电子邮件的发件人地址</param>
        /// <param name="emailFromPassword">发件人密码（如果该密码为空，则取消验证发件人身份）</param>
        /// <param name="emailTo">收件人的地址</param>
        /// <param name="cc">抄送 (CC) 收件人的地址</param>
        /// <param name="subject">电子邮件的主题行</param>
        /// <param name="body">邮件正文</param>
        /// <param name="isBodyHtml">邮件正文是否为 Html 格式的值</param>
        /// <param name="attachments">此电子邮件的数据的附件集合</param>
        /// <returns></returns>
        public static bool Send(string smtpServer, string emailFrom, string emailFromPassword, string emailTo, string cc, string subject, string body, bool isBodyHtml, List<Attachment> attachments)
        {
            return Send(smtpServer, emailFrom, emailFromPassword, new string[] { emailTo }, new string[] { cc }, subject, body, isBodyHtml, attachments);
        }
        /// <summary>
        /// 发送含附件的电子邮件（可压缩附件）
        /// </summary>
        /// <param name="smtpServer">SMTP服务器</param>
        /// <param name="emailFrom">电子邮件的发件人地址</param>
        /// <param name="emailFromPassword">发件人密码（如果该密码为空，则取消验证发件人身份）</param>
        /// <param name="emailTo">收件人的地址集合</param>
        /// <param name="cc">抄送 (CC) 收件人的地址集合</param>
        /// <param name="subject">电子邮件的主题行</param>
        /// <param name="body">邮件正文</param>
        /// <param name="isBodyHtml">邮件正文是否为 Html 格式的值</param>
        /// <param name="streams">此电子邮件的数据的附件文件流的集合</param>
        /// <returns></returns>
        //public static bool Send(string smtpServer, string emailFrom, string emailFromPassword, string[] emailTo, string[] cc, string subject, string body, bool isBodyHtml, List<FileStream> streams)
        //{
        //    List<Attachment> attachments = GetAttachments(streams);
        //    return Send(smtpServer, emailFrom, emailFromPassword, emailTo, cc, subject, body, isBodyHtml, attachments);
        //}
        /// <summary>
        /// 发送含附件的电子邮件
        /// </summary>
        /// <param name="smtpServer">SMTP服务器</param>
        /// <param name="emailFrom">电子邮件的发件人地址</param>
        /// <param name="emailFromPassword">发件人密码（如果该密码为空，则取消验证发件人身份）</param>
        /// <param name="emailTo">收件人的地址集合</param>
        /// <param name="cc">抄送 (CC) 收件人的地址集合</param>
        /// <param name="subject">电子邮件的主题行</param>
        /// <param name="body">邮件正文</param>
        /// <param name="isBodyHtml">邮件正文是否为 Html 格式的值</param>
        /// <param name="attachments">此电子邮件的数据的附件集合</param>
        /// <returns></returns>
        public static bool Send(string smtpServer, string emailFrom, string emailFromPassword, string[] emailTo, string[] cc, string subject, string body, bool isBodyHtml, List<Attachment> attachments)
        {
            MailMessage message = GetMailMessage(emailFrom, emailTo, cc, subject, body, isBodyHtml, attachments);
            return Send(smtpServer, emailFrom, emailFromPassword, message);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="smtpServer"></param>
        /// <param name="emailFrom"></param>
        /// <param name="emailFromPassword"></param>
        /// <param name="message"></param>
        /// <param name="isBodyHtml"></param>
        /// <returns></returns>
        public static bool Send(string smtpServer, string emailFrom, string emailFromPassword, MailMessage message)
        {
            return SendAction(smtpServer, emailFrom, emailFromPassword, message);
        }
        #endregion

        #region 多个SMTP服务器
        /// <summary>
        /// 发送电子邮件
        /// </summary>
        /// <param name="emailTo">收件人的地址</param>
        /// <param name="cc">抄送 (CC) 收件人的地址</param>
        /// <param name="subject">电子邮件的主题行</param>
        /// <param name="body">邮件正文</param>
        /// <param name="isBodyHtml">邮件正文是否为 Html 格式的值</param>
        /// <param name="smtpInfos">SMTP服务器集合</param>
        /// <returns></returns>
        public static bool Send(string emailTo, string cc, string subject, string body, bool isBodyHtml, ref SmtpInfoList smtpInfos)
        {
            return Send(emailTo, cc, subject, body, isBodyHtml, null, ref smtpInfos);
        }
        /// <summary>
        /// 发送含附件的电子邮件（可压缩附件）
        /// </summary>
        /// <param name="emailTo">收件人的地址集合</param>
        /// <param name="cc">抄送 (CC) 收件人的地址集合</param>
        /// <param name="subject">电子邮件的主题行</param>
        /// <param name="body">邮件正文</param>
        /// <param name="isBodyHtml">邮件正文是否为 Html 格式的值</param>
        /// <param name="streams">此电子邮件的数据的附件文件流的集合</param>
        /// <param name="smtpInfos">SMTP服务器集合</param>
        /// <returns></returns>
        //public static bool Send(string[] emailTo, string[] cc, string subject, string body, bool isBodyHtml, List<FileStream> streams, ref SmtpInfoList smtpInfos)
        //{
        //    List<Attachment> attachments = GetAttachments(streams);
        //    MailMessage message = GetMailMessage(null, emailTo, cc, subject, body, isBodyHtml, attachments);
        //    return Send(message, ref smtpInfos);
        //}
        /// <summary>
        /// 发送含附件的电子邮件
        /// </summary>
        /// <param name="emailTo">收件人的地址</param>
        /// <param name="cc">抄送 (CC) 收件人的地址</param>
        /// <param name="subject">电子邮件的主题行</param>
        /// <param name="body">邮件正文</param>
        /// <param name="isBodyHtml">邮件正文是否为 Html 格式的值</param>
        /// <param name="attachments">此电子邮件的数据的附件集合</param>
        /// <param name="smtpInfos">SMTP服务器集合</param>
        /// <returns></returns>
        public static bool Send(string emailTo, string cc, string subject, string body, bool isBodyHtml, List<Attachment> attachments, ref SmtpInfoList smtpInfos)
        {
            return Send(new string[] { emailTo }, new string[] { cc }, subject, body, isBodyHtml, attachments, ref smtpInfos);
        }
        /// <summary>
        /// 发送含附件的电子邮件
        /// </summary>
        /// <param name="emailTo">>收件人的地址集合</param>
        /// <param name="cc">抄送 (CC) 收件人的地址集合</param>
        /// <param name="subject">电子邮件的主题行</param>
        /// <param name="body">邮件正文</param>
        /// <param name="isBodyHtml">邮件正文是否为 Html 格式的值</param>
        /// <param name="attachments">此电子邮件的数据的附件集合</param>
        /// <param name="smtpInfos">SMTP服务器集合</param>
        /// <returns></returns>
        public static bool Send(string[] emailTo, string[] cc, string subject, string body, bool isBodyHtml, List<Attachment> attachments, ref SmtpInfoList smtpInfos)
        {
            MailMessage message = GetMailMessage(null, emailTo, cc, subject, body, isBodyHtml, attachments);
            return Send(message, ref smtpInfos);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="isBodyHtml">邮件正文是否为 Html 格式的值</param>
        /// <param name="smtpInfos">SMTP服务器集合</param>
        /// <returns></returns>
        public static bool Send(MailMessage message, ref SmtpInfoList smtpInfos)
        {
            if (smtpInfos == null)
                throw new Exception("Invalid Data: SmtpInfos is null");

            if (smtpInfos.LastSuccess != null)
            {
                SmtpInfo smtp = SendAction(message, smtpInfos.LastSuccess);
                if (smtp.Exception == null)
                {
                    return true;
                }
                else
                {
                    smtpInfos.LastSuccess = null;
                }
            }

            SmtpInfoList streamlineList = smtpInfos.GetValidList();

            foreach (SmtpInfo smtpInfo in streamlineList)
            {
                SendAction(message, smtpInfo);
                if (smtpInfo.Exception == null)
                {
                    smtpInfos.LastSuccess = smtpInfo;
                    return true;
                }
                else
                {
                    if (smtpInfo.Equals(streamlineList[streamlineList.Count - 1]))
                    {
                        //throw smtpInfos.LastFailed.Exception;
                        return false;
                    }
                }
            }
            return false;
        }
        #endregion

        #region Check Email
        public static bool isValidEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return true;

            bool myIsEmail = false;
            string myRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(myRegex);
            if (reg.IsMatch(email))
            {
                myIsEmail = true;
            }
            return myIsEmail;
        }

        public static bool isValidEmail(string emails, string split, out List<string> invalidList)
        {
            invalidList = new List<string>();

            if (string.IsNullOrEmpty(emails))
                return true;

            if (!string.IsNullOrEmpty(split))
            {
                string[] emailList = emails.Split(new string[] { split }, StringSplitOptions.None);

                foreach (string email in emailList)
                {
                    if (!isValidEmail(email))
                    {
                        invalidList.Add(email);
                    }
                }
            }
            else
            {
                if (!isValidEmail(emails))
                {
                    invalidList.Add(emails);
                }
            }

            return invalidList.Count == 0;
        }
        #endregion

        #region Private Function
        private static SmtpInfo SendAction(MailMessage message, SmtpInfo smtpInfo)
        {
            try
            {
                SendAction(smtpInfo.SmtpServer, smtpInfo.UserName, smtpInfo.Password, message);
                smtpInfo.Exception = null;
                return smtpInfo;
            }
            catch (Exception ex)
            {
                smtpInfo.FailedOn = DateTime.Now;
                smtpInfo.Exception = ex;
                return smtpInfo;
            }
        }
        private static bool SendAction(string smtpServer, string emailFrom, string emailFromPassword, MailMessage message)
        {
            SmtpClient client = new SmtpClient(smtpServer);
            if (!string.IsNullOrEmpty(emailFromPassword))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(emailFrom, emailFromPassword);
            }
            else
                client.UseDefaultCredentials = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;

            message.From = new MailAddress(emailFrom);

            client.Send(message);
            return true;
        }

        private static MailMessage GetMailMessage(string emailFrom, string[] emailTo, string[] cc, string subject, string body, bool isBodyHtml, List<Attachment> attachments)
        {
            MailMessage message = new MailMessage();
            message.Subject = subject;
            message.Body = body;

            if (emailTo != null)
            {
                foreach (string to in emailTo)
                {
                    if (!string.IsNullOrEmpty(to))
                        message.To.Add(to);
                }
            }
            if (cc != null)
            {
                foreach (string c in cc)
                {
                    if (!string.IsNullOrEmpty(c))
                        message.CC.Add(c);
                }
            }

            if (attachments != null && attachments.Count > 0)
            {
                foreach (Attachment a in attachments)
                {
                    if (a != null)
                    {
                        message.Attachments.Add(a);
                    }
                }
            }

            if (!string.IsNullOrEmpty(emailFrom))
            {
                message.From = new MailAddress(emailFrom);
            }
            message.Priority = MailPriority.Normal;
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.IsBodyHtml = isBodyHtml;

            return message;
        }
        //private static List<Attachment> GetAttachments(List<FileStream> streams)
        //{
        //    List<Attachment> attachments = new List<Attachment>();

        //    //string HTML = @"<br><br>To uncompress the file you may need one of follow softwares <br>winrar:<a href=http://www.winrar.com>http://www.winrar.com</a><br>7-zip:<a href=http://www.7-zip.org/>http://www.7-zip.org</a>";
        //    //string STR = "\r\n\r\nTo uncompress the file you may need one of follow softwares \r\nwinrar:http://www.winrar.com \r\n7-zip: http://www.7-zip.org/";
        //    //bool usedGzip = false;

        //    if (streams != null && streams.Count > 0)
        //    {
        //        foreach (FileStream s in streams)
        //        {
        //            if (s != null)
        //            {
        //                if (s.UseGzip)
        //                {
        //                    attachments.Add(new Attachment(hwj.CommonLibrary.Object.FileHelper.Stream2GzipStream(s.Stream), s.FileName + ".gz"));
        //                    //usedGzip = true;
        //                }
        //                else
        //                    attachments.Add(new Attachment(s.Stream, s.FileName));
        //            }
        //        }
        //    }
        //    return attachments;
        //}
        public static Attachment GetAttachment(string text, string FileName, bool useGzip)
        {
            if (!string.IsNullOrEmpty(text))
            {
                if (useGzip)
                {
                    return new Attachment(hwj.CommonLibrary.Object.TextHelper.StringToMemoryStream(text, true), FileName + ".gz");
                }
                else
                {
                    Byte[] buffer = System.Text.Encoding.UTF8.GetBytes(text);
                    return new Attachment(hwj.CommonLibrary.Object.TextHelper.BytesToStream(buffer), FileName);
                }
            }
            else
                return null;
        }
        public static Attachment GetAttachment(System.IO.Stream stream, string FileName, bool useGzip)
        {
            if (stream != null)
            {
                if (useGzip)
                {
                    return new Attachment(hwj.CommonLibrary.Object.TextHelper.StreamToMemoryStream(stream, true), FileName + ".gz");
                }
                else
                    return new Attachment(stream, FileName);
            }
            return null;
        }

        #endregion
    }

}

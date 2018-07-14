using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;

namespace hwj.CommonLibrary.Object
{
    public class FTPHelper
    {
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="ftp"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <param name="fileName">文件名(含路径)</param>
        public static void Upload(string ftp, string user, string password, string fileName)
        {
            FileInfo fileInf = new FileInfo(fileName);
            string uri = ftp + "/" + fileInf.Name;
            FtpWebRequest reqFTP = InitFtpWebRequest(uri, user, password, WebRequestMethods.Ftp.UploadFile);

            FtpActionForFileInfo(reqFTP, fileInf);
        }
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="ftp"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <param name="stream">文件流</param>
        /// <param name="fileName">文件名(不含路径)</param>
        public static void Upload(string ftp, string user, string password, Stream stream, string fileName)
        {
            string uri = ftp + "/" + fileName;
            FtpWebRequest reqFTP = InitFtpWebRequest(uri, user, password, WebRequestMethods.Ftp.UploadFile);

            FtpActionForStream(reqFTP, stream, fileName);
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="ftp"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <param name="fileName">文件名(不含路径)</param>
        public static void Delete(string ftp, string user, string password, string fileName)
        {
            string uri = ftp + "/" + fileName;
            FtpWebRequest reqFTP = InitFtpWebRequest(uri, user, password, WebRequestMethods.Ftp.DeleteFile);

            FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
            //Console.WriteLine("Delete status: {0}", response.StatusDescription);
            response.Close();

        }
        /// <summary>
        /// 检查文件是否存在
        /// </summary>
        /// <param name="ftp"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <param name="fileName">文件名(不含路径)</param>
        /// <returns></returns>
        public static bool ExistFile(string ftp, string user, string password, string fileName)
        {
            string uri = ftp + "/" + fileName;
            FtpWebRequest reqFTP = InitFtpWebRequest(uri, user, password, WebRequestMethods.Ftp.GetFileSize);
            try
            {
                FtpWebResponse ftpresponse = (FtpWebResponse)reqFTP.GetResponse();
                return true;
            }
            catch (WebException ex)
            {
                FtpWebResponse wr = (FtpWebResponse)ex.Response;
                return false;
            }
            catch
            {
                return false;
            }
        }

        #region Private Function
        private static FtpWebRequest InitFtpWebRequest(string uri, string user, string password, string method)
        {
            return InitFtpWebRequest(uri, user, password, false, method, true);
        }
        private static FtpWebRequest InitFtpWebRequest(string uri, string user, string password, bool keepLive, string method, bool useBinary)
        {
            FtpWebRequest reqFTP;

            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));//根据uri创建FtpWebRequest对象 
            reqFTP.Credentials = new NetworkCredential(user, password);//ftp用户名和密码
            reqFTP.KeepAlive = keepLive;//默认为true，连接不会被关闭(在一个命令之后被执行)
            reqFTP.Method = method;//指定执行什么命令
            reqFTP.UseBinary = useBinary;//指定数据传输类型

            return reqFTP;
        }

        private static void FtpActionForFileInfo(FtpWebRequest reqFTP, FileInfo fileInf)
        {
            int contentLen;
            int buffLength = 2048;// 缓冲大小设置为2kb
            byte[] buff = new byte[buffLength];

            FileStream fs = fileInf.OpenRead();// 打开一个文件流 (System.IO.FileStream) 去读上传的文件
            contentLen = fs.Read(buff, 0, buffLength);// 每次读文件流的2kb

            reqFTP.ContentLength = fileInf.Length;// 上传文件时通知服务器文件的大小
            Stream strm = reqFTP.GetRequestStream();// 把上传的文件写入流

            // 流内容没有结束
            while (contentLen != 0)
            {
                strm.Write(buff, 0, contentLen);// 把内容从file stream 写入 upload stream
                contentLen = fs.Read(buff, 0, buffLength);
            }

            // 关闭两个流
            strm.Close();
            fs.Close();
        }
        private static void FtpActionForStream(FtpWebRequest reqFTP, Stream stream, string fileName)
        {
            // 缓冲大小设置为2kb
            int buffLength = 2048;
            byte[] buff = new byte[buffLength];
            int contentLen;

            //上传文件时通知服务器文件的大小
            reqFTP.ContentLength = stream.Length;

            // 把上传的文件写入流
            Stream strm = reqFTP.GetRequestStream();

            // 每次读文件流的2kb
            contentLen = stream.Read(buff, 0, buffLength);

            // 流内容没有结束
            while (contentLen != 0)
            {
                // 把内容从file stream 写入 upload stream
                strm.Write(buff, 0, contentLen);
                contentLen = stream.Read(buff, 0, buffLength);
            }

            // 关闭两个流
            strm.Close();
            stream.Close();
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;

namespace hwj.CommonLibrary.Object
{
    /// <summary>
    /// 文本类
    /// </summary>
    public class TextHelper
    {
        /// <summary>
        /// 将 Stream 转成 byte[]
        /// </summary>
        public static byte[] StreamToBytes(Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);

            // 设置当前流的位置为流的开始
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        }
        /// <summary>
        /// 将 byte[] 转成 Stream
        /// </summary>
        public static Stream BytesToStream(byte[] bytes)
        {
            Stream stream = new MemoryStream(bytes);
            return stream;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static MemoryStream StreamToMemoryStream(Stream stream)
        {
            return StreamToMemoryStream(stream, false);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="useGZip">使用GZip压缩</param>
        /// <returns></returns>
        public static MemoryStream StreamToMemoryStream(Stream stream, bool useGZip)
        {
            Byte[] buffer = StreamToBytes(stream);
            return BytesToMemoryStream(buffer, useGZip);
        }
        /// <summary>
        /// 字符串转为流(用Encoding.UTF8)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static MemoryStream StringToMemoryStream(string data)
        {
            return StringToMemoryStream(data, false);
        }
        /// <summary>
        /// 字符串转为流(用Encoding.UTF8)
        /// </summary>
        /// <param name="data"></param>
        /// <param name="useGZip">使用GZip压缩</param>
        /// <returns></returns>
        public static MemoryStream StringToMemoryStream(string data, bool useGZip)
        {
            Byte[] buffer = Encoding.UTF8.GetBytes(data);
            return BytesToMemoryStream(buffer, useGZip);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static MemoryStream BytesToMemoryStream(Byte[] buffer)
        {
            return BytesToMemoryStream(buffer, false);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="useGZip">使用GZip压缩</param>
        /// <returns></returns>
        public static MemoryStream BytesToMemoryStream(Byte[] buffer, bool useGZip)
        {
            MemoryStream ms = new MemoryStream();

            if (useGZip)
            {
                GZipStream zipStream = new GZipStream(ms, CompressionMode.Compress, true);
                zipStream.Write(buffer, 0, buffer.Length);
                zipStream.Close();
            }
            else
            {
                ms.Write(buffer, 0, buffer.Length);
            }

            ms.Position = 0;
            return ms;
        }

        public static string GetMD5Key(string data)
        {
            return GetMD5Key(StringToMemoryStream(data));
        }
        public static string GetMD5Key(Stream InputStream)
        {
            StringBuilder sb = new StringBuilder();

            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] OutBytes = md5.ComputeHash(InputStream);
            InputStream.Close();
            for (int i = 0; i < OutBytes.Length; i++)
            {
                sb.Append(OutBytes[i].ToString("x2"));
            }

            return sb.ToString();
        }
        //public static Stream Decompress(Stream stream)
        //{
        //    MemoryStream ms = new MemoryStream();
        //    GZipStream zipStream = new GZipStream(stream, CompressionMode.Decompress);

        //    using (StreamReader reader = new StreamReader(zipStream))
        //    {

        //        string tmp = reader.ReadToEnd();
        //        ms = StringToMemoryStream(tmp);
        //    }
        //    zipStream.Close();
        //    return ms;
        //}
    }
}

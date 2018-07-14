using System;
using System.Collections.Generic;
using System.Text;

namespace hwj.CommonLibrary.Object
{
    public class WebServiceResult : ErrorMsg
    {
        /// <summary>
        /// 扩展参数1
        /// </summary>
        public string Ext1 { get; set; }
        /// <summary>
        /// 扩展参数2
        /// </summary>
        public string Ext2 { get; set; }
        /// <summary>
        /// 扩展参数3
        /// </summary>
        public string Ext3 { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public string Version { get; set; }


        public WebServiceResult():base()
        {

            Ext1 = string.Empty;
            Ext2 = string.Empty;
            Ext3 = string.Empty;
            Version = string.Empty;
        }
        public WebServiceResult FromXml(string xml)
        {
            return SerializationHelper.FromXml<WebServiceResult>(xml);
        }
        public string ToXml()
        {
            return SerializationHelper.SerializeToXml(this);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace hwj.CommonLibrary.Object
{
    public class ReturnObject
    {
        #region Property

        /// <summary>
        /// 是否错误
        /// </summary>
        public bool IsError { get; set; }
        Error _FirstError = null;
        /// <summary>
        /// 获取首个Error对象
        /// </summary>
        public Error FirstError
        {
            get
            {
                if (ErrorList != null && ErrorList.Count > 0)
                {
                    return ErrorList[0];
                }
                else
                {
                    return null;
                }
            }
            set { _FirstError = value; }
        }
        List<Error> _ErrorList = new List<Error>();
        /// <summary>
        /// 错误列表
        /// </summary>
        public List<Error> ErrorList
        {
            get { return _ErrorList; }
            set
            {
                _ErrorList = value;
                if (_ErrorList != null && _ErrorList.Count > 0)
                {
                    IsError = true;
                }
                else
                {
                    IsError = false;
                }
            }
        }
        #endregion

        public ReturnObject()
        {
            IsError = false;

            ErrorList = new List<Error>();
        }

        #region Public Function
        public void Add(string message)
        {
            Add(string.Empty, message, null);
        }
        public void Add(string code, string message)
        {
            Add(code, message, null);
        }
        public void Add(string code, string message, Exception exception)
        {
            if (string.IsNullOrEmpty(code) && string.IsNullOrEmpty(message) && exception == null)
                return;

            if (ErrorList == null)
                ErrorList = new List<Error>();
            ErrorList.Add(new Error(code, message, exception));

            IsError = ErrorList.Count > 0;
        }

        //public ReturnObject FromXml(string xml)
        //{
        //    return SerializationHelper.FromXml<ReturnObject>(xml);
        //}
        //public string ToXml()
        //{
        //    return SerializationHelper.SerializeToXml(this);
        //}
        #endregion

        #region Error Class
        public class Error
        {
            /// <summary>
            /// 错误编号
            /// </summary>
            public string Code { get; set; }
            /// <summary>
            /// 错误信息
            /// </summary>
            public string Message { get; set; }
            /// <summary>
            /// 错误对象
            /// </summary>
            [XmlIgnore()]
            public Exception Exception { get; private set; }

            public Error()
                : this(string.Empty, string.Empty, null)
            {

            }

            public Error(string code, string message)
                : this(code, message, null)
            {

            }
            public Error(string code, string message, Exception exception)
            {
                Code = code;
                Message = message;
                this.Exception = exception;
            }

            public string ToString()
            {
                return string.Format("{0}-{1}", Code, Message);
            }
        }
        #endregion
    }

    public class WSReturnObject : ReturnObject
    {
        #region Property
        /// <summary>
        /// 版本号
        /// </summary>
        public string Version { get; set; }

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
        #endregion

        public WSReturnObject()
            : base()
        {
            Ext1 = string.Empty;
            Ext2 = string.Empty;
            Ext3 = string.Empty;
            Version = string.Empty;
        }
    }
}

namespace hwj.CommonLibrary.Object.Extensions
{
    public static class ExStringHelper
    {
        /// <summary>
        /// 从当前 System.String 对象的开始和末尾移除所有空白字符并转大写，如Null则返回Null。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToUpperTrim(this string str)
        {
            if (str == null)
            {
                return str;
            }
            return str.ToUpper().Trim();
        }

        /// <summary>
        /// 从当前 System.String 对象的开始和末尾移除所有空白字符并转大写，如Null则返回string.Empty。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToUpperTrimEmpty(this string str)
        {
            if (str == null)
            {
                return string.Empty;
            }
            return str.ToUpper().Trim();
        }

        /// <summary>
        /// 从当前 System.String 对象的开始和末尾移除所有空白字符并转小写，如Null则返回Null。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToLowerTrim(this string str)
        {
            if (str == null)
            {
                return str;
            }
            return str.ToLower().Trim();
        }

        /// <summary>
        /// 从当前 System.String 对象的开始和末尾移除所有空白字符并转小写，如Null则返回string.Empty。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToLowerTrimEmpty(this string str)
        {
            if (str == null)
            {
                return string.Empty;
            }
            return str.ToLower().Trim();
        }

        /// <summary>
        /// 从当前 System.String 对象的开始和末尾移除所有空白字符，如Null则返回Null。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string TryTrim(this string str)
        {
            if (str == null)
            {
                return str;
            }
            return str.Trim();
        }

        /// <summary>
        /// 从当前 System.String 对象的开始和末尾移除所有空白字符，如Null则返回string.Empty。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string TryTrimEmpty(this string str)
        {
            if (str == null)
            {
                return string.Empty;
            }
            return str.Trim();
        }

        /// <summary>
        /// 检查当前 System.String 对象是否数字.(通过正则检查)
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNumber(this string str)
        {
            if(!string.IsNullOrEmpty(str))
            {
                return NumberHelper.IsNumeric(str);
            }
            return false;
        }
    }
}
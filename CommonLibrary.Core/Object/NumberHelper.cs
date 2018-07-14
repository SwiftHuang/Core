using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace hwj.CommonLibrary.Object
{
    public class NumberHelper
    {
        public static string ToString(object value)
        {
            return ToString(value, null);
        }

        public static string ToString(object value, string format)
        {
            if (!string.IsNullOrEmpty(format))
                return decimal.Parse(value.ToString()).ToString(format);
            else
                return decimal.Parse(value.ToString()).ToString("##0.00");
        }

        /// <summary>
        /// 是否数字(建议使用TryParse)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNumeric(string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;
            return Regex.IsMatch(value, @"^[+-]?\d*[.]?\d*$");
        }

        /// <summary>
        /// 是否整数(建议使用TryParse)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsInt(string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;
            return Regex.IsMatch(value, @"^[+-]?\d*$");
        }

        public static bool IsUnsign(string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;
            return Regex.IsMatch(value, @"^\d*[.]?\d*$");
        }

        /// <summary>
        /// 四舍五入(向上取整 2.5->3/2.4->2)
        /// </summary>
        /// <param name="valueToRound"></param>
        /// <returns></returns>
        public static double RoundUp(double valueToRound)
        {
            return (Math.Floor(valueToRound + 0.5));
        }

        /// <summary>
        /// 四舍五入(向上取整 2.5->3/2.4->2)
        /// </summary>
        /// <param name="valueToRound"></param>
        /// <returns></returns>
        public static decimal RoundUp(decimal valueToRound)
        {
            return (Math.Floor(valueToRound + decimal.Parse("0.5")));
        }

        /// <summary>
        /// 向下取整(2.5->2/2.4->2)
        /// </summary>
        /// <param name="valueToRound"></param>
        /// <returns></returns>
        public static double RoundDown(double valueToRound)
        {
            double floorValue = Math.Floor(valueToRound);
            if ((valueToRound - floorValue) > .5)
                return (floorValue + 1);
            else
                return floorValue;
        }

        /// <summary>
        /// 向下取整(2.5->2/2.4->2)
        /// </summary>
        /// <param name="valueToRound"></param>
        /// <returns></returns>
        public static decimal RoundDown(decimal valueToRound)
        {
            decimal floorValue = Math.Floor(valueToRound);
            if ((valueToRound - floorValue) > decimal.Parse("0.5"))
                return (floorValue + 1);
            else
                return floorValue;
        }

        /// <summary>
        /// 取整数(不进行四舍五入,只取整数)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal GetInteger(decimal value)
        {
            return Math.Truncate(value);
        }

        /// <summary>
        /// 取整数(不进行四舍五入,只取整数)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double GetInteger(double value)
        {
            return Math.Truncate(value);
        }

        /// <summary>
        /// 是否偶数值
        /// </summary>
        /// <param name="intValue"></param>
        /// <returns></returns>
        public static bool IsEven(int value)
        {
            return ((value % 2) == 0);
        }

        /// <summary>
        /// 是否奇数值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsOdd(int value)
        {
            return ((value % 2) == 1);
        }

        private static Function.DecimalToChinese dtc = new hwj.CommonLibrary.Function.DecimalToChinese();

        /// <summary>
        /// 将阿拉伯数字转为大写中文数字
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetChinese(decimal value)
        {
            return dtc.ConvertToChinese(Math.Round(value, 2), false);
        }

        /// <summary>
        /// 将阿拉伯数字转为英文
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetEnglish(decimal value)
        {
            return hwj.CommonLibrary.Function.DecimalToEnglish.HKMoneytoEng(value);
        }

        /// <summary>
        /// 计算断号
        /// </summary>
        /// <param name="CurrentSeqList">当前的序号列表</param>
        /// <param name="MinSeqNum">最小序号</param>
        /// <param name="MaxSeqNum">最大序号</param>
        /// <returns></returns>
        public static List<int> GetBreakSeqNum(List<int> CurrentSeqList, int MinSeqNum, int MaxSeqNum)
        {
            return GetBreakSeqNum(CurrentSeqList, MinSeqNum, MaxSeqNum, 0);
        }

        /// <summary>
        /// 计算断号
        /// </summary>
        /// <param name="CurrentSeqList">序号列表</param>
        /// <param name="MinSeqNum">最小序号</param>
        /// <param name="MaxSeqNum">最大序号</param>
        /// <param name="top">返回个数</param>
        /// <returns></returns>
        public static List<int> GetBreakSeqNum(List<int> CurrentSeqList, int MinSeqNum, int MaxSeqNum, int top)
        {
            int index = 0;
            List<int> lst = new List<int>();
            CurrentSeqList.Sort();
            for (int i = MinSeqNum; i <= MaxSeqNum; i++)
            {
                if (CurrentSeqList.BinarySearch(i) < 0)
                {
                    lst.Add(i);
                    if (top > 0)
                    {
                        index++;
                        if (index == top)
                            return lst;
                    }
                }
            }
            return lst;
        }
    }
}
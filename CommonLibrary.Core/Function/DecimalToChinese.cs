using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace hwj.CommonLibrary.Function
{
    internal class DecimalToChinese
    {
        //1 常量的规定
        /// <summary>
        /// 数位
        /// </summary>
        public enum NumLevel { Cent, Chiao, Yuan, Ten, Hundred, Thousand, TenThousand, hundredMillon, Trillion };

        /// <summary>
        /// 数位的指数
        /// </summary>
        private int[] NumLevelExponent = new int[] { -2, -1, 0, 1, 2, 3, 4, 8, 12 };

        /// <summary>
        /// 数位的中文字符
        /// </summary>
        private string[] NumLeverChineseSign = new string[] { "分", "角", "元", "拾", "佰", "仟", "万", "亿", "兆" };

        /// <summary>
        /// 大写字符
        /// </summary>
        private string[] NumChineseCharacter = new string[] { "零", "壹", "贰", "叁", "肆", "伍", "陆", "柒", "捌", "玖" };

        /// <summary>
        /// 整(当没有 角分 时)
        /// </summary>
        private const string EndOfInt = "整";


        //2：数字合法性验证，采用正则表达式验证
        /// <summary>
        /// 正则表达验证数字是否合法
        /// </summary>
        /// <param name="Num"></param>
        /// <returns></returns>
        public bool IsValidated<T>(T Num)
        {
            Regex reg = new Regex(@"^(([0])|([1-9]\d{0,23}))(\.\d{1,2})?$");
            if (reg.IsMatch(Num.ToString()))
            {
                return true;
            }
            return false;
        }


        //3： 获取数位 例如 1000的数位为 NumLevel.Thousand
        /// <summary>
        /// 获取数字的数位　使用log
        /// </summary>
        /// <param name="Num"></param>
        /// <returns></returns>
        private NumLevel GetNumLevel(double Num)
        {
            double numLevelLength;
            NumLevel NLvl = new NumLevel();
            if (Num > 0)
            {
                numLevelLength = Math.Floor(Math.Log10(Num));
                for (int i = NumLevelExponent.Length - 1; i >= 0; i--)
                {
                    if (numLevelLength >= NumLevelExponent[i])
                    {
                        NLvl = (NumLevel)i;
                        break;
                    }
                }
            }
            else
            {
                NLvl = NumLevel.Yuan;
            }
            return NLvl;

        }


        //4：判断数字之间是否有跳位，也就是中文中间是否要加零，例如1020 就应该加零。
        /// <summary>
        /// 是否跳位
        /// </summary>
        /// <returns></returns>
        private bool IsDumpLevel(double Num)
        {
            if (Num > 0)
            {
                NumLevel? currentLevel = GetNumLevel(Num);
                NumLevel? nextLevel = null;
                int numExponent = this.NumLevelExponent[(int)currentLevel];

                double postfixNun = Math.Round(Num % (Math.Pow(10, numExponent)), 2);
                if (postfixNun > 0)
                    nextLevel = GetNumLevel(postfixNun);
                if (currentLevel != null && nextLevel != null)
                {
                    if (currentLevel > nextLevel + 1)
                    {
                        return true;
                    }
                }
            }
            return false;

        }

        //5 把长数字分割为两个较小的数字数组，例如把9999亿兆，分割为9999亿和0兆，因为计算机不支持过长的数字。
        /// <summary>
        /// 是否大于兆，如果大于就把字符串分为两部分，
        /// 一部分是兆以前的数字
        /// 另一部分是兆以后的数字
        /// </summary>
        /// <param name="Num"></param>
        /// <returns></returns>
        private bool IsBigThanTillion(string Num)
        {
            bool isBig = false;
            if (Num.IndexOf('.') != -1)
            {
                //如果大于兆
                if (Num.IndexOf('.') > NumLevelExponent[(int)NumLevel.Trillion])
                {
                    isBig = true;
                }
            }
            else
            {
                //如果大于兆
                if (Num.Length > NumLevelExponent[(int)NumLevel.Trillion])
                {
                    isBig = true;
                }
            }
            return isBig;
        }

        /// <summary>
        /// 把数字字符串由‘兆’分开两个
        /// </summary>
        /// <returns></returns>
        private double[] SplitNum(string Num)
        {
            //兆的开始位
            double[] TillionLevelNums = new double[2];
            int trillionLevelLength;
            if (Num.IndexOf('.') == -1)
                trillionLevelLength = Num.Length - NumLevelExponent[(int)NumLevel.Trillion];
            else
                trillionLevelLength = Num.IndexOf('.') - NumLevelExponent[(int)NumLevel.Trillion];
            //兆以上的数字
            TillionLevelNums[0] = Convert.ToDouble(Num.Substring(0, trillionLevelLength));
            //兆以下的数字
            TillionLevelNums[1] = Convert.ToDouble(Num.Substring(trillionLevelLength));
            return TillionLevelNums;
        }



        //6 是否以“壹拾”开头，如果是就可以把它变为“拾”
        private bool IsStartOfTen(double Num)
        {
            bool isStartOfTen = false;
            while (Num >= 10)
            {
                if (Num == 10)
                {
                    isStartOfTen = true;
                    break;
                }
                //Num的数位
                NumLevel currentLevel = GetNumLevel(Num);
                int numExponent = this.NumLevelExponent[(int)currentLevel];
                Num = Convert.ToInt32(Math.Floor(Num / Math.Pow(10, numExponent)));
                if (currentLevel == NumLevel.Ten && Num == 1)
                {
                    isStartOfTen = true;
                    break;
                }
            }
            return isStartOfTen;
        }


        //7 合并大于兆连个数组转化成的货币字符串
        /// <summary>
        /// 合并分开的数组中文货币字符
        /// </summary>
        /// <param name="tillionNums"></param>
        /// <returns></returns>
        private string ContactNumChinese(double[] tillionNums)
        {
            string uptillionStr = CalculateChineseSign(tillionNums[0], NumLevel.Trillion, true, IsStartOfTen(tillionNums[0]));
            string downtrillionStr = CalculateChineseSign(tillionNums[1], null, true, false);
            string chineseCharactor = string.Empty;
            //分开后的字符是否有跳位
            if (GetNumLevel(tillionNums[1] * 10) == NumLevel.Trillion)
            {
                chineseCharactor = uptillionStr + NumLeverChineseSign[(int)NumLevel.Trillion] + downtrillionStr;
            }
            else
            {
                chineseCharactor = uptillionStr + NumLeverChineseSign[(int)NumLevel.Trillion];
                if (downtrillionStr != "零元整")
                {
                    chineseCharactor += NumChineseCharacter[0] + downtrillionStr;
                }
                else
                {
                    chineseCharactor += "元整";
                }
            }
            return chineseCharactor;

        }


        //8：递归计算货币数字的中文
        /// <summary>
        /// 计算中文字符串
        /// </summary>
        /// <param name="Num">数字</param>
        /// <param name="NL">数位级别 比如1000万的 数位级别为万</param>
        /// <param name="IsExceptTen">是否以‘壹拾’开头</param>
        /// <returns>中文大写</returns>
        public string CalculateChineseSign(double Num, NumLevel? NL, bool IsDump, bool IsExceptTen)
        {
            Num = Math.Round(Num, 2);
            bool isDump = false;
            //Num的数位
            NumLevel? currentLevel = GetNumLevel(Num);
            int numExponent = this.NumLevelExponent[(int)currentLevel];

            string Result = string.Empty;

            //整除后的结果
            int prefixNum;
            //余数 当为小数的时候 分子分母各乘100
            double postfixNun;
            if (Num >= 1)
            {
                prefixNum = Convert.ToInt32(Math.Floor(Num / Math.Pow(10, numExponent)));
                postfixNun = Math.Round(Num % (Math.Pow(10, numExponent)), 2);
            }
            else
            {
                prefixNum = Convert.ToInt32(Math.Floor(Num * 100 / Math.Pow(10, numExponent + 2)));
                postfixNun = Math.Round(Num * 100 % (Math.Pow(10, numExponent + 2)), 2);
                postfixNun *= 0.01;
            }

            if (prefixNum < 10)
            {
                //避免以‘壹拾’开头
                if (!(NumChineseCharacter[(int)prefixNum] == NumChineseCharacter[1]
                && currentLevel == NumLevel.Ten && IsExceptTen))
                {
                    Result += NumChineseCharacter[(int)prefixNum];
                }
                else
                {
                    IsExceptTen = false;
                }
                //加上单位
                if (currentLevel == NumLevel.Yuan)
                {
                    ////当为 “元” 位不为零时 加“元”。
                    if (NL == null)
                    {
                        Result += NumLeverChineseSign[(int)currentLevel];
                        //当小数点后为零时 加 "整"
                        if (postfixNun == 0)
                        {
                            Result += EndOfInt;
                        }
                    }
                }
                else
                {
                    Result += NumLeverChineseSign[(int)currentLevel];
                }
                //当真正的个位为零时　加上“元”
                if (NL == null && postfixNun < 1 && currentLevel > NumLevel.Yuan && postfixNun > 0)
                {
                    Result += NumLeverChineseSign[(int)NumLevel.Yuan];

                }


            }
            else
            {
                //当 前缀数字未被除尽时， 递归下去
                NumLevel? NextNL = null;
                if ((int)currentLevel >= (int)(NumLevel.TenThousand))
                    NextNL = currentLevel;

                Result += CalculateChineseSign((double)prefixNum, NextNL, isDump, IsExceptTen);
                if ((int)currentLevel >= (int)(NumLevel.TenThousand))
                {
                    Result += NumLeverChineseSign[(int)currentLevel];
                }
            }

            //是否跳位
            // 判断是否加零， 比如302 就要给三百 后面加零，变为 三百零二。
            if (IsDumpLevel(Num))
            {
                Result += NumChineseCharacter[0];
                isDump = true;

            }

            //余数是否需要递归
            if (postfixNun > 0)
            {
                Result += CalculateChineseSign(postfixNun, NL, isDump, false);
            }
            else if (postfixNun == 0 && currentLevel > NumLevel.Yuan)
            {
                //当数字是以零元结尾的加上 元整 比如1000000一百万元整
                if (NL == null)
                {
                    Result += NumLeverChineseSign[(int)NumLevel.Yuan];
                    Result += EndOfInt;
                }
            }

            return Result;
        }

        //9：外部调用的转换方法。
        /// <summary>
        /// 外部调用的转换方法
        /// </summary>
        /// <param name="Num"></param>
        /// <returns></returns>
        public string ConvertToChinese(decimal Num, bool checkValue)
        {
            bool isNegative = false;
            if (checkValue && !IsValidated<string>(Num.ToString()))
            {
                throw new OverflowException("数值格式不正确，请输入小于9999亿兆的数字且最多精确的分的金额！");
            }
            if (Num < 0)
            {
                Num = Num * -1;
                isNegative = true;
            }
            string chineseCharactor = string.Empty;
            if (IsBigThanTillion(Num.ToString()))
            {
                double[] tillionNums = SplitNum(Num.ToString());
                chineseCharactor = ContactNumChinese(tillionNums);
            }
            else
            {
                double dNum = Convert.ToDouble(Num.ToString());
                chineseCharactor = CalculateChineseSign(dNum, null, true, IsStartOfTen(dNum));
            }
            if (isNegative)
                return "负" + chineseCharactor;
            return chineseCharactor;
        }

    }
}

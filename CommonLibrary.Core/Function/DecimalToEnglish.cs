using System;
using System.Collections.Generic;
using System.Text;

namespace hwj.CommonLibrary.Function
{
    internal class DecimalToEnglish
    {
        public static bool HasAnd = false;
        public static string HKMoneytoEng(decimal Num)
        {
            HasAnd = false;
            if (Num >= decimal.Zero)
            {
                if (Num == decimal.Zero)
                    return "ZERO";
                string StrNum = Num.ToString();
                string FinalStr = string.Empty;
                string IntPart = string.Empty;
                string StrInteger = string.Empty;
                bool NoFloat = IsNoFloat(StrNum);

                if (!NoFloat)
                {
                    HasAnd = true;
                    string StrFloat = getFloat(StrNum);
                    string FloatPart = MoneytoEng(StrFloat.PadRight(2, '0'));
                    StrInteger = getInteger(StrNum);
                    IntPart = MoneytoEng(StrInteger);

                    string[] b = IntPart.Split(' ');
                    List<string> NewList = new List<string>();
                    for (int i = 0; i < b.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(b[i]))
                            NewList.Add(b[i]);
                    }
                    //NewList.Insert(NewList.Count - 1, "AND");
                    for (int j = 0; j < NewList.Count; j++)
                    {
                        if (j == 0)
                            FinalStr += NewList[j];
                        else
                            FinalStr += " " + NewList[j];
                     }

                    if (IntPart != "ZERO ")
                        FinalStr = FinalStr + " AND " + FloatPart + " CENTS ONLY";
                    else
                        FinalStr = FloatPart + "CENTS ONLY";
                }
                else
                {
                    StrInteger = getInteger(StrNum);
                    IntPart = MoneytoEng(StrInteger);

                    string[] b = IntPart.Split(' ');
                    List<string> NewList = new List<string>();
                    for (int i = 0; i < b.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(b[i]))
                            NewList.Add(b[i]);
                    }
                    //NewList.Insert(NewList.Count - 1, "AND");
                    for (int j = 0; j < NewList.Count; j++)
                    {
                        if (j == 0)
                            FinalStr += NewList[j];
                        else
                            FinalStr += " " + NewList[j];
                    }

                    FinalStr += " ONLY";
                }
                FinalStr=FinalStr.TrimStart(' ');
                if (FinalStr.StartsWith("AND"))
                    return FinalStr.Remove(0, 4);
                else
                    return FinalStr;
            }
            else
                return "Negative Number!";
        }
        public static string MoneytoEng(string n)
        {
            // string result="";
            string m = getFloat(n);
            string strfloat = " point" + m.Replace("0", " zero").Replace("1", " one").Replace("2", " two").Replace("3", " three").Replace("4", " four").Replace("5", " five").Replace("6", " six").Replace("7", " seven").Replace("8", " eight").Replace("9", " nine");
            string[] b = new string[6];
            int i = 0;
            double t = double.Parse(n);
            if (getInteger(n) == "0")
            {
                b[0] = "zero ";
            }
            // for (i=0;t>=1000.0;i++)
            else
            {
                do
                {

                    int k = int.Parse(getInteger(t.ToString())) % 1000;
                    b[i] = getNum3(k);
                    t = t / 1000.0;
                    //if (int.Parse(getInteger(t.ToString())) < 1000 & int.Parse(getInteger(t.ToString())) > 0)
                    //{
                        switch (i)
                        {
                            case 0:
                                if (k != 0)
                                {
                                    b[i] = getNum3(int.Parse(getInteger(t.ToString()))) +" thousand "+ b[i];
                                }
                                else
                                {
                                    b[i] = getNum3(int.Parse(getInteger(t.ToString()))) +" thousand "+ b[i];
                                }
                                break;
                            case 1:
                                if (k != 0)
                                {
                                    b[i] = getNum3(int.Parse(getInteger(t.ToString()))) + " million " + b[i];
                                }
                                else
                                {
                                    b[i] = getNum3(int.Parse(getInteger(t.ToString()))) + " million " + b[i];
                                    b[i - 1] = b[i - 1].Replace(" thousand ", "");
                                }
                                break;
                            case 2:
                                if (k != 0)
                                {
                                    b[i] = getNum3(int.Parse(getInteger(t.ToString()))) + " billion " + b[i];
                                }
                                else
                                {
                                    b[i] = getNum3(int.Parse(getInteger(t.ToString()))) + " billion " + b[i];
                                    b[i - 1] = b[i - 1].Replace(" million ", "");
                                }
                                break;
                            case 3:
                                if (k != 0)
                                {
                                    b[i] = getNum3(int.Parse(getInteger(t.ToString()))) + " trillion " + b[i];
                                }
                                else
                                {
                                    b[i] = getNum3(int.Parse(getInteger(t.ToString()))) + " trillion " + b[i];
                                    b[i - 1] = b[i - 1].Replace(" billion ", "");
                                }
                                break;
                            default:
                                b[i] = " the number is too large!!!";
                                break;
                        }
                    //}
                    //else if (int.Parse(getInteger(t.ToString())) >= 1000)
                    //{
                    //    switch (i)
                    //    {
                    //        case 0:
                    //            b[i] = " thousand " + b[i];
                    //            break;
                    //        case 1:
                    //            b[i] = " million " + b[i];
                    //            break;
                    //        case 2:
                    //            b[i] = " billion " + b[i];
                    //            break;
                    //        case 3:
                    //            b[i] = " trillion " + b[i];
                    //            break;
                    //        default:
                    //            b[i] = " the number is too large!!!";
                    //            break;
                    //    }
                    //}
                   
                    i++;
                } while (t >= 1000.0);
            }
            // return result;
            string result = (b[5] + b[4] + b[3] + b[2] + b[1] + b[0]).ToUpper();
            result = result.Trim();
            if (result.StartsWith("THOUSAND"))
                result = result.Remove(0, 9);
            return result;

            //0-999

        }
        public static string MoneytoEngWithAnd(string n)
        {
            // string result="";
            string m = getFloat(n);
            string strfloat = " point" + m.Replace("0", " zero").Replace("1", " one").Replace("2", " two").Replace("3", " three").Replace("4", " four").Replace("5", " five").Replace("6", " six").Replace("7", " seven").Replace("8", " eight").Replace("9", " nine");
            string[] b = new string[6];
            int i = 0;
            double t = double.Parse(n);
            if (getInteger(n) == "0")
            {
                b[0] = "zero";
            }
            // for (i=0;t>=1000.0;i++)
            else
            {
                do
                {

                    int k = int.Parse(getInteger(t.ToString())) % 1000;
                    b[i] = getNum3(k);
                    t = t / 1000.0;
                    if (int.Parse(getInteger(t.ToString())) < 1000 & int.Parse(getInteger(t.ToString())) > 0)
                    {
                        switch (i)
                        {
                            case 0:
                                if (k != 0)
                                {
                                    b[i] = getNum3(int.Parse(getInteger(t.ToString()))) + " thousand and " + b[i];
                                }
                                else
                                {
                                    b[i] = getNum3(int.Parse(getInteger(t.ToString()))) + " thousand " + b[i];
                                }
                                break;
                            case 1:
                                if (k != 0)
                                {
                                    b[i] = getNum3(int.Parse(getInteger(t.ToString()))) + " million and " + b[i];
                                }
                                else
                                {
                                    b[i] = getNum3(int.Parse(getInteger(t.ToString()))) + " million " + b[i];
                                }
                                break;
                            case 2:
                                if (k != 0)
                                {
                                    b[i] = getNum3(int.Parse(getInteger(t.ToString()))) + " billion and " + b[i];
                                }
                                else
                                {
                                    b[i] = getNum3(int.Parse(getInteger(t.ToString()))) + " billion " + b[i];
                                }
                                break;
                            case 3:
                                if (k != 0)
                                {
                                    b[i] = getNum3(int.Parse(getInteger(t.ToString()))) + " trillion and " + b[i];
                                }
                                else
                                {
                                    b[i] = getNum3(int.Parse(getInteger(t.ToString()))) + " trillion " + b[i];
                                }
                                break;
                            default:
                                b[i] = " the number is too large!!!";
                                break;
                        }
                    }
                    else if (int.Parse(getInteger(t.ToString())) >= 1000)
                    {
                        switch (i)
                        {
                            case 0:
                                b[i] = " thousand " + b[i];
                                break;
                            case 1:
                                b[i] = " million " + b[i];
                                break;
                            case 2:
                                b[i] = " billion " + b[i];
                                break;
                            case 3:
                                b[i] = " trillion " + b[i];
                                break;
                            default:
                                b[i] = " the number is too large!!!";
                                break;
                        }
                    }
                    // while(i>=0)
                    // {
                    // result=result+b[i];
                    // i--;
                    // }
                    i++;
                } while (t >= 1000.0);
            }
           
            return b[5] + b[4] + b[3] + b[2] + b[1] + b[0] + strfloat;

            //0-999

        }
        private static bool IsNoFloat(string n)
        {
            string str = getFloat(n);
            if (str == "0" || str == "00" || str == "000" || str == "0000")
                return true;
            else
                return false;
        }
        private static string getNum3(int n)
        {
            string m = n.ToString();
            
            string[] b = new string[3];
            string str = "";
            if (n >= 100 & n < 1000)
            {
                b[0] = m.Substring(0, 1).Replace("0", "").Replace("1", "one hundred ").Replace("2", "two hundred ").Replace("3", "three hundred ").Replace("4", "four hundred ").Replace("5", "five hundred ").Replace("6", "six hundred ").Replace("7", "seven hundred ").Replace("8", "eight hundred ").Replace("9", "nine hundred ");
                if (!HasAnd && n % 100 != 0)
                {
                    b[0] =  b[0] +" AND ";
                }
                else if(!HasAnd)
                {
                    b[0] = " AND " +b[0]  ;
                }
                if (m.Substring(1, 1) == "1")
                {

                    str = m.Substring(1, 2).Replace("10", "ten ").Replace("11", "eleven ").Replace("12", "twelve ").Replace("13", "thirteen ").Replace("14", "fourteen ").Replace("15", "fifteen ").Replace("16", "sixteen ").Replace("17", "seventeen ").Replace("18", "eighteen ").Replace("19", "nineteen ");
                }
                else if (m.Substring(1, 1) == "0")
                {
                    str = m.Substring(1, 2).Replace("00", "").Replace("01", "one ").Replace("02", "two ").Replace("03", "three ").Replace("04", "four ").Replace("05", "five ").Replace("06", "six ").Replace("07", "seven ").Replace("08", "eight ").Replace("09", "nine ");
                }
                else
                {
                    b[1] = m.Substring(1, 1).Replace("2", "twenty ").Replace("3", "thirty ").Replace("4", "forty ").Replace("5", "fifty ").Replace("6", "sixty ").Replace("7", "seventy ").Replace("8", "eighty ").Replace("9", "ninety ");
                    b[2] = m.Substring(2, 1).Replace("0", "").Replace("1", "one ").Replace("2", "two ").Replace("3", "three ").Replace("4", "four ").Replace("5", "five ").Replace("6", "six ").Replace("7", "seven ").Replace("8", "eight ").Replace("9", "nine ");
                    str = b[1] + b[2];
                }
               
            }
            else if (n < 100 & n >= 10)
            {
                b[0] = "";
                if (m.Substring(0, 1) == "1")
                {
                    str = m.Substring(0, 2).Replace("10", "ten ").Replace("11", "eleven ").Replace("12", "twelve ").Replace("13", "thirteen ").Replace("14", "fourteen ").Replace("15", "fifteen ").Replace("16", "sixteen ").Replace("17", "seventeen ").Replace("18", "eighteen ").Replace("19", "nineteen ");
                }
                else
                {
                    b[1] = m.Substring(0, 1).Replace("2", "twenty ").Replace("3", "thirty ").Replace("4", "forty ").Replace("5", "fifty ").Replace("6", "sixty ").Replace("7", "seventy ").Replace("8", "eighty ").Replace("9", "ninety ");
                    b[2] = m.Substring(1, 1).Replace("0", "").Replace("1", "one ").Replace("2", "two ").Replace("3", "three ").Replace("4", "four ").Replace("5", "five ").Replace("6", "six ").Replace("7", "seven ").Replace("8", "eight ").Replace("9", "nine ");
                    str = b[1] + b[2];
                }
                if (!HasAnd)
                {
                    str = " AND " + str;
                }
            }
            else if (n >= 0 & n < 10)
            {
                b[0] = "";
                b[1] = "";
                b[2] = m.Substring(0, 1).Replace("0", "").Replace("1", "one ").Replace("2", "two ").Replace("3", "three ").Replace("4", "four ").Replace("5", "five ").Replace("6", "six ").Replace("7", "seven ").Replace("8", "eight ").Replace("9", "nine ");
                str = b[1] + b[2];
                if (!HasAnd&&n!=0)
                {
                    str = " AND " + str;
                }
            }
            HasAnd = true;
            return b[0] + str;
        }
        private static string getInteger(string n)
        {
            string[] a;
            a = n.Split('.');
            string s = a[0];
            return s;
        }
        private static string getFloat(string n)
        {
            if (n.Replace("0", "").Replace("1", "").Replace("2", "").Replace("3", "").Replace("4", "").Replace("5", "").Replace("6", "").Replace("7", "").Replace("8", "").Replace("9", "") == "")
            {
                n = n + ".0";
            }
            string[] a;
            a = n.Split('.');
            string s = a[1];
            return s;
        }
        public static string GetMoneyStr(double num)//总
        {
            try
            {
                string m_point = "圆";
                string m_sign;
                int m_len;
                //int g_max=20;
                int g_dec = 4;
                string m_srcint;
                string m_srcdec;
                string m_retv = "";
                int m_cntr = 0;
                string m_decstr;

                if (num >= 0.00)
                {
                    m_sign = "";
                }
                else
                {
                    m_sign = "负";
                    num = num * (-1);
                }
                m_srcint = num.ToString().Trim();//m_srcint--"5658626456.235"
                string[] s_cut = m_srcint.Split(new char[] { '.' });
                if (s_cut.Length == 2)
                {
                    if (s_cut[0].ToString().Trim().Length > 15)
                    {
                        m_retv = m_sign + m_retv + m_srcint + "转换失败!";
                        return m_retv;

                    }
                    if (s_cut[1].ToString().Trim().Length > 4)
                    {
                        m_srcint = m_srcint.Substring(0, m_srcint.Length - (s_cut[1].ToString().Trim().Length - 4));
                    }
                }
                else if (s_cut.Length == 1)
                {
                    m_srcint = m_srcint + ".0000";
                }
                else
                {
                    return num.ToString() + "转换失败!";
                }
                s_cut = m_srcint.Split(new char[] { '.' }, 2);

                m_srcdec = s_cut[1].ToString().Trim();
                m_srcdec = m_srcdec + "0000";
                m_srcdec = m_srcdec.Substring(0, 4);

                m_srcint = s_cut[0].ToString().Trim();
                m_len = m_srcint.Length;

                string m_chr, m_last = "", m_this = "", m_cnzero = Num2cn("0"), m_cnname, m_lbase, m_tbase = Len2cnbase(m_len);
                m_lbase = m_tbase;
                for (m_cntr = 1; m_cntr <= m_len; m_cntr++)
                {
                    m_chr = m_srcint.Substring(m_cntr - 1, 1);
                    m_this = Num2cn(m_chr);
                    m_cnname = Len2cnname(m_len - m_cntr + 1);
                    m_tbase = Len2cnbase(m_len - m_cntr + 1);
                    if (m_lbase == m_tbase)
                    {
                        if (m_last == m_cnzero && m_this == m_cnzero)
                        { }
                        else
                        {
                            if ((m_this == m_cnzero))
                                m_retv = m_retv + m_this + "";
                            else
                                m_retv = m_retv + m_this + m_cnname;
                        }
                    }
                    else
                    {
                        if (m_retv.Length >= 2)
                        {
                            if (m_retv.Substring(m_retv.Length - 2, 2) == m_cnzero)
                            {
                                m_retv = m_retv.Substring(0, m_retv.Length - 2);
                                m_last = "";
                            }
                        }
                        m_retv = m_retv + m_lbase;
                        if (m_last == m_cnzero && m_this == m_cnzero)
                        { }
                        else
                        {
                            if ((m_this == m_cnzero))
                                m_retv = m_retv + m_this + "";
                            else
                                m_retv = m_retv + m_this + m_cnname;
                        }
                    }
                    m_lbase = m_tbase;
                    m_last = m_this;
                }
                if (m_retv.Length - 2 >= 0)
                {
                    if (m_retv.Substring(m_retv.Length - 2, 2) == m_cnzero)
                    {
                        m_retv = m_retv.Substring(0, m_retv.Length - 2);
                    }
                }
                m_retv = m_retv + m_point;
                m_decstr = "";

                if (double.Parse(m_srcdec) == 0)
                {
                    if (m_point == "圆")
                        m_retv = m_retv + "整";
                    else
                        m_retv = m_retv + "";
                }
                else
                {
                    m_len = g_dec;
                    m_this = "";
                    m_last = "";
                    for (m_cntr = m_len; m_cntr > 0; m_cntr--)
                    {
                        m_chr = m_srcdec.Substring(m_cntr - 1, 1);
                        m_this = Num2cn(m_chr);
                        if (m_point == "圆")
                        {

                            m_cnname = Jedec(m_cntr);
                            if (m_this == m_cnzero && null == m_decstr)
                            { }
                            else
                            {
                                if (m_last == m_cnzero && m_this == m_cnzero)
                                { }
                                else
                                {
                                    if (m_this == m_cnzero)
                                        m_decstr = m_this + "" + m_decstr;
                                    else
                                        m_decstr = m_this + m_cnname + m_decstr;
                                }
                            }
                        }
                        else
                        {
                            m_cnname = "";
                            if (m_this == m_cnzero && null == m_decstr)
                            { }
                            else
                                m_decstr = m_this + m_decstr;

                        }
                        m_last = m_this;
                    }
                }
                string myretu = m_sign + m_retv + m_decstr;
                if (myretu.Substring(myretu.Length - 1, 1) == "零")
                    return myretu.Substring(0, myretu.Length - 1);
                else
                    return myretu;
            }
            catch
            {
                return num.ToString() + "转换失败!";
            }

        }
        #region 1--"角" 2--"分"
        private static string Jedec(int num)
        {
            string retu = "";
            switch (num)
            {
                case 1:
                    retu = "角";
                    break;
                case 2:
                    retu = "分";
                    break;
                case 3:
                    retu = "厘";
                    break;
                case 4:
                    retu = "毫";
                    break;
                default:
                    retu = "";
                    break;
            }
            return retu;
        }

        #endregion
        #region cn >=14 && cn<=19--"兆"
        private static string Len2cnbase(int cn)
        {
            string retu = "";
            if (cn >= 14 && cn <= 19)
            {
                retu = "兆";
            }
            else if (cn >= 9 && cn <= 13)
            {
                retu = "亿";
            }
            else if (cn >= 5 && cn <= 8)
            {
                retu = "万";
            }
            else if (cn < 5)
            {
                retu = "";
            }
            else
            {
                retu = "N/A";
            }
            return retu;
        }
        #endregion
        #region 1--"",2--"拾"
        private static string Len2cnname(int len)
        {
            string retu = "";
            switch (len)
            {
                case 1:
                    retu = "";
                    break;
                case 2:
                    retu = "拾";
                    break;
                case 3:
                    retu = "佰";
                    break;
                case 4:
                    retu = "仟";
                    break;
                case 5:
                    retu = "";
                    break;
                case 6:
                    retu = "拾";
                    break;
                case 7:
                    retu = "佰";
                    break;
                case 8:
                    retu = "仟";
                    break;
                case 9:
                    retu = "";
                    break;
                case 10:
                    retu = "拾";
                    break;
                case 11:
                    retu = "佰";
                    break;
                case 12:
                    retu = "仟";
                    break;
                case 13:
                    retu = "万";
                    break;
                case 14:
                    retu = "";
                    break;
                case 15:
                    retu = "拾";
                    break;
                case 16:
                    retu = "佰";
                    break;
                default:
                    retu = "N/A";
                    break;
            }
            return retu;
        }
        #endregion
        #region "0"--"零" 1--"壹"
        private static string Num2cn(string numchr)
        {
            string retu = "";
            switch (numchr)
            {
                case "0":
                    retu = "零";
                    break;
                case "1":
                    retu = "壹";
                    break;
                case "2":
                    retu = "贰";
                    break;
                case "3":
                    retu = "叁";
                    break;
                case "4":
                    retu = "肆";
                    break;
                case "5":
                    retu = "伍";
                    break;
                case "6":
                    retu = "陆";
                    break;
                case "7":
                    retu = "柒";
                    break;
                case "8":
                    retu = "捌";
                    break;
                case "9":
                    retu = "玖";
                    break;
                default:
                    retu = numchr;
                    break;
            }
            return retu;
        }
        #endregion
    }
}

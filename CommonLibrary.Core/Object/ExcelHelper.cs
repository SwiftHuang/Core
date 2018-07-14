using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;

namespace hwj.CommonLibrary.Object
{
    public class ExcelHelper
    {
        /// <summary>
        /// 获取Excel中的Sheet.
        /// </summary>
        /// <param name="excelFile">文件完整路径</param>
        /// <param name="hdrIsNo">HDR=Yes，这代表第一行是标题，不做为数据使用 ，如果用HDR=NO，则表示第一行不是标题，做为数据来使用。系统默认的是YES</param>
        /// <param name="IMEX">0 ---输出模式;1---输入模式;2----链接模式(完全更新能力)</param>
        /// <returns></returns>
        public static List<string> GetExcelSheetNames(string excelFile, bool hdrIsNo = false, int IMEX = 1)
        {
            System.Data.DataTable dt = null;
            //此連接只能操作Excel2007之前(.xls)文件
            //string strConn = "Provider=Microsoft.Jet.OleDb.4.0;" + "data source=" + excelFile + ";Extended Properties='Excel 8.0; HDR=NO; IMEX=1'"; 
            //此連接可以操作.xls與.xlsx文件
            string strConn = string.Format("Provider=Microsoft.Ace.OleDb.12.0;data source={0};Extended Properties='Excel 12.0; HDR={1}; IMEX={2}'"
                , excelFile
                , hdrIsNo ? "NO" : "YES"
                , IMEX);

            using (OleDbConnection objConn = new OleDbConnection(strConn))
            {
                objConn.Open();
                dt = objConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                if (dt == null)
                {
                    return null;
                }
                List<string> lst = new List<string>();
                foreach (DataRow row in dt.Rows)
                {
                    lst.Add(row["TABLE_NAME"].ToString());
                }
                return lst;
            }
        }
        /// <summary>
        /// 获取Excel中指定Sheet的内容
        /// </summary>
        /// <param name="excelFile">文件完整路径</param>
        /// <param name="sheetName">指定获取的Sheet名</param>
        /// <param name="hdrIsNo">HDR=Yes，这代表第一行是标题，不做为数据使用 ，如果用HDR=NO，则表示第一行不是标题，做为数据来使用。系统默认的是YES</param>
        /// <param name="IMEX">0 ---输出模式;1---输入模式;2----链接模式(完全更新能力)</param>
        /// <returns></returns>
        public static DataTable ExcelToDataTable(string excelFile, string sheetName, bool hdrIsNo = false, int IMEX = 1)
        {
            //此連接只能操作Excel2007之前(.xls)文件
            //string strConn = "Provider=Microsoft.Jet.OleDb.4.0;" + "data source=" + excelFile + ";Extended Properties='Excel 8.0; HDR=NO; IMEX=1'"; 
            //此連接可以操作.xls與.xlsx文件
            string strConn = string.Format("Provider=Microsoft.Ace.OleDb.12.0;data source={0};Extended Properties='Excel 12.0; HDR={1}; IMEX={2}'"
                , excelFile
                , hdrIsNo ? "NO" : "YES"
                , IMEX);

            using (OleDbConnection conn = new OleDbConnection(strConn))
            {
                OleDbDataAdapter oada = new OleDbDataAdapter(string.Format("SELECT * FROM [{0}]", sheetName), conn);
                DataSet ds = new DataSet();
                oada.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
                return null;
            }
        }
    }
}

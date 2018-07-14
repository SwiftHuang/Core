using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


namespace hwj.CommonLibrary.WebSite.Base
{
    public class BasePrintControl : UserControl
    {
        public int PrintPageSize { get; set; }
        public bool Export { get; set; }
        public string FileName { get; set; }
        public string PrintTitle { get; set; }
        public string PrintParams { get; set; }
        public BasePrintControl()
            : base()
        {
            Export = false;
            if (!string.IsNullOrEmpty(FileName))
                FileName = FileName + ".xls";
            else
                FileName = DateTime.Now.ToString("yyyyMMddHHMMss") + ".xls";
        }
        protected override void Render(HtmlTextWriter writer)
        {
            if (Export)
            {
                HttpResponse Rp = Response;
                Rp.Clear(); //清除缓冲区流中的所有内容输出
                Rp.Buffer = true;
                Rp.Charset = "UTF-8"; //"GB2312"   //设置输出流的http字符集
                //保存附件用"attachment;filename=bang.xls";在线打开用"online;filename=bang.xls"
                //可以是.doc、.xls、.txt、.htm、
                Rp.AppendHeader("Content-Disposition", "attachment;filename=" + FileName);
                Rp.ContentEncoding = System.Text.Encoding.UTF8;//.GetEncoding("GB2312");//设置输出流为简体中文
                //设置输出文件类型为excel文件。保存为word时，应为"application/ms-word" 
                //可以为application/ms-excel、application/ms-word、application/ms-txt、application/ms-html、或其他浏览器可直接支持文档　 
                Rp.ContentType = "application/ms-excel";
                System.Globalization.CultureInfo myCItrad = new System.Globalization.CultureInfo("ZH-CN", true);//区域设置
                System.IO.StringWriter oStringWriter = new System.IO.StringWriter(myCItrad);
                System.Web.UI.HtmlTextWriter oHtmlTextWriter = new System.Web.UI.HtmlTextWriter(oStringWriter);
                base.Render(oHtmlTextWriter);
                Rp.Write(oStringWriter.ToString());

                Rp.End();//将当前所有缓冲的输出
            }
            else
                base.Render(writer);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;

namespace hwj.CommonLibrary.WebSite
{
    /// <summary>
    /// JavaScript Helper
    /// </summary>
    public class JSHelper
    {
        public static void Alert(Page page, string text)
        {
            Alert(page, text, null);
        }
        public static void Alert(Page page, string text, string key)
        {
            page.ClientScript.RegisterStartupScript(page.GetType(), GeneralScriptKey(key), "alert('" + text + "');", true);
        }

        public static void ExecScript(Page page, string script)
        {
            ExecScript(page, script, null);
        }
        public static void ExecScript(Page page, string script, string key)
        {
            if (!string.IsNullOrEmpty(script))
            {
                page.ClientScript.RegisterStartupScript(page.GetType(), GeneralScriptKey(key), script, true);
            }
        }

        //public static void AjaxAlert(Control control, string text)
        //{
        //    AjaxAlert(control, text);
        //}
        //public static void AjaxAlert(Control control, string text, string key)
        //{
        //    ScriptManager.RegisterStartupScript(control, control.GetType(), GeneralScriptKey(key), "alert('" + text + "');", true);
        //}

        //public static void AjaxExecScript(Control control, string script)
        //{
        //    AjaxExecScript(control, script, null);
        //}
        //public static void AjaxExecScript(Control control, string script, string key)
        //{
        //    if (!string.IsNullOrEmpty(script))
        //    {
        //        ScriptManager.RegisterStartupScript(control, control.GetType(), GeneralScriptKey(key), script, true);
        //    }
        //}

        public static string GeneralScriptKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                int iSeed = 0;
                string sDateKey;

                sDateKey = DateTime.Now.ToString("yyyymmddhhmmss");
                iSeed = Convert.ToInt16(sDateKey.Substring(sDateKey.Length - 1));
                Random ra = new Random(iSeed);

                return String.Format("{0}{1}", sDateKey, ra.Next());
            }
            else
            {
                return key;
            }

        }
    }
}

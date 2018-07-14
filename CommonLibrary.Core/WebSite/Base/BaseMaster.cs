using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace hwj.CommonLibrary.WebSite.Base
{
    public class BaseMaster : MasterPage
    {
        public BaseMaster()
        {

        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
        }

        protected override void OnUnload(EventArgs e)
        {
           base.OnUnload(e);
        }
    }
}

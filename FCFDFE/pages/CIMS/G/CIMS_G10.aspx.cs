using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Content;

namespace FCFDFE.pages.CIMS.G
{
    public partial class CIMS_G10 : System.Web.UI.Page
    {
        public string strMenuName = "", strMenuNameItem = "";
        Common FCommon = new Common();
        protected void G11_Click(object sender, EventArgs e)
        {
            
        }

        protected void G12_Click(object sender, EventArgs e)
        {
            Response.Redirect("CIMS_G11.aspx");
        }

        protected void G13_Click(object sender, EventArgs e)
        {
            Response.Redirect("CIMS_G12.aspx");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);
        }
    }
}
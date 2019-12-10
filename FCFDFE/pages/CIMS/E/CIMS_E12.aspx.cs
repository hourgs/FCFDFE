using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCFDFE.Content;

namespace FCFDFE.pages.CIMS.E
{
    public partial class CIMS_E12 : System.Web.UI.Page
    {
        Common FCommon = new Common();
        public string strMenuName = "", strMenuNameItem = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (FCommon.getAuth(this))
                FCommon.getPageNode(this, ref strMenuName, ref strMenuNameItem);

        }

        protected void btnArmy_Click(object sender, EventArgs e)
        {
            Response.Redirect("CIMS_E12_1.aspx");
        }

        protected void btnCivi_Click(object sender, EventArgs e)
        {
            Response.Redirect("CIMS_E12_2.aspx");
        }
    }
}